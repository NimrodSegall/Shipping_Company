using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float carLength = 1f;

    private int carLayerMask = 1 << 8;

    [SerializeField]
    private float baseSpeed = 7f;

    [SerializeField]
    private float lifeTime = 20f;

    [SerializeField]
    private float detectionDistance = 10f;

    private Rigidbody rb = null;

    [SerializeField]
    private LaneConnection currentNode = null;
    [SerializeField]
    private LaneConnection nextNode = null;

    private Rigidbody carInfrontRb = null;
    private float newSpeed = 0f;
    private float stoppingDistance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        baseSpeed = Random.Range(4f, 10f);
        newSpeed = baseSpeed;
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNode != null)
        {
            nextNode = currentNode.next;
            if (nextNode != null)
            {
                transform.LookAt(nextNode.transform);
            }
            rb.velocity = newSpeed * transform.forward;
            if (ReachedNextNode())
            {
                currentNode = nextNode.next;
            }
        }

        GetCarInfront();
        if(carInfrontRb != null)
        {
            ChangeVelocityToAvoidCollision();
        }
        else
        {
            newSpeed = baseSpeed;
        }

    }

    public void SetStartingNode(LaneConnection startingNode)
    {
        currentNode = startingNode;
    }

    private bool ReachedNextNode()
    {
        float distance = (transform.position - nextNode.transform.position).magnitude;
        return distance < 0.1f;
    }

    private void ChangeVelocityToAvoidCollision()
    {
        float distance = (transform.position - carInfrontRb.transform.position).magnitude - (carLength / 2);
        newSpeed = baseSpeed * ((distance - stoppingDistance) / detectionDistance);
    }

    private void GetCarInfront()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, detectionDistance, carLayerMask, QueryTriggerInteraction.Collide);
        if(hitInfo.collider != null)
        {
            carInfrontRb = hitInfo.collider.GetComponent<Rigidbody>();
        }
        else
        {
            carInfrontRb = null;
        }
    }
}
