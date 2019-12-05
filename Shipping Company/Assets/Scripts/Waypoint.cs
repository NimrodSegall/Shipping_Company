using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public List<Waypoint> nexts = new List<Waypoint>();
    public List<Waypoint> prevs = new List<Waypoint>();

    public Waypoint leadingTo = null;

    public bool isBranch = false;

    [HideInInspector]
    public float width = 0.5f;

    public Pathfinding.RoadNode graphNode = null;

    public string placeName = "";

    private void Start()
    {
        if(nexts.Count > 1)
        {
            isBranch = true;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void ConnectToNext(Waypoint other)
    {
        if (nexts.Count > 0)
        {
            nexts.RemoveAll(item => item == null);
        }
        if (other.prevs.Count > 0)
        {
            other.prevs.RemoveAll(item => item == null);
        }
        this.nexts.Add(other);
        other.prevs.Add(this);
    }

    public void DebugRay(Waypoint other)
    {
        Debug.DrawRay(transform.position, other.transform.position - transform.position, Color.white, 100f, false);
    }
}
