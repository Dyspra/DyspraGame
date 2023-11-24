using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed;
    public Vector3 direction;

    private List<GameObject> list;

	private void Start()
	{
		list = new List<GameObject>();
	}

	// Update is called once per frame
	void Update()
    {
        foreach (GameObject ingredient in list) 
        {
            ingredient.GetComponent<Rigidbody>().velocity = speed * direction * Time.deltaTime;
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
        list.Add(collision.gameObject);
	}

	private void OnCollisionExit(Collision collision)
	{
		list.Remove(collision.gameObject);
	}
}
