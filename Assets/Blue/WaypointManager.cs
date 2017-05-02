
using System.Collections.Generic;
using UnityEngine;
using Blue.Waypoints;

// by @Bullrich

public class WaypointManager : MonoBehaviour {
    public List<Waypoint> waypoints = new List<Waypoint>();

    void Awake() {
        foreach (Transform t in transform)
            waypoints.Add(t.GetComponent<Waypoint>());
        SetByOrder(waypoints);
    }

    void SetByOrder(List<Waypoint> listWP) {
        for (int i = 0; i < listWP.Count; i++) {
            int next = (i + 1 < listWP.Count ? i + 1 : 0);
            listWP[i].next = listWP[next];
        }
    }

    [System.Obsolete("Not being used because it separates the groups. Use SetByOrder instead")]
    public void SetWaypoints(List<Waypoint> listWP) {
        for (int i = 0; i < listWP.Count; i++) {
            float startDistance = 99f;
            int selectedWaypoint = 0;
            for (int j = 0; j < listWP.Count; j++) {
                float distanceBetweenWaypoints = (listWP[i].transform.position - listWP[j].transform.position).sqrMagnitude;
                if (distanceBetweenWaypoints < startDistance)
                    if (listWP[j].Next != listWP[i] &&
                        listWP[j] != listWP[i] &&
                        //waypoints[j].Next == null &&
                        !listWP[j].hasWaypoint) {
                        startDistance = distanceBetweenWaypoints;
                        selectedWaypoint = j;
                    }
            }
            listWP[i].next = listWP[selectedWaypoint];
            listWP[selectedWaypoint].hasWaypoint = true;
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