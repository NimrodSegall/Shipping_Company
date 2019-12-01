using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    private WaypointNavigatorController controller = null;
    public Waypoint currentWaypoint = null;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<WaypointNavigatorController>();
        controller.SetDestination(currentWaypoint.GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.ReachedDestination())
        {
            bool shouldBranch = false;
            if(currentWaypoint.branches != null && currentWaypoint.branches.Length > 0)
            {
                shouldBranch = Random.Range(0f, 1f) < currentWaypoint.branchRatio;
            }

            if(shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Length - 1)];
            }

            if(currentWaypoint.next != null)
            {
                currentWaypoint = currentWaypoint.next;
                controller.SetDestination(currentWaypoint.GetPosition());
                
            }
        }
    }
}
