using UnityEngine;

namespace RoadTypes
{
    public class RoadStraight : RoadBase, IRoadInterface
    {

        public void SetCreateDirection(int newDir)
        {
            createDirection = newDir;
        }

        public bool IsDirectionConnectable(int direction)
        {
            Vector3 vecDir = DirToVec(direction);
            // Is the direction transform.forward or -transform.forward?
            if (Utilities.IsAlmostOne(Mathf.Abs(Vector3.Dot(vecDir, transform.forward))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDirectionConnectable(Vector3 direction)
        {
            Vector3 vecDir = direction;
            // Is the direction transform.forward or -transform.forward?
            if (Utilities.IsAlmostOne(Mathf.Abs(Vector3.Dot(vecDir, transform.forward))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateRoad(RoadBase prevRoad, float gridSize, int newOrientation, Vector3[] newPosVec)
        {
            if (prevRoad != null)
            {
                transform.position = prevRoad.transform.position + DirToVec(prevRoad.createDirection) * gridSize;
                createDirection = RotateRelativeTo(StrToInd("forward"), prevRoad.createDirection);
                orientation = prevRoad.createDirection;

                transform.forward = DirToVec(prevRoad.createDirection);
            }
        }

        public void CreateRoad(RoadBase prevRoad, float gridSize)
        {
            if (prevRoad != null)
            {
                transform.position = prevRoad.transform.position + DirToVec(prevRoad.createDirection) * gridSize;
                createDirection = RotateRelativeTo((int)directions.forward, prevRoad.createDirection);
                orientation = prevRoad.createDirection;

                transform.forward = DirToVec(prevRoad.createDirection);
            }
        }
    }
}
