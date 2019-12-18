using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using System;

namespace Pathfinding
{
    public class Pathfinder
    {
        public RoadGraph graph;

        public Pathfinder(RoadGraph graph)
        {
            this.graph = graph;
        }

        public List<RoadNode> ClosestLeadingTo(Waypoint target)
        {
            List<RoadNode> nodes = new List<RoadNode>();
            Waypoint current = target;
            bool found = false;
            List<Waypoint> paths = GetPaths(current);
            while (!found)
            {
                foreach (Waypoint path in paths.ToArray())
                {
                    if (path?.graphNode != null)
                    {
                        found = true;
                        nodes.Add(path.graphNode);
                        paths.Remove(path);
                    }
                    else
                    {
                        paths.Remove(path);
                        paths.AddRange(GetPaths(path));
                    }
                }
            }
            return nodes;
        }

        public RoadNode ClosestFrom(Waypoint start)
        {
            Waypoint wp = start;
            while(wp.graphNode == null)
            {
                wp = wp.nexts[0];
            }
            return wp.graphNode;
        }

        public RoadEdge[] Djikstra(Waypoint start, Waypoint target)
        {
            // Find some explanations here: https://www.geeksforgeeks.org/dijkstras-shortest-path-algorithm-greedy-algo-7/
            ResetGraph();
            RoadNode startingNode = ClosestFrom(start);
            RoadNode endNode = ClosestFrom(target);

            startingNode.minDist = 0f;
            startingNode.minDistFrom = null;


            // Hash function just returns node id. Wastes some memory, but quick to implement
            RoadNode[] checkedList = new RoadNode[graph.nodes.Count];
            SimplePriorityQueue<RoadNode> priorityQueue = new SimplePriorityQueue<RoadNode>();
            foreach (RoadNode node in graph.nodes)
            {
                priorityQueue.Enqueue(node, node.minDist);
            }

            bool stopCondition = GetStopCondition(checkedList, endNode);
            while (!stopCondition)
            {
                RoadNode checkedNode = priorityQueue.Dequeue();
                checkedList[checkedNode.id] = checkedNode;
                for (int i = 0; i < checkedNode.nexts.Count; i++)
                {
                    if(checkedNode.minDist + checkedNode.nextEdges[i].weight < checkedNode.nexts[i].minDist)
                    {
                        checkedNode.nexts[i].minDist = checkedNode.minDist + checkedNode.nextEdges[i].weight;
                        checkedNode.nexts[i].minDistFrom = checkedNode.nextEdges[i];
                    }
                }
                stopCondition = GetStopCondition(checkedList, endNode);
            }

            List<RoadEdge> reversePath = new List<RoadEdge>();
            RoadNode currentRoad = endNode;
            while(currentRoad.minDist != 0)
            {
                reversePath.Add(currentRoad.minDistFrom);
                currentRoad = currentRoad.minDistFrom.from;
            }

            RoadEdge[] path = reversePath.ToArray();

            Array.Reverse(path, 0, path.Length);
            

            return path;
        }

        public void ResetGraph()
        {
            foreach (RoadNode node in graph.nodes)
            {
                node.minDist = Mathf.Infinity;
                node.waypoint.leadingTo = null;
                node.minDistFrom = null;
            }
        }

        private List<Waypoint> GetPaths(Waypoint wp)
        {
            Waypoint current = FindLastBranch(wp);

            List<Waypoint> paths = new List<Waypoint>();
            paths.AddRange(current.prevs);

            foreach(Waypoint path in paths.ToArray())
            {
                path.leadingTo = current;
                if (path.isBranch)
                {
                    paths.Remove(path);
                    paths.AddRange(path.prevs);
                    foreach(Waypoint newPath in path.prevs)
                    {
                        newPath.leadingTo = path;
                    }
                }
            }

            return paths;
        }

        private Waypoint FindLastBranch(Waypoint wp)
        {
            Waypoint current = wp;
            while (current.prevs.ToArray().Length < 2)
            {
                current.prevs[0].leadingTo = current;
                current = current.prevs[0];
            }
            return current;
        }

        private bool GetStopCondition(RoadNode[] chckedList, RoadNode target)
        {
            return chckedList[target.id] == target;
        }

        private int IndexOfMinVal(RoadNode[] nodeArray)
        {
            int minInd = -1;
            float minVal = Mathf.Infinity;
            for (int i = 0; i < nodeArray.Length; i++)
            {
                if(nodeArray[i].minDist < minVal)
                {
                    minVal = nodeArray[i].minDist;
                    minInd = i;
                }
            }
            return minInd;
        }
    }
}
