using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentController : MonoBehaviour
{
    public float forceMagnitude = 10.0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ApplyWindForce();
        }
    }

    void ApplyWindForce()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 forceDirection = (hit.point - transform.position).normalized;
                rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
