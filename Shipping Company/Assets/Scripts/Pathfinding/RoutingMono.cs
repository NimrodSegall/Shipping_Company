using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class RoutingMono : MonoBehaviour
    {
        [SerializeField]
        public Waypoint startWp;

        [SerializeField]
        public Waypoint targetWp;

        public List<Waypoint> path = new List<Waypoint>();

        private Pathfinder pathfinder;
        private RoadGraph graph;

        [SerializeField]
        private WaypointNavigator car = null;

        void Start()
        {

            if (startWp != null)
            {
                graph = new RoadGraph(startWp);
            }

            if(targetWp != null && graph != null)
            {
                pathfinder = new Pathfinder(graph);

                RoadEdge[] edgePath = pathfinder.Djikstra(startWp, targetWp);
                List<Waypoint> wpPath = EdgePathToWaypoints(edgePath);
                path = wpPath;
            }

            if(car != null)
            {
                car.NavigateByPath(path);
            }
        }

        private List<Waypoint> EdgePathToWaypoints(RoadEdge[] edgePath)
        {
            List<Waypoint> wpPath = new List<Waypoint>();
            foreach(RoadEdge edge in edgePath)
            {
                wpPath.AddRange(edge.waypoints);
            }
            return wpPath;
        }

        public void SetCarNav()
        {
            if (car != null)
            {
                startWp = car.currentWaypoint;
            }
            if (car != null && targetWp != null)
            {
                car.targetWaypoint = targetWp;
            }
        }
    }
}
