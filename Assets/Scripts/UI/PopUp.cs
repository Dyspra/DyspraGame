using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    static Animator animator;
    static Text popUpText;

    static bool disableLoadTrigger = false;
    static bool displayPopUpTrigger = false;
    static string popUpMessage = "";

    [SerializeField] GameObject LoadingAnimate;

    void Start()
    {
        animator = GetComponent<Animator>();
        popUpText = transform.Find("Text").gameObject.GetComponent<Text>();
    }

    void Update()
    {
        if (displayPopUpTrigger)
        {
            DisplayPopUp(popUpMessage);
            displayPopUpTrigger = false;
        }
        if (disableLoadTrigger)
        {
            LoadingAnimate.SetActive(false);
            disableLoadTrigger = false;
        }
    }

    public static void DisplayPopUp(string popUpMessage)
    {
        Debug.Log("Sending popup : " + popUpMessage);
        popUpMessage = popUpMessage.Replace("Firebase.FirebaseException: ", "");
        popUpText.text = popUpMessage;
        animator.SetTrigger("display");
    }

    public static void PrepareMessagePopUp(string preparedPopUpMessage)
    {
        popUpMessage = preparedPopUpMessage;
        displayPopUpTrigger = true;
        disableLoadTrigger = true;
    }
}
