using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MenuTransition : MonoBehaviour
{
    Animator MenuAnimator;

    GameObject ProfileMenu;
    GameObject AvatarMenu;
    GameObject GameMenu;
    GameObject SignInMenu;
    GameObject LogInMenu;
    GameObject BaseMenu;
    GameObject ExercicesMenu;
    GameObject ProgressionMenu;
    GameObject FormsMenu;

    bool sentMail = false;
    public static bool startExercises = false;

    void Start()
    {
        ProfileMenu = transform.Find("ProfileMenu").gameObject;
        AvatarMenu = transform.Find("AvatarMenu").gameObject;
        GameMenu = transform.Find("GameMenu").gameObject;
        SignInMenu = transform.Find("SignInMenu").gameObject;
        LogInMenu = transform.Find("LogInMenu").gameObject;
        BaseMenu = transform.Find("BaseMenu").gameObject;
        ExercicesMenu = transform.Find("ExercicesMenu").gameObject;
        ProgressionMenu = transform.Find("ProgressionMenu").gameObject;
        FormsMenu = transform.Find("FormsMenu").gameObject;
        MenuAnimator = GetComponent<Animator>();
        if (startExercises == true)
        {
            startExercises = false;
            ResetAllTriggers();
            MenuAnimator.SetTrigger("StartExercice");
            BaseMenu.SetActive(false);
            ExercicesMenu.SetActive(true);
        }
    }

    void Update()
    {
        if (MenuAnimator.IsInTransition(0)) return;
        if (BDDInteractor.Instance.isUserAuthentified() && !GameMenu.activeSelf && !ProfileMenu.activeSelf && !AvatarMenu.activeSelf && !ExercicesMenu.activeSelf && !FormsMenu.activeSelf && !ProgressionMenu.activeSelf)
        {
            if (!BDDInteractor.Instance.GetUserVerified()) //check that user has successfully verified their email adress
            {
                if (!sentMail)
                {
                    PopUp.PrepareMessagePopUp("Veuillez vérifier votre compte à travers le lien envoyé par email et connectez-vous.");
                    BaseMenu.transform.Find("Button - Redirection").gameObject.SetActive(true);
                    BDDInteractor.Instance.SendConfirmationEmail();
                    sentMail = true;
                }
                else if (!BDDInteractor.Instance.GetIsMailPending())
                {
                    sentMail = false;
                    BDDInteractor.Instance.LogOut();
                }
            }
            else
            {
                ResetAllTriggers();
                MenuAnimator.SetTrigger("Game");
                BaseMenu.SetActive(false);
                GameMenu.SetActive(true);
                AnalyticsManager.Instance.SetUserId(BDDInteractor.Instance.GetCurrentUserId());
            }
        }
        else if (!BDDInteractor.Instance.isUserAuthentified() && GameMenu.activeSelf)
        {
            ResetAllTriggers();
            MenuAnimator.SetTrigger("Disconnect");
            SignInMenu.SetActive(false);
            LogInMenu.SetActive(false);
            BaseMenu.SetActive(true);
        }
    }

    void ResetAllTriggers()
    {
        MenuAnimator.ResetTrigger("Game");
        MenuAnimator.ResetTrigger("Disconnect");
        MenuAnimator.ResetTrigger("Exercices");
        MenuAnimator.ResetTrigger("History");
        MenuAnimator.ResetTrigger("Progression");
        MenuAnimator.ResetTrigger("LeaveExercices");
        MenuAnimator.ResetTrigger("LeaveHistory");
        MenuAnimator.ResetTrigger("LeaveProgression");
    }

    public void TransitionExercicesMenu()
    {
        ResetAllTriggers();
        MenuAnimator.SetTrigger("Exercices");
    }

    public void LeaveExercicesMenu()
    {
        ResetAllTriggers();
        MenuAnimator.SetTrigger("LeaveExercices");
    }

    public void TransitionProgressionMenu()
    {
        ResetAllTriggers();
        MenuAnimator.SetTrigger("Progression");
    }

    public void LeaveProgressionMenu()
    {
        ResetAllTriggers();
        MenuAnimator.SetTrigger("LeaveProgression");
    }

    public void TransitionHistoryMenu()
    {
        ResetAllTriggers();
        MenuAnimator.SetTrigger("History");
    }

    public void LeaveHistoryMenu()
    {
        ResetAllTriggers();
        MenuAnimator.SetTrigger("LeaveHistory");
    }

    #region OnAnimate

    public void EnableGameMenu()
    {
        GameMenu.SetActive(true);
    }

    public void EnableExercicesMenu()
    {
        ExercicesMenu.SetActive(true);
    }

    public void EnableProgressionMenu()
    {
        ProgressionMenu.SetActive(true);
    }

    public void DisableGameMenu()
    {
        GameMenu.SetActive(false);
    }

    public void DisableExercicesMenu()
    {
        ExercicesMenu.SetActive(false);
    }

    public void DisableProgressionMenu()
    {
        ProgressionMenu.SetActive(false);
    }

    public void DisableLogInMenu()
    {
        LogInMenu.SetActive(false);
    }

    #endregion
}
