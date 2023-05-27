using Firebase.Auth;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuthController : MonoBehaviour
{
    FirebaseAuth auth;
    KeyValuePair<string, string>? autologin = null;

    [HideInInspector] public static string currentUserId;

    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    void Update()
    {
        if (autologin != null)
        {
            LogIn(autologin.Value.Key, autologin.Value.Value);
            autologin = null;
        }
    }

    public void Register(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                PopUp.PrepareMessagePopUp("Création de compte annulée.");
                return;
            }
            if (task.IsFaulted)
            {
                PopUp.PrepareMessagePopUp(task.Exception.Flatten().InnerExceptions[0].ToString());
                return;
            }

            // Création de compte réussie
            FirebaseUser user = task.Result;
            autologin = new KeyValuePair<string, string>(email, password);
        });
    }

    public void LogIn(string email, string password)
    {
        // Connectez-vous avec l'adresse e-mail et le mot de passe
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                PopUp.PrepareMessagePopUp("Connexion annulée.");
                return;
            }
            if (task.IsFaulted)
            {
                PopUp.PrepareMessagePopUp(task.Exception.Flatten().InnerExceptions[0].ToString());
                return;
            }

            // Connexion réussie
            FirebaseUser user = task.Result;
            PopUp.PrepareMessagePopUp("Connecté à " + user.Email + " !");
        });
    }

    [Button("Log out")]
    public void LogOut()
    {
        auth.SignOut();
        PopUp.PrepareMessagePopUp("Déconnexion complétée.");
    }

    public string GetCurrentUserId()
    {
        return auth.CurrentUser != null ? auth.CurrentUser.UserId : "";
    }

    public bool GetIsConnected()
    {
        return auth.CurrentUser != null;
    }
}
