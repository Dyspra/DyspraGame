using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDirection : MonoBehaviour
{
    public GameObject origin;
    public GameObject laserDisplay;
    public LayerMask collisionLayer;
    LineRenderer laser;

    public float[] offset;

    private void Update()
    {
        /*Vector3 newDisplay = new Vector3(30.31f * (origin.transform.position.x) + offset[0], 30.31f * (origin.transform.position.y) + offset[1], laserDirection.transform.position.z);
        Vector3 newPos = new Vector3(origin.transform.position.x, origin.transform.position.y, laserDirection.transform.position.z);
        laserDisplay.transform.position = newDisplay;
        laserDirection.transform.position = newPos;
        transform.position = newDisplay;*/
        RaycastHit hit;
        if (Physics.Raycast(origin.transform.position, Vector3.forward, out hit, Mathf.Infinity, collisionLayer))
        {
            float dist = Vector3.Distance(origin.transform.position, hit.point);
            laserDisplay.transform.position = new Vector3(hit.point.x * dist, hit.point.y * dist, hit.point.z);
            Vector3 hitPoint = hit.point;
            transform.position = laserDisplay.transform.position;
		}
    }
}
