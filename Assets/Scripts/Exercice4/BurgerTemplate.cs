using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BurgerTemplate : MonoBehaviour
{
    public List<GameObject> objlist;
    public List<Ingredients.IngrediantType> ingredients;
    public Transform spawnpos;
    public float spwtime;

    IEnumerator Orderingredients()
    {
        foreach (Ingredients.IngrediantType inburger in ingredients)
        {
            Instantiate(objlist[Convert.ToInt32(inburger)], spawnpos.position, Quaternion.identity);
            yield return new WaitForSeconds(spwtime);
        }
    }

    public void ObjectIntoList()
    {
        StartCoroutine(Orderingredients());
    }
}
