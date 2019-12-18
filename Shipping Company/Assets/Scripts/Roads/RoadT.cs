﻿using UnityEngine;

namespace RoadTypes
{
    public class RoadT : RoadBase, IRoadInterface
    {
        public void SetCreateDirection(int newDir)
        {
            createDirection = newDir;
        }
        // Is the direction -transform.forward or transform.right or-transform.right?
        public bool IsDirectionConnectable(int direction)
        {
            Vector3 vecDir = DirToVec(direction);
            if (Utilities.IsAlmostOne(Vector3.Dot(vecDir, -transform.forward)) || Utilities.IsAlmostOne(Vector3.Dot(vecDir, transform.right))
                || Utilities.IsAlmostOne(Vector3.Dot(vecDir, -transform.right)))
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
                transform.position = newPosVec[0];
                createDirection = RotateRelativeTo((int)directions.right, newOrientation);
                orientation = newOrientation;
                transform.forward = DirToVec(newOrientation);
            }
        }

        public void CreateRoad(RoadBase prevRoad, float gridSize)
        {
            if (prevRoad != null)
            {
                transform.position = prevRoad.transform.position + DirToVec(prevRoad.createDirection) * gridSize;
                createDirection = RotateRelativeTo((int)directions.right, prevRoad.createDirection);
                orientation = prevRoad.createDirection;
                transform.forward = DirToVec(prevRoad.createDirection);
            }
        }
    }
}
