using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
{
    [SerializeField] InputField emailFieldRegister;
    [SerializeField] InputField passwordFieldRegister;
    [SerializeField] InputField firstNameFieldRegister;
    [SerializeField] InputField surnameFieldRegister;

    [SerializeField] InputField emailFieldLogIn;
    [SerializeField] InputField passwordFieldLogIn;

    void Start()
    {
    }

    void Update()
    {
        if (BDDInteractor.Instance.getCachedProfile() != null)
        {
            Debug.Log("username : " + BDDInteractor.Instance.getCachedProfile().Username);
            BDDInteractor.Instance.RemoveProfile();
        }
    }

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
}