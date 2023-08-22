using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDirection : MonoBehaviour
{
    public GameObject laserObject;
    public LayerMask collisionLayer;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(laserObject.transform.position, Vector2.up, Mathf.Infinity, collisionLayer);
        if (hit.collider != null)
        {
            Vector3 hitPoint = hit.point;
            transform.position = hitPoint;
            Debug.DrawLine(laserObject.transform.position, transform.position, Color.red);
        }
    }
}
