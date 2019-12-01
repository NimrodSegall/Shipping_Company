using UnityEngine;

namespace RoadTypes
{
    public interface IRoadInterface
    {
        bool IsDirectionConnectable(string direction);
        void SetCreateDirection(string newDir);
        void CreateRoad(RoadBase prevRoad, float gridSize, string newOrientation, Vector3[] newPosVec);
    }
}
