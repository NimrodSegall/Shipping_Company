using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private float speed = 7f;

    [SerializeField]
    private float lifeTime = 20f;

    private Rigidbody rb = null;

    [SerializeField]
    private LaneConnection currentNode = null;
    [SerializeField]
    private LaneConnection nextNode = null;

    // Start is called before the first frame update
    void Start()
    {
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
            rb.velocity = speed * transform.forward;
            if(ReachedNextNode())
            {
                currentNode = nextNode.next;
            }
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
}
