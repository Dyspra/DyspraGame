using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTuto : MonoBehaviour
{
    Vector3 initialPosition;

    void Start()
    {
        initialPosition = this.transform.position;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<DeathZone>() != null)
        {
            this.transform.position = initialPosition;
        }
    }
}
