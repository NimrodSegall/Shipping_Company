using UnityEngine;

namespace RoadTypes
{
    public class RoadCornerR : RoadBase, IRoadInterface
    {
        public void SetCreateDirection(string newDir)
        {
            createDirection = newDir;
        }

        // Is the direction transform.forward or transform.right ?
        public bool IsDirectionConnectable(string direction)
        {
            Vector3 vecDir = DirToVec(direction);
            if (Utilities.IsAlmostOne(Vector3.Dot(vecDir, -transform.forward)) || Utilities.IsAlmostOne(Vector3.Dot(vecDir, transform.right)))
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
            if (Utilities.IsAlmostOne(Vector3.Dot(vecDir, transform.forward)) || Utilities.IsAlmostOne(Vector3.Dot(vecDir, transform.right)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateRoad(RoadBase prevRoad, float gridSize, string newOrientation, Vector3[] newPosVec)
        {
            if (prevRoad != null)
            {
                transform.position = prevRoad.transform.position + DirToVec(prevRoad.createDirection) * gridSize;
                createDirection = RotateRelativeTo("forward", prevRoad.createDirection);
                orientation = prevRoad.createDirection;
                transform.forward = DirToVec(prevRoad.createDirection);
            }
        }
    }
}
