using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    private Vector2 screenBoundaries;
    private float[] jellyfish_select = new float[3];
    public int nbOfItems = 0;
    public Timer timer;
    [SerializeField] private int initialItemSpawn = 0;
    private float select;
    private float delta_time = 0;
    private void Start()
    {
        ChangeProbabilities(0);
    }
    private void OnEnable()
    {
        GetScreenBoundaries();
        delta_time = timer.maxTime;
        for(int i = 0; i < nbOfItems; i++) {
            SpawnObject(objectsToSpawn[initialItemSpawn]);            
        }
    }
    private void GetScreenBoundaries()
    {
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }
    private void ChangeProbabilities(int Round)
    {
        if (Round == 0) {
            jellyfish_select[0] = 1;
            jellyfish_select[1] = 0;
            jellyfish_select[2] = 0;

        } else if (Round == 1) {
            jellyfish_select[0] = 0.7f;
            jellyfish_select[1] = 0.3f;
            jellyfish_select[2] = 0;
        } else {
            jellyfish_select[0] = 0.6f;
            jellyfish_select[1] = 0.3f;
            jellyfish_select[2] = 0.1f;
        }
    }
    private void SpawnObject(GameObject ToSpawn)
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(screenBoundaries.x * -1, screenBoundaries.x),
            Random.Range(screenBoundaries.y * -1, screenBoundaries.y),
            0f
        );
        Instantiate(ToSpawn, randomPosition, Quaternion.identity);
    }
    private void SpawnRandomObject()
    {
        select = Random.Range(0f, 1f);
    
        if (select <= jellyfish_select[2]) {
            SpawnObject(objectsToSpawn[2]);
        } else if (select <= jellyfish_select[1]) {
			SpawnObject(objectsToSpawn[1]);
        } else {
			SpawnObject(objectsToSpawn[0]);
        }
    }
    private void Update()
    {
        if (timer.maxTime - timer.currentTime >= 60f)
            ChangeProbabilities(1);
        if (timer.maxTime - timer.currentTime >= 120f)
            ChangeProbabilities(2);
        if (delta_time - timer.currentTime >= 6f) {
            SpawnRandomObject();
            delta_time = timer.currentTime;
        }
    }
}
