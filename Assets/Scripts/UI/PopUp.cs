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
        popUpMessage = TranslatePopUp(popUpMessage.Replace("Firebase.FirebaseException: ", ""));
        Debug.Log("Sending popup : " + popUpMessage);
        popUpText.text = popUpMessage;
        animator.SetTrigger("display");
    }

    public static void PrepareMessagePopUp(string preparedPopUpMessage)
    {
        popUpMessage = preparedPopUpMessage;
        displayPopUpTrigger = true;
        disableLoadTrigger = true;
    }

    static string TranslatePopUp(string popUpMessage)
    {
        switch (popUpMessage)
        {
            case "The password is invalid or the user does not have a password.":
                return "Le mot de passe est incorrect, veuillez réessayer.";
            case "An email address must be provided.":
                return "Une adresse email est nécessaire.";
            case "A password must be provided.":
                return "Un mot de passe est nécessaire.";
            case "The email address is badly formatted.":
                return "L'adresse email entrée n'est pas correctement formatée, veuillez réessayer.";
            case "The given password is invalid.":
                return "Le mot de passe entré est invalide, veuillez réessayer.";
            case "There is no user record corresponding to this identifier. The user may have been deleted.":
                return "Nous ne trouvons aucun utilisateur correspondant à cette adresse email, veuillez réessayer.";
            case "The email address is already in use by another account.":
                return "Cet email est déjà lié à un autre compte Dyspra.";
            default:
                return popUpMessage;
        }
    }
}