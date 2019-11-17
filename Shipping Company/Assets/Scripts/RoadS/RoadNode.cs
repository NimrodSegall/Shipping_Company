using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNode : MonoBehaviour
{
    public RoadNode[] nexts;
    public RoadNode[] prevs;

    [HideInInspector]
    public int numOfNexts = 0;
    [HideInInspector]
    public int numOfPrevs = 0;

    public Transform nextDirection = null;

    private float searchDistance = 50f;

    private int nodeLayerMask = 1 << 9;

    private int maxPrevsSize = 4;

    // Start is called before the first frame update
    void Awake()
    {
        prevs = new RoadNode[maxPrevsSize];
        if (nexts.Length < 1)
        {
            nexts = SearchNext(nextDirection);
        }
        numOfNexts = nexts.Length;
    }

    private void Start()
    {
        AssignPrevToNexts();
    }

    public void AddPrevious(RoadNode newPrev)
    {
        prevs[numOfPrevs] = newPrev;
        numOfPrevs++;
    }

    private void AssignPrevToNexts()
    {
        foreach (RoadNode next in nexts)
        {
            if (next != null)
            {
                next.AddPrevious(this);
            }
        }
    }

    private RoadNode[] SearchNext(Transform searchDirection)
    {
        Vector3 direction;
        // If direction was not specified, the next node will be infront of the current one (or non-existent)
        if (nextDirection != null)
        {
            direction = searchDirection.position - transform.position;
        }
        else
        {
            direction = searchDistance * transform.forward;
        }

        // If the next node wasn't already specified, there is only one next node (or null)
        nexts = new RoadNode[1];
        bool hit = Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, direction.magnitude, nodeLayerMask, QueryTriggerInteraction.Collide);
        if (hit)
        {
            nexts[0] = hitInfo.collider.GetComponent<RoadNode>();
        }
        else
        {
            nexts[0] = null;
        }
        return nexts;
    }
}
