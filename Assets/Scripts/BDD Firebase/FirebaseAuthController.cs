using Firebase.Auth;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuthController : MonoBehaviour
{
    FirebaseAuth auth;
    KeyValuePair<string, string>? autologin = null;

    public bool isMailPending = false;

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

        });
    }

    
    public void SendConfirmationEmail()
    {
        isMailPending = true;
        auth.CurrentUser.SendEmailVerificationAsync().ContinueWith(verTask =>
        {
            if (verTask.IsCanceled)
            {
                PopUp.PrepareMessagePopUp("Envoi d'email de vérification annulé.");
                return;
            }

            if (verTask.IsFaulted)
            {
                PopUp.PrepareMessagePopUp(verTask.Exception.Flatten().InnerExceptions[0].ToString());
                return;
            }

            isMailPending = false;
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

            // Connexion r�ussie
            // FirebaseUser user = task.Result;
            // PopUp.PrepareMessagePopUp("Connect� � " + user.Email + " !");
        });
    }

    [Button("Log out")]
    public void LogOut()
    {
        auth.SignOut();
    }

    public string GetCurrentUserId()
    {
        return auth.CurrentUser != null ? auth.CurrentUser.UserId : "";
    }

    public string GetCurrentUserEmail()
    {
        return auth.CurrentUser.Email;
    }

    public bool GetIsConnected()
    {
        return auth.CurrentUser != null;
    }

    public bool GetUserVerified()
    {
        return auth.CurrentUser.IsEmailVerified;
    }
}
