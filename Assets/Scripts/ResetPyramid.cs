using UnityEngine;

public class ResetPyramid : MonoBehaviour
{
    private Vector3[] initialPositions;
    private Rigidbody[] rigidbodies;

    void Start()
    {
        initialPositions = new Vector3[transform.childCount];
        rigidbodies = new Rigidbody[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            initialPositions[i] = transform.GetChild(i).position;
            rigidbodies[i] = transform.GetChild(i).GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Invoke("ResetPyramidPosition", 2f);
        }
    }

    void ResetPyramidPosition()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            rigidbodies[i].velocity = Vector3.zero;
            rigidbodies[i].angularVelocity = Vector3.zero;
            transform.GetChild(i).position = initialPositions[i];
            transform.GetChild(i).rotation = Quaternion.identity;
        }
    }
}

