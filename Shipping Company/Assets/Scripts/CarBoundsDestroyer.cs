using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBoundsDestroyer : MonoBehaviour
{
    private int carLayerMask = 8;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == carLayerMask)
        {
            Destroy(other.gameObject);
        }
    }
}
