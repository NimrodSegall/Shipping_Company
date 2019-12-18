using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class BuildingAreaVisualizer
{
    public static int numOfRays = 10;
    public static float roadLength = 15f;
    public static int roadLayerMask = 1 << 10;
    /*
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(RoadTypes.RoadStraight road, GizmoType gizmoType)
    {
        List<Vector3> points = FindEdgeOfBuildableArea(road);

        Gizmos.color = Color.green * 0.8f;
        foreach(Vector3 point in points)
        {
            Gizmos.DrawSphere(point, 1f);
        }
    }

    public static List<Vector3> FindEdgeOfBuildableArea(RoadTypes.RoadStraight road)
    {
        List<Vector3> hitsPoints = new List<Vector3>();
        for (int i = 0; i < numOfRays; i++)
        {
            Vector3 origin = (road.transform.position - (road.transform.forward * roadLength / 2)) + road.transform.forward * (i + 1 / numOfRays);
            Vector3 originRight = origin + road.transform.right * ((roadLength / 2));
            Vector3 originLeft = origin - road.transform.right * ((roadLength / 2));
            hitsPoints.Add(originRight);
            hitsPoints.Add(originLeft);


            if (Physics.Raycast(originRight, road.transform.right, out RaycastHit hitInfoRight, 100000f, roadLayerMask))
            {
                hitsPoints.Add(hitInfoRight.point);
                
            }
            if (Physics.Raycast(originLeft, -road.transform.right, out RaycastHit hitInfoLeft))
            {
                hitsPoints.Add(hitInfoLeft.point);
                
            }
        }
        return hitsPoints;
    }
    */

}
