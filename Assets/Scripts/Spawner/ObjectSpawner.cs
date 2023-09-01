using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    protected Vector2 screenBoundaries;
    public float spawnAreaSize = 4.5f;
    public Timer endTime;
    public int nbOfItems = 0;
    private void Start()
    {
        SpawnRandomObjects();
    }
    protected void GetScreenBoundaries()
    {
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }
    private void SpawnRandomObjects()
    {
        for (int i = 0; i < nbOfItems; i++)
        {
            GameObject randomObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

            Vector3 randomPosition = new Vector3(
                Random.Range(-screenBoundaries.x, screenBoundaries.x),
                Random.Range(-screenBoundaries.y, screenBoundaries.y),
                0f
            );

            // Spawn the object at the random position
            Instantiate(randomObject, randomPosition, Quaternion.identity);
        }
    }
}
