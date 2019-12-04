using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    private WaypointNavigatorController controller = null;
    public Waypoint currentWaypoint = null;
    public Waypoint targetWaypoint = null;

    public List<Waypoint> path = new List<Waypoint>();
    [SerializeField]
    private int pathPosition = 0;
    private bool isGuided = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<WaypointNavigatorController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypoint != null)
        {
            if (isGuided)
            {
                GuidedMotion();
            }
            else
            {
                RandomMotion();
            }
        }
        else
        {
            Stop();
        }

    }

    public void NavigateByPath(List<Waypoint> path)
    {
        this.path = path;
        if (path.Count > 0)
        {
            isGuided = true;
            pathPosition = 0;
            if (pathPosition > path.Count)
            {
                NavigateRandomly();
            }
        }
    }

    public void NavigateRandomly()
    {
        isGuided = false;
        targetWaypoint = null;
    }

    public void Stop()
    {
        controller.SetDestination(transform.position);
    }

    private void GuidedMotion()
    {
        if (controller.ReachedDestination())
        {
            currentWaypoint = path[pathPosition];
            pathPosition++;
            if (currentWaypoint == targetWaypoint || pathPosition > path.Count - 1)
            {
                NavigateRandomly();
            }
            controller.SetDestination(currentWaypoint.GetPosition());
        }
    }

    private void RandomMotion()
    {
        if (controller.ReachedDestination())
        {
            if (currentWaypoint?.nexts?.Count > 0)
            {
                int choice = Random.Range(0, currentWaypoint.nexts.Count);
                currentWaypoint = currentWaypoint.nexts[choice];
                controller.SetDestination(currentWaypoint.GetPosition());
            }

            /*
            bool shouldBranch = false;
            if (currentWaypoint.branches != null && currentWaypoint.branches.Length > 0)
            {
                shouldBranch = Random.Range(0f, 1f) < currentWaypoint.branchRatio;
            }

            if (shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Length - 1)];
            }

            if (currentWaypoint.next != null)
            {
                currentWaypoint = currentWaypoint.next;
                controller.SetDestination(currentWaypoint.GetPosition());

            }
            */
        }
    }

    public void SetInitialWp(Waypoint wp, Vector3 forward, bool doRandomMotion)
    {
        currentWaypoint = wp;
        transform.forward = forward;
        
        if(doRandomMotion)
        {
            NavigateRandomly();
        }
        controller = GetComponent<WaypointNavigatorController>();
        controller.SetDestination(wp.GetPosition());
        
    }
}
