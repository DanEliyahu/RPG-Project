﻿using UnityEngine;

namespace Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float WAYPOINT_GIZMOS_RADIUS = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), WAYPOINT_GIZMOS_RADIUS);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            return (i + 1) % transform.childCount;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}