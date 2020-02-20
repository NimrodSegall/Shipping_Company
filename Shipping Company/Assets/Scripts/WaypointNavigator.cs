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

    private float loadingTime = 10f;
    private float lastLoadingEventTime = 0f;

    private readonly int roadsLayer = 1 << 10;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<WaypointNavigatorController>();
        if (currentWaypoint == null)
        {
            currentWaypoint = FindNearestWp();
            isGuided = false;
        }
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

    private void GuidedMotion()
    {
        if (controller.ReachedDestination())
        {
            if (currentWaypoint.isLoadingSpot)
            {
                LoadCargo();
            }

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
                while(currentWaypoint.nexts[choice].isLoadingSpot)
                {
                    choice = (choice + 1) % currentWaypoint.nexts.Count;
                }
                currentWaypoint = currentWaypoint.nexts[choice];
                controller.SetDestination(currentWaypoint.GetPosition());
            }
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

    private void LoadCargo()
    {
        if (Time.time > lastLoadingEventTime + loadingTime)
        {
            lastLoadingEventTime = Time.time;
        }
        else
        {
            controller.ToggleStop(true);
        }
    }

    private Waypoint FindNearestWp()
    {
        Collider nearestCol = Utilities.FindNearestCollider(transform.position, 2f, roadsLayer);
        Waypoint nearestWp = nearestCol.GetComponent<RoadTypes.RoadBase>().lanes[0]?.GetComponent<Waypoint>();
        return nearestWp;
    }
}
