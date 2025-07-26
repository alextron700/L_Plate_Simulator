using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// this turns Waypoints into Routes.
public class WaypointRouteManager : MonoBehaviour
{
    [Tooltip("drag and drop waypoint GameObjects here, IN ORDER. NOTE: WAYPOINTS SHOULD HAVE COORDINATES")]
    public List<CarAINavigationCheckpoint> routeWaypoints = new List<CarAINavigationCheckpoint>();
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        if (routeWaypoints == null || routeWaypoints.Count < 1) return; // return if there are no waypoints in route
        Gizmos.color = Color.yellow;
        for (int i = 0; i < routeWaypoints.Count - 1; i++) { 
            if(routeWaypoints[i] != null && routeWaypoints[i=1] != null)
            {
                Gizmos.DrawLine(routeWaypoints[i].transform.position, routeWaypoints[i + 1].transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
