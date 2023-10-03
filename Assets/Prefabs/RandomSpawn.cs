using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Vector3 spawnArea;
    void Start()
    {
        RespawnPrefab();
    }

    public void RespawnPrefab()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f),
            0f,
            Random.Range(-spawnArea.z / 2f, spawnArea.z / 2f)
        );

        GameObject spawnedPrefab = Instantiate(prefabToSpawn, transform.position + spawnPosition, Quaternion.identity);
        spawnedPrefab.transform.parent = transform;
    }
}

