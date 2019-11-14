using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    [SerializeField]
    private GameObject backObject = null;
    [SerializeField]
    private GameObject frontObject = null;

    [HideInInspector]
    public LaneConnection back = null;
    [HideInInspector]
    public LaneConnection front = null;

    private float rayDistance = 0;

    [HideInInspector]
    public Vector3 direction = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        back = backObject.GetComponent<LaneConnection>();
        front = frontObject.GetComponent<LaneConnection>();
        back.next = front;
        front.prev = back;

        direction = front.transform.position - back.transform.position;
        rayDistance = direction.magnitude / 1.8f;
        FindNextAndPrev();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FindNextAndPrev()
    {
        front.next = SearchConnectionInDirection(direction);
        back.prev = SearchConnectionInDirection(-direction);
    }

    private LaneConnection SearchConnectionInDirection(Vector3 direction)
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, direction, rayDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("laneConnection") && hit.collider.transform.parent != transform)
            {
                return hit.collider.transform.gameObject.GetComponent<LaneConnection>();
            }
        }
        return null;
    }
}
