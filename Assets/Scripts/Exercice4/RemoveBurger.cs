using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBurger : MonoBehaviour
{
    public List<GameObject> toremove;

/*    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.TryGetComponent<Ingredients>(out Ingredients tocheck) == true)
        {
            toremove.Add(other.gameObject);
        }
    }
*/
    public void RemoveIngredients()
    {
        GameObject todelete;
        foreach (GameObject suppress in toremove)
        {
            todelete = suppress;
            //toremove.Remove(suppress);
            Destroy(todelete);
        }
    }

    public void AddIngredientToList(GameObject toadd)
    {
        toremove.Add(toadd);
    }
    void Start()
    {
        toremove = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
