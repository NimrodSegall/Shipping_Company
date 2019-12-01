using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint prev;
    public Waypoint next;

    [Range(0f, 5f)]
    public float width = 0.5f;

    //public List<Waypoint> branches;
    public Waypoint[] branches;
    [Range(0f, 1f)]
    public float branchRatio = 0.5f;

    public RoadNode graphNode = null;

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2;
        Vector3 maxBound = transform.position - transform.right * width / 2;
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }

    public void ConnectToNext(Waypoint other)
    {
        this.next = other;
        other.prev = this;
    }

    public void DebugRay(Waypoint other)
    {
        Debug.DrawRay(transform.position, other.transform.position - transform.position, Color.white, 100f, false);
    }
}
