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
    public bool registeredComplete = false;

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
        registeredComplete = false;
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

            // FirebaseUser user = task.Result.getUser();
            UnityEngine.Debug.Log(task.Result);
            // AnalyticsManager.Instance.SetUserId(user.UserId);
            registeredComplete = true;
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


    public void SendPasswordResetEmail(string email)
    {
        isMailPending = true;
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                PopUp.PrepareMessagePopUp("L'email n'a pas pu être envoyé.");
                return;
            }
            if (task.IsFaulted)
            {
                PopUp.PrepareMessagePopUp(task.Exception.Flatten().InnerExceptions[0].ToString());
                return;
            }

            PopUp.PrepareMessagePopUp("Email de réinitialisation de mot de passe a bien été envoyé à " + email);
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
            // PopUp.PrepareMessagePopUp("Connecté à " + user.Email + " !");
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
