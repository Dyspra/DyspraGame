using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float duration;
    public Transform spawnpos;
    public List<GameObject> ingredients;
    int randomingredient;

    void Start()
    {
        StartCoroutine(spawningredients());
    }

    void Update()
    {
        
    }

    IEnumerator spawningredients()
    {
        while(true)
        {
            yield return new WaitForSeconds(duration);
            randomingredient = Random.Range(0, (ingredients.Count -1));
            Instantiate(ingredients[randomingredient], spawnpos.position, Quaternion.identity);
        }
    }
}
