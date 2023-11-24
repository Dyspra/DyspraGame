using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientCheck : MonoBehaviour
{
    public List<Ingredients.IngrediantType> ingredients;

	private int i = 0;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Ingredients>().type == ingredients[i])
		{
			i++;
		} else
		{
			Destroy(other.gameObject);
		}
	}
}
