using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collide");
        Ingredients ingredients;
        if (other.gameObject.TryGetComponent<Ingredients>(out ingredients) == true)
        {
            Destroy(other.gameObject);
        }
    }
}
