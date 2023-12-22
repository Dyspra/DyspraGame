using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDirection : MonoBehaviour
{
    public GameObject origin;
    public GameObject laserDisplay;
    public GameObject laserDirection;
    public LayerMask collisionLayer;
    LineRenderer laser;

    public float[] offset;

    private void Update()
    {
        Vector3 newDisplay = new Vector3(30.31f * (origin.transform.position.x) + offset[0], 30.31f * (origin.transform.position.y) + offset[1], laserDirection.transform.position.z);
        Vector3 newPos = new Vector3(origin.transform.position.x, origin.transform.position.y, laserDirection.transform.position.z);
        laserDisplay.transform.position = newDisplay;
        laserDirection.transform.position = newPos;
        transform.position = newDisplay;
    }
}
        /*RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, origin.transform.right, out hit, Mathf.Infinity, collisionLayer))
        {
            laserDirection.transform.position = hit.point;
            Vector3 hitPoint = hit.point;
            transform.position = hitPoint;
        }*/
