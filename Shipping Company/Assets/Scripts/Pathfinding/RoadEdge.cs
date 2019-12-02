namespace Pathfinding
{
    public class RoadEdge
    {
        public RoadNode from;
        public RoadNode to;
        public Waypoint[] waypoints;
        public float weight;

        public RoadEdge(RoadNode from, RoadNode to, Waypoint[] waypoints)
        {
            this.from = from;
            this.to = to;
            this.waypoints = waypoints;
            this.weight = GetWeight(waypoints.Length);
        }
        
        private float GetWeight(int stepsDist)
        {
            return stepsDist * GameParameters.gridSize;
        }
    }
}

