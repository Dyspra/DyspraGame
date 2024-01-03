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
        float x, y;
        Vector2 screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        if (Physics.Raycast(origin.transform.position, Vector3.forward, out hit, Mathf.Infinity, collisionLayer))
        {
            float dist = Vector3.Distance(origin.transform.position, hit.point);
            //x =  (hit.point.x + screenBoundaries.x /  (screenBoundaries.x *2)) * (screenBoundaries.x *2) - screenBoundaries.x;
            //y =  (hit.point.y + screenBoundaries.y /  (screenBoundaries.y *2)) * (screenBoundaries.y *2) - screenBoundaries.y;
            //Debug.Log("Fingerx = " + origin.transform.position.x + " Fingery = " + origin.transform.position.y);
            //Debug.Log("x = " + hit.point.x + " y = " + hit.point.y);
            //Vector3 point = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(new Vector3(hit.point.x, hit.point.y, 0)));
            Vector3 planeNormal = Vector3.forward;
            Vector3 point = Vector3.ProjectOnPlane(origin.transform.position, planeNormal);
            point.z = 0;
            /*Matrix4x4 translationMatrix = Matrix4x4.Translate(new Vector3(0.0f, 0.0f, hit.point.z - origin.transform.position.z));
            translationMatrix.MultiplyPoint3x4(origin.transform.position);*/
            laserDisplay.transform.position = /*point;*/new Vector3(point.x * dist /*+ offset[0]*/, point.y * dist /*+ offset[1]*/, 0);
            Vector3 hitPoint = point;
            transform.position = laserDisplay.transform.position;
		}
    }
}
