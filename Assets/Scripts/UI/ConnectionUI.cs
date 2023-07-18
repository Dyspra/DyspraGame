using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
{
    [SerializeField] InputField emailFieldRegister;
    [SerializeField] InputField passwordFieldRegister;
    [SerializeField] InputField passwordConfirmFieldRegister;
    [SerializeField] InputField firstNameFieldRegister;
    [SerializeField] InputField surnameFieldRegister;

    [SerializeField] InputField emailFieldLogIn;
    [SerializeField] InputField passwordFieldLogIn;
    
    [SerializeField] InputField emailFieldReset;

    [SerializeField] GameObject BaseMenu;
    [SerializeField] GameObject SignInMenu;

    [SerializeField] GameObject LoadingAnimate;

    void Update()
    {
        if (BDDInteractor.Instance.GetRegisteredComplete())
        {
            SignInMenu.SetActive(false);
            BaseMenu.SetActive(true);
        }
    }

    public void Register()
    {
        if (passwordFieldRegister.text != passwordConfirmFieldRegister.text)
        {
            PopUp.PrepareMessagePopUp("Les deux mots de passe doivent correspondre.");
            return;
        }
        BDDInteractor.Instance.Register(emailFieldRegister.text, passwordFieldRegister.text, firstNameFieldRegister.text, surnameFieldRegister.text);
        passwordFieldRegister.text = "";
        passwordConfirmFieldRegister.text = "";
    }

    public void LogIn()
    {
        BDDInteractor.Instance.LogIn(emailFieldLogIn.text, passwordFieldLogIn.text);
        passwordFieldLogIn.text = "";
    }

    public void LogOut()
    {
        BDDInteractor.Instance.LogOut();
    }

    public void ResetPassword()
    {
        BDDInteractor.Instance.SendPasswordResetEmail(emailFieldReset.text);
    }

    public void LaunchGame()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    #region OnAnimate

    public void DisableLoadingAnimate()
    {
        LoadingAnimate.SetActive(false);
    }

    public void EnableLoadingAnimate()
    {
        LoadingAnimate.SetActive(true);
    }

    public void EnableLoadingAnimateIfNoBaseMenu()
    {
        if (!GameObject.Find("Canvas").transform.Find("Menu").Find("BaseMenu").gameObject.activeSelf)
            EnableLoadingAnimate();
    }
    #endregion
}