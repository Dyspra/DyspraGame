using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDirection : MonoBehaviour
{
    public GameObject origin;
    public GameObject laserDirection;
    public LayerMask collisionLayer;
    LineRenderer laser;

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(origin.transform.position, origin.transform.right, out hit, Mathf.Infinity, collisionLayer))
        {
            laserDirection.transform.position = hit.point;
            Vector3 hitPoint = hit.point;
            transform.position = hitPoint;
        }
    }
}
