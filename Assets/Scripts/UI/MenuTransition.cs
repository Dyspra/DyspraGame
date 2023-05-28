using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTransition : MonoBehaviour
{
    Animator MenuAnimator;

    GameObject ProfileMenu;
    GameObject GameMenu;
    GameObject SignInMenu;
    GameObject LogInMenu;
    GameObject BaseMenu;
    void Start()
    {
        ProfileMenu = transform.Find("ProfileMenu").gameObject;
        GameMenu = transform.Find("GameMenu").gameObject;
        SignInMenu = transform.Find("SignInMenu").gameObject;
        LogInMenu = transform.Find("LogInMenu").gameObject;
        BaseMenu = transform.Find("BaseMenu").gameObject;
        MenuAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (MenuAnimator.IsInTransition(0)) return;
        if (BDDInteractor.Instance.isUserAuthentified() && !GameMenu.activeSelf && !ProfileMenu.activeSelf)
        {
            MenuAnimator.SetTrigger("Game");
            MenuAnimator.ResetTrigger("Disconnect");
            BaseMenu.SetActive(false);
            GameMenu.SetActive(true);
        }
        else if (!BDDInteractor.Instance.isUserAuthentified() && GameMenu.activeSelf)
        {
            MenuAnimator.SetTrigger("Disconnect");
            MenuAnimator.ResetTrigger("Game");
            SignInMenu.SetActive(false);
            LogInMenu.SetActive(false);
            BaseMenu.SetActive(true);
        }
    }

    public void EnableGameMenu()
    {
        GameMenu.SetActive(true);
    }

    public void DisableGameMenu()
    {
        GameMenu.SetActive(false);
    }
}