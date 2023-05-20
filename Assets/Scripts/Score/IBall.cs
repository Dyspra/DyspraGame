using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBall : MonoBehaviour
{
    public GameObject canonReference;
    public float spawnProbability;

    public abstract void ApplyEffect();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("KEK");
            ApplyEffect();
        }
    }
}
