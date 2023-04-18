using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    public float rotationSpeed = 1;
    [Range(1, 100)] public int blastPower = 5;

    public List<GameObject> objectToShoot;
    public Transform shotPoint;
    public bool displayLine = true;

    LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blastPower < 0)
            blastPower = 1;
        if (displayLine && !line.enabled)
            line.enabled = true;
        else if (!displayLine && line.enabled)
            line.enabled = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int randomItem = Random.Range(0, objectToShoot.Count);
            GameObject createdObject = Instantiate(objectToShoot[randomItem], shotPoint.position, shotPoint.rotation);
            createdObject.GetComponent<Rigidbody>().velocity = shotPoint.transform.up * blastPower;
        }
    }
}
