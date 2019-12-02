using UnityEngine;
using System.Collections.Generic;

namespace Pathfinding
{
    public class RoadGraph
    {
        public List<RoadNode> nodes = new List<RoadNode>();
        public int numOfNodes = 0;

        public RoadGraph(Waypoint startWp)
        { 
            Initialize(startWp);
        }

        public void Add(RoadNode newNode)
        {
            if (!newNode.beenAddedToGraph)
            {
                nodes.Add(newNode);
                newNode.beenAddedToGraph = true;
                numOfNodes++;
                newNode.id = numOfNodes;
            }
        }

        public void Initialize(Waypoint startWp)
        {
            nodes.Add(GetFirstNode(startWp));
            GetAllNodes();
        }

        private RoadNode GetFirstNode(Waypoint startWp)
        {
            RoadNode node = new RoadNode(startWp);
            node.GetNexts();
            return node;
        }

        private void GetAllNodes()
        {
            int initCount = nodes.Count;
            int currentCount = -1;

            while (initCount != currentCount)
            {
                initCount = nodes.Count;
                foreach (RoadNode graphNode in nodes.ToArray())
                {
                    foreach (RoadNode nextNode in graphNode.nexts)
                    {
                        if (!nextNode.beenAddedToGraph)
                        {
                            Add(nextNode);
                            nextNode.GetNexts();
                        }
                    }
                }
                currentCount = nodes.Count;
            }
        }

        public void VisualizeGraph()
        {
            foreach (RoadNode node in nodes)
            {
                node.Visualize(true, false);
            }
        }
    }
}
