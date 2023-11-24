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
}
