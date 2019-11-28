using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: Right now there is a time delay before the spawn point search. This should be done
// right after all lanes get connected (to save time, and make sure that if there are many lanes this doesn't mess up)
public class CarSpawner : MonoBehaviour
{
    /*
    [SerializeField]
    private float searchRadius = 5f;
    [SerializeField]
    private float minDelayBetweenSpawns = 0.5f;
    [SerializeField]
    private float maxDelayBetweenSpawns = 4f;
    [SerializeField]
    private GameObject[] cars = null;

    public RoadNode spawnNode = null;

    private int nodeLayerMask = 1 << 9;

    private float spawnDelay = 1;
    private float lastSpawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(spawnNode == null)
        {
            spawnNode = FindSpawnNode();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnCars();
    }

    private RoadNode FindSpawnNode()
    {
        Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, searchRadius, nodeLayerMask);
        foreach (Collider col in collidersInRadius)
        {
            RoadNode node = col.GetComponentInParent<RoadNode>();
            if (node.prevs[0] == null)
            {
                return node;
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
            GameObject newCar = Instantiate(cars[carChoice], spawnNode.transform.position, Quaternion.identity);
            newCar.GetComponent<CarController>().SetStartingNode(spawnNode);
        }
        
        
    }

    private float NewSpawnDelay()
    {
        return Random.Range(minDelayBetweenSpawns, maxDelayBetweenSpawns);
    }
    */
}
