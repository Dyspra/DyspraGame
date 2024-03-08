using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class IngredientCheck : MonoBehaviour
{
	public List<IngredientList> list;
    public List<Ingredients.IngrediantType> ingredients;
	public ScoreExercice4 score;
	public BurgerTemplate template;
	public RemoveBurger mainplate, templateplate;

	private int i = 0;

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("COLLIDE WITH : " + other.gameObject.name);
		if (other.gameObject.GetComponent<Ingredients>().type == ingredients[i])
		{
			i++;
			mainplate.AddIngredientToList(other.gameObject);
			Debug.Log("i = " + i + " && ingredients.Count = " + ingredients.Count);
			if (i == ingredients.Count)
			{
				score.IncreaseScore(i);
				i = 0;
				ingredients = list[Random.Range(0, list.Count)].Ingredients;
				template.ingredients = ingredients;
				templateplate.RemoveIngredients();
				mainplate.RemoveIngredients();
				template.ObjectIntoList();
			}
		} else
		{
			Debug.Log("type = " + other.gameObject.GetComponent<Ingredients>().type + " && required = " + ingredients[i]);
			Destroy(other.gameObject);
		}
	}
}
