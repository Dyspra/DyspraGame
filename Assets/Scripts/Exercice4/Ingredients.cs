using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredients : MonoBehaviour
{
	public enum IngrediantType
	{
		BottomBun,
		TopBun,
		Steak,
		Salad,
		Cheese,
		Tomato
	}
	public IngrediantType type;

	protected void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<>().isGrabbing)
		{
			FollowHand();
		}
	}
	protected void FollowHand()
	{
		this.transform.position = GameObject.Find("Hand").transform.position;
		PauseGravity();
	}
	protected void StopFollowHand()
	{
		UseGravity();
	}
	protected void PauseGravity()
	{
		this.GetComponent<Rigidbody>().useGravity = false;
	}
	protected void UseGravity()
	{
		this.GetComponent<Rigidbody>().useGravity = true;
	}
}
