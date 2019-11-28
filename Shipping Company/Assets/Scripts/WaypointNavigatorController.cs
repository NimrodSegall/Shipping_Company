using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigatorController : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 10f;

    private float stopDistance = 2.5f;

    private Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget(destination);
    }

    private void MoveTowardsTarget(Vector3 target)
    {
        RotateDirection(target);
        transform.position += speed * transform.forward * Time.deltaTime;
    }

    public void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
    }

    public bool ReachedDestination()
    {
        return (destination - transform.position).magnitude < stopDistance;
    }

    private void RotateDirection(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
