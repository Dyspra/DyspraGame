using Mediapipe.Unity;
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
    [SerializeField] InputField firstNameFieldRegister;
    [SerializeField] InputField surnameFieldRegister;

    [SerializeField] InputField emailFieldLogIn;
    [SerializeField] InputField passwordFieldLogIn;

    [SerializeField] GameObject LoadingAnimate;

    public void Register()
    {
        BDDInteractor.Instance.Register(emailFieldRegister.text, passwordFieldRegister.text, firstNameFieldRegister.text, surnameFieldRegister.text);
        passwordFieldRegister.text = "";
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