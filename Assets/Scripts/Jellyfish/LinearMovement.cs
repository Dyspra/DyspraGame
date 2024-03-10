using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : AJellyfishBehaviour
{
	public Vector3 direction;

	protected void Update()
	{
		Move(direction);
	}

	protected void LateUpdate()
	{
		
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
