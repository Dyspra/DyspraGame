using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFromTemplate : MonoBehaviour
{
    public RemoveBurger refromtemplate;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Ingredients>(out Ingredients tocheck) == true)
        {
            refromtemplate.AddIngredientToList(other.gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
