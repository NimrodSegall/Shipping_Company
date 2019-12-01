using UnityEngine;
using System.Collections.Generic;

public class RoadNode
{
    public List<RoadNode> nexts = new List<RoadNode>();
    public List<RoadNode> prevs;

    public Waypoint waypoint = null;

    public bool beenInitialized = false;
    public bool beenAddedToGraph = false;

    public RoadNode(Waypoint startingWaypoint)
    {
        waypoint = ClosestNodableWaypoint(startingWaypoint);
    }

    public void GetNexts()
    {
        if (waypoint != null)
        {
            Waypoint[] paths = GetPaths();
            AssignNexts(paths);
        }
        beenInitialized = true;
    }

    private Waypoint ClosestNodableWaypoint(Waypoint current)
    {
        if(current.branches.Length > 0)
        {
            return current;
        }
        else
        {
            return ClosestNodableWaypoint(current.next);
        }
    }

    private Waypoint[] GetPaths()
    {
        Waypoint[] paths;
        if (waypoint != null)
        {
            paths = new Waypoint[waypoint.branches.Length + 1];
            paths[0] = waypoint.next;
            for (int i = 1; i < waypoint.branches.Length + 1; i++)
            {
                paths[i] = waypoint.branches[i - 1];
            }
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
            Waypoint nextWaypoint = ClosestNodableWaypoint(path);
            if (nextWaypoint != null)
            {
                if (nextWaypoint.graphNode == null)
                {
                    RoadNode next = new RoadNode(nextWaypoint);
                    nextWaypoint.graphNode = next;

                }
                nexts.Add(nextWaypoint.graphNode);
            }
        }
    }

    public void ShowPathToNexts()
    {
        if (waypoint != null)
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
