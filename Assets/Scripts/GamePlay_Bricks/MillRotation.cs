using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillRotation : MonoBehaviour
{
    public float rotationsPerMinute = 3f;
    public bool clockWise = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (clockWise)
        {
            transform.Rotate(0, -6.0f*rotationsPerMinute*Time.deltaTime, 0);
        }
        else
        {
            transform.Rotate(0, 6.0f*rotationsPerMinute*Time.deltaTime, 0);
        }
    }
}
