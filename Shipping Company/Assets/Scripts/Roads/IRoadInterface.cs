using UnityEngine;

namespace RoadTypes
{
    // This is a base interface from which all other road classes inherent from, together with RoadBase
    public interface IRoadInterface
    {
        bool IsDirectionConnectable(int direction);
        void SetCreateDirection(int newDir);
        void CreateRoad(RoadBase prevRoad, float gridSize, int newOrientation, Vector3[] newPosVec);
        void CreateRoad(RoadBase prevRoad, float gridSize);
    }
}
