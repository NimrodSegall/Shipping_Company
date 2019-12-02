using UnityEngine;

public class CarBoundsDestroyer : MonoBehaviour
{
    private int carLayerMask = 8;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == carLayerMask)
        {
            Destroy(other.gameObject);
        }
    }
}
