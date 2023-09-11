using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
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

    bool sentMail = false;
    void Start()
    {
        ProfileMenu = transform.Find("ProfileMenu").gameObject;
        AvatarMenu = transform.Find("AvatarMenu").gameObject;
        GameMenu = transform.Find("GameMenu").gameObject;
        SignInMenu = transform.Find("SignInMenu").gameObject;
        LogInMenu = transform.Find("LogInMenu").gameObject;
        BaseMenu = transform.Find("BaseMenu").gameObject;
        ExercicesMenu = transform.Find("ExercicesMenu").gameObject;
        MenuAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (MenuAnimator.IsInTransition(0)) return;
        if (BDDInteractor.Instance.isUserAuthentified() && !GameMenu.activeSelf && !ProfileMenu.activeSelf && !AvatarMenu.activeSelf && !ExercicesMenu.activeSelf)
        {
            if (!BDDInteractor.Instance.GetUserVerified()) //check that user has successfully verified their email adress
            {
                if (!sentMail)
                {
                    PopUp.PrepareMessagePopUp("Veuillez v�rifier votre compte � travers le lien envoy� par email et connectez-vous.");
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
                MenuAnimator.SetTrigger("Game");
                MenuAnimator.ResetTrigger("Disconnect");
                MenuAnimator.ResetTrigger("Exercices");
                BaseMenu.SetActive(false);
                GameMenu.SetActive(true);
                AnalyticsManager.Instance.SetUserId(BDDInteractor.Instance.GetCurrentUserId());
            }
        }
        else if (!BDDInteractor.Instance.isUserAuthentified() && GameMenu.activeSelf)
        {
            MenuAnimator.SetTrigger("Disconnect");
            MenuAnimator.ResetTrigger("Game");
            MenuAnimator.ResetTrigger("Exercices");
            SignInMenu.SetActive(false);
            LogInMenu.SetActive(false);
            BaseMenu.SetActive(true);
        }
    }

    public void TransitionExercicesMenu()
    {
        MenuAnimator.SetTrigger("Exercices");
        MenuAnimator.ResetTrigger("Game");
        MenuAnimator.ResetTrigger("Disconnect");
        MenuAnimator.ResetTrigger("LeaveExercices");
    }

    public void LeaveExercicesMenu()
    {
        MenuAnimator.SetTrigger("LeaveExercices");
        MenuAnimator.ResetTrigger("Game");
        MenuAnimator.ResetTrigger("Disconnect");
        MenuAnimator.ResetTrigger("Exercices");
        MenuAnimator.ResetTrigger("History");
        MenuAnimator.ResetTrigger("LeaveHistory");
    }

    public void TransitionHistoryMenu()
    {
        MenuAnimator.SetTrigger("History");
        MenuAnimator.ResetTrigger("Game");
        MenuAnimator.ResetTrigger("Disconnect");
        MenuAnimator.ResetTrigger("LeaveHistory");
        MenuAnimator.ResetTrigger("Exercices");
        MenuAnimator.ResetTrigger("LeaveExercices");
    }

    public void LeaveHistoryMenu()
    {
        MenuAnimator.SetTrigger("LeaveHistory");
        MenuAnimator.ResetTrigger("Game");
        MenuAnimator.ResetTrigger("Disconnect");
        MenuAnimator.ResetTrigger("History");
        MenuAnimator.ResetTrigger("Exercices");
        MenuAnimator.ResetTrigger("LeaveExercices");
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

    public void DisableGameMenu()
    {
        GameMenu.SetActive(false);
    }

    public void DisableExercicesMenu()
    {
        ExercicesMenu.SetActive(false);
    }

    public void DisableLogInMenu()
    {
        LogInMenu.SetActive(false);
    }

    #endregion
}
