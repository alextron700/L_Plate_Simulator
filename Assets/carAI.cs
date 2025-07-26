using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// this script controls NPC car behaviors. MUST HAVE A Testscript, AND WaypointRouteManager, MUST BE ATTACHED TO A RigidBody
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Testscript))]
[RequireComponent(typeof(WaypointRouteManager))]
public class carAI : MonoBehaviour
{
    public float accelerator; // must be between -1 and 1
    public float steering; // must be between -1 and 1
    private Testscript _carPhysics;
    // Accelerator: + 1 == forward -1 == reverse
    // steering: +1 == right -1 == left
    // Start is called before the first frame update
    public WaypointRouteManager WaypointRoute;
    private int currentWaypoint = 0;
    public float waypointReachedRadius = 5f;// how far can the car be from the waypoint to activate

    void Start()
    {
        _carPhysics = GetComponent<Testscript>();
        if (_carPhysics)
        {
            Debug.Log("Check success");
        }
        else
        {
            Debug.LogError("CarAI must be attached to a Testscript");
            return;
        }
    
        if(WaypointRoute == null) // make sure a route is assigned!
        {
            WaypointRoute = GetComponent<WaypointRouteManager>();
            if(WaypointRoute == null || WaypointRoute.routeWaypoints.Count < 1)
            {
                Debug.LogError("CarAI should be assigned a route!");
                return;
            }
        }
     
    }
    void turnLeft(float turnPower)
    {
        //Debug.Log("turning");
        steering = -1 * Mathf.Abs(turnPower);
        _carPhysics.steering = steering;
        
    }
    void turnRight(float turnPower)
    {
       // Debug.Log("turning");
        steering = Mathf.Abs(turnPower);
        _carPhysics.steering = steering;
    }
    void goForward( float acceleration)
    {
        if(_carPhysics.currentSpeed > _carPhysics.speedLimit)
        {
            accelerator = 0;
        }
        else
        {
            accelerator = Mathf.Abs(acceleration);
        }
        _carPhysics.accelerator = acceleration;
    }

    // Update is called once per frame
    // this is the car's predefined behavior
    void Update()
    {

        if (currentWaypoint < WaypointRoute.routeWaypoints.Count)
        {
            CarAINavigationCheckpoint target = WaypointRoute.routeWaypoints[currentWaypoint];
            Vector3 nextWayPointPos = target.position;
            float distanceToWaypoint = Vector3.Distance(transform.position, nextWayPointPos);
            if (distanceToWaypoint <= waypointReachedRadius)
            {

                currentWaypoint++;

                // if (currentWaypoint >= WaypointRoute.routeWaypoints.Count)
                //  {
                //    goForward(0);
                //     return;
                //}

            }
            if (distanceToWaypoint > waypointReachedRadius)
            {
                goForward(0.5f);
                // Debug.Log(target);
                // Debug.Log(distanceToWaypoint);

            }
            Vector3 AxisDistances = nextWayPointPos - transform.position;
            float Angle = Mathf.Atan2(AxisDistances.x, AxisDistances.z) * Mathf.Rad2Deg;
            steering = Mathf.DeltaAngle(transform.eulerAngles.y, Angle);
            steering /= 180f;
            _carPhysics.steering = steering;
            if (target == null)
            {
                goForward(0);
                return;
            }
            if (target.assignedAction.Length > 0)
            {
                _carPhysics.speedLimit = target.speedLimit;
                // determine correct action for a waypoint
                /*
                 To add a New behavior
                 case "TagValue": //<- make sure there is a matching one in CarAINavigationCheckpoint.cs (MAKE SURE CAPITALISATION MATCHES!)
                      //do stuff
                  break;
                 */
                switch (target.assignedAction[0])
                {
                    case "TurnLeft":
                        if (target.assignedAction.Length > 1  && target.assignedAction[1] != null)
                        {
                            turnLeft(float.Parse(target.assignedAction[1]));
                        }
                        else
                        {
                            turnLeft(0.5f);
                        }
                        break;
                    case "Accelerate":
                        if (target.assignedAction.Length > 1 && target.assignedAction[1] != null)
                        {
                            goForward(float.Parse(target.assignedAction[1]));
                        }
                        else
                        {
                            goForward(0.5f);
                        }
                        break;
                    case "RouteEnd":
                        goForward(0);
                        turnLeft(0);
                        break;
                    case "TurnRight":
                        if ( target.assignedAction.Length > 1 && target.assignedAction[1] != null)
                        {
                            turnRight(float.Parse(target.assignedAction[1]));
                        }
                        else
                        {
                            turnRight(0.5f);
                        }
                        break;
                }

            }

        }
    }
}
