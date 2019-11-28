using UnityEngine;

namespace RoadTypes
{
    [SelectionBase]
    public class RoadBase : MonoBehaviour
    {
        private static string[] directions = { "forward", "right", "backward", "left" };

        public GameObject[] lanes = null;
        public GameObject[] lanesOut = new GameObject[2];
        //[HideInInspector]
        public GameObject[] lanesIn = new GameObject[2];
        public string createDirection = "forward";
        public string orientation = "forward";

        public static bool IsAlmostOne(float num)
        {
            if (num > 0.98f && num < 1.02f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int DirectionToInd(string checkedDirection)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                if (directions[i] == checkedDirection)
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
                if(IsAlmostOne(Vector3.Dot(DirToVec(dir), vecDirs[i])))
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
                if (IsAlmostOne(Vector3.Dot(dir, vecDirs[i])))
                {
                    return strDirs[i];
                }
            }
            return null;
        }

        public int RotationDistance(string dir1, string dir2)
        {
            return (DirectionToInd(dir2) - DirectionToInd(dir1) + directions.Length) % directions.Length;
        }

        public string RotateRight(string direction, int times)
        {
            int numberOfRotations = (DirectionToInd(direction) + times + directions.Length) % directions.Length;
            return directions[numberOfRotations];
        }


        public string RelativeDirection(string globalDirection)
        {
            int timesToRotate = RotationDistance(globalDirection, orientation);
            return RotateRight(globalDirection, timesToRotate);
        }

        public void ConnectToPrevRoad(RoadBase prevRoad)
        {
            Waypoint lane0Waypoint;
            Waypoint lane1Waypoint;
            if(IsSumOfDistMin(lanesIn[0].GetComponent<Waypoint>(), lanesIn[1].GetComponent<Waypoint>(),
                prevRoad.lanesOut[0].GetComponent<Waypoint>(), prevRoad.lanesOut[1].GetComponent<Waypoint>()))
            {
                lane0Waypoint = lanesIn[0].GetComponent<Waypoint>();
                lane1Waypoint = lanesIn[1].GetComponent<Waypoint>();
            }
            else
            {
                lane0Waypoint = lanesIn[1].GetComponent<Waypoint>();
                lane1Waypoint = lanesIn[0].GetComponent<Waypoint>();
            }
            lane0Waypoint.prev = prevRoad.lanesOut[0].GetComponent<Waypoint>();
            lane0Waypoint.prev.next = lane0Waypoint;

            lane1Waypoint.next = prevRoad.lanesOut[1].GetComponent<Waypoint>();
            lane1Waypoint.next.prev = lane1Waypoint;
        }

        public void ConnectToNextRoad(RoadBase nextRoad)
        {
            Waypoint lane0Waypoint = lanesIn[0].GetComponent<Waypoint>();
            Waypoint lane1Waypoint = lanesIn[1].GetComponent<Waypoint>();
            lane0Waypoint.next = nextRoad.lanesIn[0].GetComponent<Waypoint>();
            lane0Waypoint.next.prev = lane0Waypoint;

            lane1Waypoint.prev = nextRoad.lanesIn[1].GetComponent<Waypoint>();
            lane1Waypoint.prev.next = lane1Waypoint;
        }

        public string RotateRelativeTo(string defaultOrientation, string newOrientation)
        {
            int numOfRots = RotationDistance(defaultOrientation, newOrientation);
            return RotateRight(createDirection, numOfRots);
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
