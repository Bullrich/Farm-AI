using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by @Bullrich

namespace Blue.Waypoints
{
	public class Waypoint : MonoBehaviour {
        public Waypoint next;
        public float nearDistance = 0.1f;

        public Waypoint Next { get { return next; } }
        public float NearDistance { get { return nearDistance; } }

        public bool IsNear(Vector3 position) {
            return (position - transform.position).sqrMagnitude < nearDistance * nearDistance;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, .2f);

            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, nearDistance);

            Gizmos.color = Color.magenta;
            if (next != null)
                Gizmos.DrawLine(transform.position, next.transform.position);
        }
    }
}
