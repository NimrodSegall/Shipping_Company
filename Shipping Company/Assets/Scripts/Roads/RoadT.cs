using UnityEngine;

namespace RoadTypes
{
    public class RoadT : RoadBase, IRoadInterface
    {

        public void SetOutLanes()
        {
            string dir = GlobalDirToRelative(createDirection);
            dir = RotateRight(dir, -RotationDistance("forward", orientation));
            switch (dir)
            {
                case "right":
                    lanesOut[0] = lanes[2];
                    lanesOut[1] = lanes[3];
                    break;

                case "backward":
                    lanesOut[0] = lanes[0];
                    lanesOut[1] = lanes[1];
                    break;

                case "left":
                    lanesOut[0] = lanes[4];
                    lanesOut[1] = lanes[5];
                    break;

                default:
                    lanesOut[0] = lanes[2];
                    lanesOut[1] = lanes[3];
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
            if (IsAlmostOne(Vector3.Dot(vecDir, -transform.forward)) || IsAlmostOne(Vector3.Dot(vecDir, transform.right))
                || IsAlmostOne(Vector3.Dot(vecDir, -transform.right)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetInLanes(string direction)
        {
            string relativeDirection = RelativeDirection(direction);
            switch (relativeDirection)
            {
                case "forward":
                    lanesIn[0] = lanes[2];
                    lanesIn[1] = lanes[3];
                    break;

                case "right":
                    lanesIn[0] = lanes[2];
                    lanesIn[1] = lanes[3];
                    break;

                case "backward":
                    lanesIn[0] = lanes[0];
                    lanesIn[1] = lanes[1];
                    break;

                case "left":
                    lanesIn[0] = lanes[4];
                    lanesIn[1] = lanes[5];
                    break;

                default:
                    lanesIn[0] = lanes[2];
                    lanesIn[1] = lanes[3];
                    break;
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
