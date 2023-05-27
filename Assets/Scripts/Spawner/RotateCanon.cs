using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCanon : MonoBehaviour
{
    public Vector3 minRotation;
    public Vector3 maxRotation;
    public float speed = 2f;

    bool isRotatingToMin = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0;
        float y = 0;
        float z = 0;

        if (maxRotation.x - minRotation.x > 0)
        {
            x = Mathf.PingPong(Time.time * speed, maxRotation.x - minRotation.x);
        }
        if (maxRotation.y - minRotation.y > 0)
        {
            y = Mathf.PingPong(Time.time * speed, maxRotation.y - minRotation.y);
        }
        if (maxRotation.z - minRotation.z > 0)
        {
            z = Mathf.PingPong(Time.time * speed, maxRotation.z - minRotation.z);
        }

        this.transform.localEulerAngles = new Vector3(x, y, z);
    }
}
