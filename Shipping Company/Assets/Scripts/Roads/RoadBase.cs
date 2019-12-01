using UnityEngine;

namespace RoadTypes
{
    [SelectionBase]
    public class RoadBase : MonoBehaviour
    {
        public static string[] directions = { "forward", "right", "backward", "left" };

        public GameObject[] lanes = null;

        public string createDirection = "forward";
        public string orientation = "forward";

        public static int DirToInd(string dir)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                if (directions[i] == dir)
                    return i;
            }
            return -1;
        }

        public static Vector3 DirToVec(string dir)
        {
            switch (dir)
            {
                case "forward":
                    return Vector3.forward;
                case "right":
                    return Vector3.right;
                case "backward":
                    return -Vector3.forward;
                case "left":
                    return -Vector3.right;
            }
            return Vector3.forward;
        }

        public static Vector3 DirToVec(string dir, GameObject relativeTo)
        {
            switch (dir)
            {
                case "forward":
                    return relativeTo.transform.forward;

                case "right":
                    return relativeTo.transform.right;

                case "backward":
                    return -relativeTo.transform.forward;

                case "left":
                    return -relativeTo.transform.right;
            }
            return relativeTo.transform.forward;
        }

        public static string GlobalDirToRelative(string dir)
        {
            Vector3[] vecDirs = { Vector3.forward, Vector3.right, -Vector3.forward, -Vector3.right };
            for (int i = 0; i< vecDirs.Length; i++)
            {
                if(Utilities.IsAlmostOne(Vector3.Dot(DirToVec(dir), vecDirs[i])))
                {
                    return VecToDir(vecDirs[i]);
                }
            }
            return "";
        }

        public static string VecToDir(Vector3 dir)
        {
            Vector3[] vecDirs = { Vector3.forward, Vector3.right, -Vector3.forward, -Vector3.right };
            string[] strDirs = { "forward", "right", "backward", "left" };
            for (int i = 0; i < vecDirs.Length; i++)
            {
                if (Utilities.IsAlmostOne(Vector3.Dot(dir, vecDirs[i])))
                {
                    return strDirs[i];
                }
            }
            return null;
        }

        public int RotationDistance(string dir1, string dir2)
        {
            return (DirToInd(dir2) - DirToInd(dir1) + directions.Length) % directions.Length;
        }

        public static string RotateClockwise(string direction, int times)
        {
            int numberOfRotations = (DirToInd(direction) + times + directions.Length) % directions.Length;
            return directions[numberOfRotations];
        }


        public string RelativeDirection(string globalDirection)
        {
            int timesToRotate = RotationDistance(globalDirection, orientation);
            return RotateClockwise(globalDirection, timesToRotate);
        }

        public static void ConnectRoads(Waypoint[][] orderedFromConnection)
        {
            Waypoint[] fromWaypoints = orderedFromConnection[0];
            Waypoint[] toWaypoints = orderedFromConnection[1];

            Debug.Log(fromWaypoints[0]);
            Debug.Log(fromWaypoints[1]);

            Debug.Log(toWaypoints[0]);
            Debug.Log(toWaypoints[1]);

            fromWaypoints[0].ConnectToNext(toWaypoints[0]);
            fromWaypoints[1].ConnectToNext(toWaypoints[1]);
        }

        public string RotateRelativeTo(string defaultOrientation, string newOrientation)
        {
            int numOfRots = RotationDistance(defaultOrientation, newOrientation);
            return RotateClockwise(createDirection, numOfRots);
        }

        public static bool IsSumOfDistMin(Vector3 p0, Vector3 p1, Vector3 q0, Vector3 q1)
        {
            float dist00 = (p0 - q0).magnitude;
            float dist10 = (p1 - q1).magnitude;
            float dist01 = (p0 - q1).magnitude;
            float dist11 = (p1 - q1).magnitude;
            return dist00 + dist11 < dist01 + dist10;
        }

        public static bool IsSumOfDistMin(GameObject p0, GameObject p1, GameObject q0, GameObject q1)
        {
            Vector3 ppos0 = p0.transform.position;
            Vector3 qpos0 = q0.transform.position;
            Vector3 ppos1 = p1.transform.position;
            Vector3 qpos1 = q1.transform.position;
            float dist00 = (ppos0 - qpos0).magnitude;
            float dist10 = (ppos1 - qpos1).magnitude;
            float dist01 = (ppos0 - qpos1).magnitude;
            float dist11 = (ppos1 - qpos1).magnitude;
            return dist00 + dist11 < dist01 + dist10;
        }

        public static bool IsSumOfDistMin(Waypoint p0, Waypoint p1, Waypoint q0, Waypoint q1)
        {
            Vector3 ppos0 = p0.transform.position;
            Vector3 qpos0 = q0.transform.position;
            Vector3 ppos1 = p1.transform.position;
            Vector3 qpos1 = q1.transform.position;
            float dist00 = (ppos0 - qpos0).magnitude;
            float dist10 = (ppos1 - qpos0).magnitude;
            float dist01 = (ppos0 - qpos1).magnitude;
            float dist11 = (ppos1 - qpos1).magnitude;
            return dist00 + dist11 < dist01 + dist10;
        }
    }


}
