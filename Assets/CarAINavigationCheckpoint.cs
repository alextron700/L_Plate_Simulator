using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// this makes a gameobject into a waypoint. simply attach this script to a gameobject.
public class CarAINavigationCheckpoint : MonoBehaviour
{
    public Vector3 position;
    public string[] assignedAction; // action/s assigned to this waypoint
    public float speedLimit;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }
    void onDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 20f);
    }
    // add new sign types here
    // add a new line, with your sign type name, followed by a comma (,)
    public enum WaypointActionType
    {
        None,
        TurnLeft,
        TurnRight,
        StopSign,
        TrafficLight,
        Roundabout,
        SlipLane,
        ChangeLaneLeft,
        ChangeLaneRight,
        GiveWay,
        publicTransitLane,
        Accelerate,
        RouteEnd,
    }
    [System.Serializable]
    public class WaypointAction
    {
        [Header("Turn Specific")]
        [Tooltip("how sharp a given turn action is, from 0.01 ( Very weak) to 1, ( hardest reccomended turn)")]
        public float turnPower = 1f;
        [Header("intersection specific")]
        public bool isRoundabout = false;
        public bool isSliplane = false;
        public bool requiresFullHalt = false;
        public bool isTransitLane = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
