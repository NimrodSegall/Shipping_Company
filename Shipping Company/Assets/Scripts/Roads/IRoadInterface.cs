using UnityEngine;

namespace RoadTypes
{
    public interface IRoadInterface
    {
        bool IsDirectionConnectable(string direction);
        void SetCreateDirAndLanesOut(string newDir);
        void CreateRoad(RoadBase prevRoad, float gridSize);
        void SetOutLanes();
        void SetInLanes(GameObject[] inLanes);
    }
}
