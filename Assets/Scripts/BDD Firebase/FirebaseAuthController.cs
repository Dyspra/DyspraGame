using Firebase.Auth;
using NaughtyAttributes;
using UnityEditor.PackageManager;
using UnityEngine;

public class FirebaseAuthController : MonoBehaviour
{
    FirebaseAuth auth;
    public string email = "user@example.com";
    public string password = "password123";
    public bool isAutologin = false;

    [HideInInspector] public static string currentUserId;

    void Start()
    {
        // Initialise Firebase Auth
        auth = FirebaseAuth.DefaultInstance;
        if (isAutologin)
        {
            LogIn();
        }
    }

    [Button("Register")]
    public void Register()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Cr�ation de compte annul�e.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Erreur de cr�ation de compte : " + task.Exception.Flatten().InnerExceptions[0]);
                return;
            }

            // Cr�ation de compte r�ussie
            FirebaseUser user = task.Result;
            Debug.Log("Utilisateur cr�� avec succ�s : " + user.Email);
        });
    }

    [Button("Log in")]
    public void LogIn()
    {
        // Connectez-vous avec l'adresse e-mail et le mot de passe
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Connexion annul�e.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Erreur de connexion : " + task.Exception.Flatten().InnerExceptions[0]);
                return;
            }

            // Connexion r�ussie
            FirebaseUser user = task.Result;
            Debug.Log("Utilisateur connect� avec succ�s : " + user.Email);
        });
    }

    [Button("Log out")]
    public void Logout()
    {
        auth.SignOut();
        Debug.Log("Signed Out");
    }

    public string GetCurrentUserId()
    {
        return auth.CurrentUser.UserId;
    }
}
