using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 1;
    public float pushForce = 10.0f;

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pushable"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 pushDirection = collision.contacts[0].normal;
                rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }
}
