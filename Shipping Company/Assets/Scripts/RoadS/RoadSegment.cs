using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    [HideInInspector]
    public Lane[] lanes = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int NumberOfLanes()
    {
        return lanes.Length;
    }
}
