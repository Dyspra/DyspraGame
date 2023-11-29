using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BurgerTemplate : MonoBehaviour
{
    public List<GameObject> objlist;
    public List<Ingredients.IngrediantType> ingredients;
    public Transform spawnpos;

    private void ObjectIntoList()
    {

       foreach (Ingredients.IngrediantType inburger in ingredients)
        {

            Instantiate(objlist[Convert.ToInt32(inburger)], spawnpos.position, Quaternion.identity);
        }
    }
}
