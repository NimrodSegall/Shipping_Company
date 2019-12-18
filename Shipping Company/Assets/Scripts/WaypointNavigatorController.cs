using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigatorController : MonoBehaviour
{
    [SerializeField]
    public float baseSpeed = 10f;
    public float rotationSpeed = 10f;

    private float stopDistance = 2.5f;

    private Vector3 destination;

    private float currentSpeed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.events.onPauseGame += OnPauseGame;
        GameEvents.events.onUnpauseGame += OnUnpauseGame;
        currentSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget(destination);
    }

    private void MoveTowardsTarget(Vector3 target)
    {
        RotateDirection(target);
        transform.position += currentSpeed * transform.forward * Time.deltaTime;
    }

    public void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
    }

    public bool ReachedDestination()
    {
        return (destination - transform.position).magnitude < stopDistance;
    }

    public void ToggleStop(bool stop)
    {
        if(stop)
        {
            currentSpeed = 0f;
        }
        else
        {
            currentSpeed = baseSpeed;
        }
    }

    private void RotateDirection(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnPauseGame()
    {
        currentSpeed = 0f;
    }

    private void OnUnpauseGame()
    {
        currentSpeed = baseSpeed;
    }
}
