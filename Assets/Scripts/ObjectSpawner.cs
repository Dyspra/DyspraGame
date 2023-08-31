using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public float spawnAreaSize = 4.5f;
    public Timer endTime;

    private void Start()
    {
        SpawnRandomObjects();
    }

    private void SpawnRandomObjects()
    {
        //while (endTime.currentTime > 0f)
        for (int i = 0; i < 3; i++)
        {
            GameObject randomObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnAreaSize, spawnAreaSize),
                Random.Range(-spawnAreaSize, spawnAreaSize),
                0f
            );

            // Spawn the object at the random position
            Instantiate(randomObject, randomPosition, Quaternion.identity);
        }
    }
}
