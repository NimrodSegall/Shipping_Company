using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    public class RoadNode
    {
        public List<RoadNode> nexts = new List<RoadNode>();
        public List<RoadNode> prevs = new List<RoadNode>();

        public List<RoadEdge> nextEdges = new List<RoadEdge>();
        public List<RoadEdge> prevEdges = new List<RoadEdge>();

        public Waypoint waypoint = null;

        public bool beenAddedToGraph = false;

        public int id = 0;

        public float minDist = Mathf.Infinity;
        public RoadEdge minDistFrom = null;

        public RoadNode(Waypoint startingWaypoint)
        {
            Waypoint[] waypointsToNearestNext = ClosestNodableWaypoint(startingWaypoint);
            if(waypointsToNearestNext.Length > 0)
            waypoint = waypointsToNearestNext[waypointsToNearestNext.Length - 1];
        }

        public void GetNexts()
        {
            if (waypoint != null)
            {
                Waypoint[] paths = GetPaths();
                AssignNexts(paths);
            }
        }

        public RoadEdge NodeToEdge(RoadNode target)
        {
            foreach(RoadEdge edge in nextEdges)
            {
                if(edge.to == target)
                {
                    return edge;
                }
            }
                return null;
        }

        private Waypoint[] GetPaths()
        {
            Waypoint[] paths;
            if (waypoint != null)
            {
                paths = waypoint.nexts.ToArray();
            }
            else
            {
                paths = null;
            }
            return paths;
        }

        private void AssignNexts(Waypoint[] paths)
        {
            foreach (Waypoint path in paths)
            {
                Waypoint[] waypointsToNearestNext = ClosestNodableWaypoint(path);
                Waypoint nextWaypoint = waypointsToNearestNext[waypointsToNearestNext.Length - 1];
                if (nextWaypoint != null)
                {
                    if (nextWaypoint.graphNode == null)
                    {
                        RoadNode next = new RoadNode(nextWaypoint);
                        nextWaypoint.graphNode = next;

                    }
                    nexts.Add(nextWaypoint.graphNode);
                    nextWaypoint.graphNode.prevs.Add(this);

                    RoadEdge edge = new RoadEdge(this, nextWaypoint.graphNode, waypointsToNearestNext);
                    nextEdges.Add(edge);
                    nextWaypoint.graphNode.prevEdges.Add(edge);
                }
            }
        }

        private Waypoint[] ClosestNodableWaypoint(Waypoint current)
        {
            List<Waypoint> allWpsOnEdge = new List<Waypoint>();
            Waypoint wp = current;
            allWpsOnEdge.Add(wp);
            while (wp.nexts.ToArray().Length < 1)
            {
                wp = wp.nexts[0];
                allWpsOnEdge.Add(wp);
            }
            return allWpsOnEdge.ToArray();
        }

        public void Visualize(bool showNode, bool showPathsToNexts)
        {
            if (waypoint != null)
            {
                if (showNode)
                {
                    GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    marker.transform.position = waypoint.transform.position;
                }
                if (showPathsToNexts)
                {
                    foreach (RoadNode node in nexts)
                    {
                        if (node.waypoint != null)
                        {
                            waypoint.DebugRay(node.waypoint);
                        }
                    }
                }
            }
        }
    }
}
