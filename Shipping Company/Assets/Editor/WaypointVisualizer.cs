using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class WaypointVisualizer
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
    {
        if((gizmoType & GizmoType.Selected) !=0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, 0.1f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2f), 
            waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

        foreach (Waypoint prev in waypoint.prevs)
        {
            if (prev != null)
            {
                Gizmos.color = Color.red;
                Vector3 offset = waypoint.transform.right * waypoint.width / 2f;
                Vector3 offsetTo = prev.transform.right * prev.width / 2f;
                Gizmos.DrawLine(waypoint.transform.position + offset, prev.transform.position + offsetTo);
            }
        }

        foreach(Waypoint next in waypoint.nexts)
        {
            if (next != null)
            {
                Gizmos.color = Color.green;
                Vector3 offset = -waypoint.transform.right * waypoint.width / 2f;
                Vector3 offsetTo = -next.transform.right * next.width / 2f;

                Gizmos.DrawLine(waypoint.transform.position + offset, next.transform.position + offsetTo);
            }
        }

        /*
        if (waypoint.prevs.Count > 0)
        {
            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.width / 2f;
            Vector3 offsetTo = waypoint.prev.transform.right * waypoint.prev.width / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.prev.transform.position + offsetTo);
        }
        

        if(waypoint.next != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = -waypoint.transform.right * waypoint.width / 2f;
            Vector3 offsetTo = -waypoint.next.transform.right * waypoint.next.width / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.next.transform.position + offsetTo);
        }
        */

        if(waypoint.isBranch)
        {
            foreach(Waypoint branch in waypoint.nexts)
            {
                if (branch != null)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(waypoint.transform.position, branch.transform.position);
                }
            }
        }
    }

}
