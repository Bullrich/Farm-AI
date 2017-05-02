
using System.Collections.Generic;
using UnityEngine;
using Blue.Waypoints;

// by @Bullrich

public class WaypointManager : MonoBehaviour {
    public List<Waypoint> waypoints = new List<Waypoint>();

    void Start() {
        foreach (Transform t in transform)
            waypoints.Add(t.GetComponent<Waypoint>());
        SetByOrder();
    }

    void SetByOrder() {
        for (int i = 0; i < waypoints.Count; i++) {
            int next = (i + 1 < waypoints.Count ? i + 1 : 0);
            waypoints[i].next = waypoints[next];
        }
    }

    [System.Obsolete("Not being used because it separates the groups. Use SetByOrder instead")]
    public void SetWaypoints() {
        for (int i = 0; i < waypoints.Count; i++) {
            float startDistance = 99f;
            int selectedWaypoint = 0;
            for (int j = 0; j < waypoints.Count; j++) {
                float distanceBetweenWaypoints = (waypoints[i].transform.position - waypoints[j].transform.position).sqrMagnitude;
                if (distanceBetweenWaypoints < startDistance)
                    if (waypoints[j].Next != waypoints[i] &&
                        waypoints[j] != waypoints[i] &&
                        //waypoints[j].Next == null &&
                        !waypoints[j].hasWaypoint) {
                        startDistance = distanceBetweenWaypoints;
                        selectedWaypoint = j;
                    }
            }
            waypoints[i].next = waypoints[selectedWaypoint];
            waypoints[selectedWaypoint].hasWaypoint = true;
        }
    }

    public Waypoint GetClosestWaypoint(Vector3 referencePosition) {
        float closestWaypoint = 9999f;
        Waypoint selectedWaypoint = null;
        foreach(Waypoint way in waypoints) {
            float distanceToWaypoint = (way.transform.position - referencePosition).sqrMagnitude;
            if(distanceToWaypoint < closestWaypoint) {
                closestWaypoint = distanceToWaypoint;
                selectedWaypoint = way;
            }
        }
        return selectedWaypoint;
    }
}