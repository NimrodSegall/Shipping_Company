using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public List<Shipment> inGoing = new List<Shipment>();
    public List<Shipment> outGoing = new List<Shipment>();

    public Waypoint loadingPoint = null;
    // Start is called before the first frame update
    void Start()
    {
        loadingPoint = GetComponentInChildren<Waypoint>();
        loadingPoint.isLoadingSpot = true;
    }
}
