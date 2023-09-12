using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Camera cam;
    public GameObject projectile;
    public Transform LfirePoint, RfirePoint;
    public float projectileSpeed = 10;

    private Vector3 destination;
    private bool lefthand;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            ShootProjectile();
    }

    void ShootProjectile()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            destination = hit.point;
        else
            destination = ray.GetPoint(1000);

        if (lefthand) {
            lefthand = false;
            InstantiateProjectile(LfirePoint);
        } else {
            lefthand = true;
            InstantiateProjectile(RfirePoint);
        }
        
    }

    void InstantiateProjectile(Transform firePoint)
    {
        var projectileObj = Instantiate (projectile, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (destination- firePoint.position).normalized * projectileSpeed;
    }
}
