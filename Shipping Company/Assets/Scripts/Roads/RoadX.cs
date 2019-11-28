using UnityEngine;

namespace RoadTypes
{
    public class RoadX : RoadBase, IRoadInterface
    {

        public void SetOutLanes()
        {
            string dir = GlobalDirToRelative(createDirection);
            dir = RotateRight(dir, -RotationDistance("forward", orientation));
            switch (dir)
            {
                case "forward":
                    lanesOut[0] = lanes[1];
                    lanesOut[1] = lanes[0];
                    break;

                case "right":
                    lanesOut[0] = lanes[3];
                    lanesOut[1] = lanes[2];
                    break;

                case "backward":
                    lanesOut[0] = lanes[5];
                    lanesOut[1] = lanes[4];
                    break;

                case "left":
                    lanesOut[0] = lanes[7];
                    lanesOut[1] = lanes[6];
                    break;

                default:
                    lanesOut[0] = lanes[1];
                    lanesOut[1] = lanes[0];
                    break;
            }
        }

        public void SetInLanes(GameObject[] inLanes)
        {
            lanesIn[0] = inLanes[0];
            lanesIn[1] = inLanes[1];
        }


        public void SetCreateDirAndLanesOut(string newDir)
        {
            createDirection = newDir;
            SetOutLanes();
        }
        // Is the direction -transform.forward or transform.right or-transform.right?
        public bool IsDirectionConnectable(string direction)
        {
            Vector3 vecDir = DirToVec(direction);
            if (IsAlmostOne(Vector3.Dot(vecDir, transform.forward)) 
                || IsAlmostOne(Vector3.Dot(vecDir, -transform.forward)) 
                || IsAlmostOne(Vector3.Dot(vecDir, transform.right))
                || IsAlmostOne(Vector3.Dot(vecDir, -transform.right)))
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
