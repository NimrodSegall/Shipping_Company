﻿using UnityEngine;

namespace RoadTypes
{
    public class RoadStraight : RoadBase, IRoadInterface
    {

        public void SetCreateDirection(string newDir)
        {
            createDirection = newDir;
        }

        public bool IsDirectionConnectable(string direction)
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
