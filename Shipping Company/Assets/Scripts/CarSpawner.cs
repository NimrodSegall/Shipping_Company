using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: Right now there is a time delay before the spawn point search. This should be done
// right after all lanes get connected (to save time, and make sure that if there are many lanes this doesn't mess up)
public class CarSpawner : MonoBehaviour
{
    [SerializeField]
    private float searchRadius = 5f;
    [SerializeField]
    private float minDelayBetweenSpawns = 0.5f;
    [SerializeField]
    private float maxDelayBetweenSpawns = 4f;
    [SerializeField]
    private GameObject[] cars = null;

    public GameObject spawnPointObject = null;

    private IEnumerator delayCoroutine;
    private float delay = 0.01f;

    private float spawnDelay = 1;
    private float lastSpawnTime = 0;

    private bool allowUpdate = false;

    // Start is called before the first frame update
    void Start()
    {
        delayCoroutine = DelayBeforeStart(delay);
        StartCoroutine(delayCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(allowUpdate)
        {
            StopCoroutine(delayCoroutine);
            SpawnCars();
        }
            
    }

    private IEnumerator DelayBeforeStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        spawnPointObject = FindEmptyConnection();
        allowUpdate = true;
    }

    private GameObject FindEmptyConnection()
    {
        Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider col in collidersInRadius)
        {
            if (col.CompareTag("laneConnection"))
            {
                if (col.GetComponentInParent<Lane>().back.prev == null)
                {
                    return col.gameObject;
                }
            }
        }
        return null;
    }

    private void SpawnCars()
    {
        if(Time.time > lastSpawnTime + spawnDelay)
        {
            int carChoice = Random.Range(0, cars.Length);
            spawnDelay = NewSpawnDelay();
            lastSpawnTime = Time.time;
            GameObject newCar = Instantiate(cars[carChoice], spawnPointObject.transform.position, Quaternion.identity);
            newCar.GetComponent<CarController>().SetStartingNode(spawnPointObject.GetComponent<LaneConnection>());
        }
        
        
    }

    private float NewSpawnDelay()
    {
        return Random.Range(minDelayBetweenSpawns, maxDelayBetweenSpawns);
    }
}
