using UnityEngine;

namespace RoadTypes
{
    // This is a base class from which all other road classes inherent from, together with IRoadInterface
    [SelectionBase]
    public class RoadBase : MonoBehaviour
    {
        public static string[] dirStrings = { "forward", "right", "backward", "left" };
        
        public enum directions
        {
            forward,
            right,
            backward,
            left
        }
        
        public static int directionsLength = 4;

        public GameObject[] lanes = null;

        public int createDirection = 0;
        public int orientation = 0;

        public static int DirToInd(string dir)
        {
            for (int i = 0; i < directionsLength; i++)
            {
                if (dirStrings[i] == dir)
                    return i;
            }
            return -1;
        }

        public static int DirToInd(int dir)
        {
            return dir;
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

        public static Vector3 DirToVec(int dir)
        {
            switch (dir)
            {
                case 0:
                    return Vector3.forward;
                case 1:
                    return Vector3.right;
                case 2:
                    return -Vector3.forward;
                case 3:
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

        public static Vector3 DirToVec(int dir, GameObject relativeTo)
        {
            switch (dir)
            {
                case 0:
                    return relativeTo.transform.forward;

                case 1:
                    return relativeTo.transform.right;

                case 2:
                    return -relativeTo.transform.forward;

                case 3:
                    return -relativeTo.transform.right;
            }
            return relativeTo.transform.forward;
        }

        public static string DirToStr(int dir)
        {
            return dirStrings[dir];
        }

        public static int StrToInd(string dir)
        {
            switch (dir)
            {
                case "forward":
                    return 0;

                case "right":
                    return 1;

                case "backward":
                    return 2;

                case "left":
                    return 3;
            }
            return -1;
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

        public static string GlobalDirToRelative(int dir)
        {
            Vector3[] vecDirs = { Vector3.forward, Vector3.right, -Vector3.forward, -Vector3.right };
            for (int i = 0; i < vecDirs.Length; i++)
            {
                if (Utilities.IsAlmostOne(Vector3.Dot(DirToVec(dir), vecDirs[i])))
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

        /*
        public static int VecToDir(Vector3 dir)
        {
            Vector3[] vecDirs = { Vector3.forward, Vector3.right, -Vector3.forward, -Vector3.right };
            int[] intDirs = {0, 1, 2, 3};
            for (int i = 0; i < vecDirs.Length; i++)
            {
                if (Utilities.IsAlmostOne(Vector3.Dot(dir, vecDirs[i])))
                {
                    return intDirs[i];
                }
            }
            return -1;
        }
        */

        public int RotationDistance(string dir1, string dir2)
        {
            return (DirToInd(dir2) - DirToInd(dir1) + directionsLength) % directionsLength;
        }

        public int RotationDistance(int dir1, int dir2)
        {
            return (dir2 - dir1 + directionsLength) % directionsLength;
        }

        public static string RotateClockwise(string direction, int times)
        {
            int numberOfRotations = (DirToInd(direction) + times + directionsLength) % directionsLength;
            return dirStrings[numberOfRotations];
        }

        public static int RotateClockwise(int direction, int times)
        {
            int numberOfRotations = (direction + times + directionsLength) % directionsLength;
            return StrToInd(dirStrings[numberOfRotations]);
        }


        public string RelativeDirection(string globalDirection)
        {
            int timesToRotate = RotationDistance(StrToInd(globalDirection), orientation);
            return RotateClockwise(globalDirection, timesToRotate);
        }

        public int RelativeDirection(int globalDirection)
        {
            int timesToRotate = RotationDistance(globalDirection, DirToInd(orientation));
            return RotateClockwise(globalDirection, timesToRotate);
        }

        public static void ConnectRoads(Waypoint[][] orderedFromConnection)
        {
            Waypoint[] fromWaypoints = orderedFromConnection[0];
            Waypoint[] toWaypoints = orderedFromConnection[1];

            fromWaypoints[0].ConnectToNext(toWaypoints[0]);
            fromWaypoints[1].ConnectToNext(toWaypoints[1]);
        }

        public string RotateRelativeTo(string defaultOrientation, string newOrientation)
        {
            int numOfRots = RotationDistance(defaultOrientation, newOrientation);
            return RotateClockwise(dirStrings[createDirection], numOfRots);
        }

        public int RotateRelativeTo(int defaultOrientation, int newOrientation)
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
