using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngredientCheck : MonoBehaviour
{
	public List<IngredientList> list;
    public List<Ingredients.IngrediantType> ingredients;
	public ScoreExercice4 score;
	public BurgerTemplate template;

	private int i = 0;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Ingredients>().type == ingredients[i])
		{
			i++;
			if (i == ingredients.Count)
			{
				score.IncreaseScore(i);
				i = 0;
				ingredients = list[Random.Range(0, list.Count)].Ingredients;
				template.ingredients = ingredients;
			}
		} else
		{
			Destroy(other.gameObject);
		}
	}
}
