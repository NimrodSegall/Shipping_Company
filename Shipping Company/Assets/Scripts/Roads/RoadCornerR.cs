using UnityEngine;

namespace RoadTypes
{
    public class RoadCornerR : RoadBase, IRoadInterface
    {
        public void SetCreateDirAndLanesOut(string newDir)
        {
            createDirection = newDir;
            SetOutLanes();
        }

        public void SetOutLanes()
        {
            if (IsAlmostOne(Vector3.Dot(DirToVec(createDirection), transform.right)))
            {
                lanesOut[0] = lanes[0];
                lanesOut[1] = lanes[1];
            }
            else
            {
                lanesOut[0] = lanes[1];
                lanesOut[1] = lanes[0];
            }
        }

        public void SetInLanes(GameObject[] inLanes)
        {
            lanesIn[0] = inLanes[0];
            lanesIn[1] = inLanes[1];
        }

        // Is the direction transform.forward or transform.right ?
        public bool IsDirectionConnectable(string direction)
        {
            Vector3 vecDir = DirToVec(direction);
            if (IsAlmostOne(Vector3.Dot(vecDir, -transform.forward)) || IsAlmostOne(Vector3.Dot(vecDir, transform.right)))
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
            if (IsAlmostOne(Vector3.Dot(vecDir, transform.forward)) || IsAlmostOne(Vector3.Dot(vecDir, transform.right)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateRoad(RoadBase prevRoad, float gridSize)
        {
            if (prevRoad != null)
            {
                transform.position = prevRoad.transform.position + DirToVec(prevRoad.createDirection) * gridSize;
                createDirection = RotateRelativeTo("forward", prevRoad.createDirection);
                
                
                orientation = prevRoad.createDirection;
                transform.forward = DirToVec(prevRoad.createDirection);
                SetOutLanes();
                ConnectToPrevRoad(prevRoad);
            }
        }
    }
}
