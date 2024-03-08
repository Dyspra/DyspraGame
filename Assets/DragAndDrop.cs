using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    GameObject ing_in_hand;
    public GameObject hand_pos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Ingredients>(out Ingredients tocheck) == true)
        {
            ing_in_hand = other.gameObject;
            //PauseGravity();
        }
    }
    protected void PauseGravity()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ing_in_hand != null)
        {
            ing_in_hand.transform.position = hand_pos.transform.position;
        }
    }
}
