using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: Right now there is a time delay before the spawn point search. This should be done
// right after all lanes get connected (to save time, and make sure that if there are many lanes this doesn't mess up)
public class CarSpawner : MonoBehaviour
{
    [SerializeField]
    private float minDelayBetweenSpawns = 0.5f;
    [SerializeField]
    private float maxDelayBetweenSpawns = 4f;
    [SerializeField]
    private GameObject[] cars = null;

    public Waypoint spawnNode = null;

    private float spawnDelay = 1;
    private float lastSpawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnNode != null)
        {
            SpawnCars();
        }
    }

    private void SpawnCars()
    {
        if(Time.time > lastSpawnTime + spawnDelay)
        {
            int carChoice = Random.Range(0, cars.Length);
            spawnDelay = NewSpawnDelay();
            lastSpawnTime = Time.time;
            GameObject newCar = Instantiate(cars[carChoice], transform.position, Quaternion.identity);
            newCar.GetComponent<WaypointNavigator>().SetInitialWp(spawnNode, transform.forward, true);
        }
        
        
    }

    private float NewSpawnDelay()
    {
        return Random.Range(minDelayBetweenSpawns, maxDelayBetweenSpawns);
    }

}
