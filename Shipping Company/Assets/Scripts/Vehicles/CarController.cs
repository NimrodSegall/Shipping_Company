using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //public static int turnSeed = (int)Random.Range(0, 2147483647);  //Number between [0, INT_MAX)
    public int turnID = 0;
    public float carLength = 1f;

    private int carLayerMask = 1 << 8;

    [SerializeField]
    private float baseSpeed = 7f;

    [SerializeField]
    private float lifeTime = 1000f;

    [SerializeField]
    private float detectionDistance = 10f;

    private Rigidbody rb = null;

    [SerializeField]
    private RoadNode currentNode = null;
    [SerializeField]
    private RoadNode nextNode = null;

    private float newSpeed = 0f;
    private float stoppingDistance = 0f;

    private float rechedNodeDistance = 0.5f;

    private CarControllerData controllerData = null;

    // Start is called before the first frame update
    void Start()
    {
        controllerData = GetCarControllerData();
        turnID = GetTurnID();

        baseSpeed = Random.Range(5, 10f);
        newSpeed = baseSpeed;
        rb = GetComponent<Rigidbody>();

        if(currentNode != null)
        {
            SetStartingNode(currentNode);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentNode != null)
        {
            GoToNextNode();
            if(ReachedNextNode())
            {
                SetCurrentAndNext(nextNode);
            }
        }
    }

    public void SetStartingNode(RoadNode newCurrentNode)
    {
        if(controllerData == null)
        {
            controllerData = GetCarControllerData();
            turnID = GetTurnID();
        }
        //SetCurrentAndNext(newCurrentNode);
        currentNode = newCurrentNode;
        transform.position = newCurrentNode.transform.position;
        if (currentNode.nexts.Length > 0)
        {
            nextNode = newCurrentNode.nexts[0];
            ChangeLookingDirection(nextNode);
        }
        else
        {
            nextNode = null;
            transform.rotation = newCurrentNode.transform.rotation;
        }
    }

    private void SetCurrentAndNext(RoadNode newCurrentNode)
    {
        currentNode = newCurrentNode;
        nextNode = GetNextNode(turnID);
        ChangeLookingDirection(nextNode);
        turnID += controllerData.turnRandomNum;
    }

    private bool ReachedNextNode()
    {
        if (nextNode != null)
        {
            float distance = (transform.position - nextNode.transform.position).magnitude;
            return distance < rechedNodeDistance;
        }
        else
            return false;
    }

    private void GoToNextNode()
    {
        Rigidbody frontCarRB = GetFrontCarRB();
        ChangeVelocityToAvoidCollision(frontCarRB);
        rb.velocity = newSpeed * transform.forward;
    }

    private void ChangeVelocityToAvoidCollision(Rigidbody frontCarRB)
    {
        if (frontCarRB != null)
        {
            float distance = (transform.position - frontCarRB.transform.position).magnitude - (carLength / 2);
            newSpeed = baseSpeed * ((distance - stoppingDistance) / detectionDistance);
        }
        else
        {
            newSpeed = baseSpeed;
        }
    }

    private Rigidbody GetFrontCarRB()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, detectionDistance, carLayerMask, QueryTriggerInteraction.Collide);
        if(hitInfo.collider != null)
        {
            return hitInfo.collider.GetComponent<Rigidbody>();
        }
        else
        {
            return null;
        }
    }

    private void ChangeLookingDirection(RoadNode target)
    {
        if (target != null)
        {
            transform.LookAt(target.transform.position, Vector3.up);
        }
    }

    private CarControllerData GetCarControllerData()
    {
        GameObject[] listOfDataObjects = GameObject.FindGameObjectsWithTag("carControllerData");  //Should only be 1, but just in case
        CarControllerData data = listOfDataObjects[0].GetComponent<CarControllerData>();
        return data;
        
    }

    private int GetTurnID()
    {
        return Random.Range(0, 2147483647);
    }

    private RoadNode GetNextNode(int currentID)
    {
        if (nextNode != null)
        {
            int choice = Mathf.Abs(currentID) % nextNode.nexts.Length;
            return nextNode.nexts[choice];
        }
        else
        {
            return null;
        }
    }
}
