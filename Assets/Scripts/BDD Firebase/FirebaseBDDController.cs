using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseBDDController : MonoBehaviour
{
    DatabaseReference dbReference;
    void Awake()
    {
        // Initialise la base de donn�es Firebase
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public IEnumerator DatabaseSetProfile(Profile profile, Action<bool> onComplete)
    {
        string json = JsonUtility.ToJson(profile);
        bool isDone = false;

        var NewProfile = dbReference
                        .Child("Profile")
                        .Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId)
                        .SetRawJsonValueAsync(json).ContinueWith(task =>
                        {
                            if (task.IsCanceled)
                            {
                                Debug.LogError("Cr�ation de profil annul�e.");
                                PopUp.PrepareMessagePopUp("Cr�ation de profil annul�e.");
                                onComplete?.Invoke(false);
                                return;
                            }
                            else if (task.IsFaulted)
                            {
                                Debug.LogError("Erreur de cr�ation de profil : " + task.Exception.Flatten().InnerExceptions[0]);
                                PopUp.PrepareMessagePopUp(task.Exception.Flatten().InnerExceptions[0].ToString());
                                onComplete?.Invoke(false);
                                return;
                            }
                            Debug.Log("Cr�ation de profil r�ussie");
                            isDone = true;
                            onComplete?.Invoke(true);
                        });

        yield return new WaitUntil(predicate: () => isDone == true);
    }

    public IEnumerator DatabaseGetProfile(Action<Profile> onComplete)
    {
        Profile profile = null;
        bool isDone = false;

        var GetProfileTask = dbReference
                        .Child("Profile")
                        .Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId)
                        .GetValueAsync().ContinueWith(task =>
                        {
                            if (task.IsCanceled)
                            {
                                Debug.LogError("R�cup�ration du profil annul�e.");
                                onComplete?.Invoke(null);
                                return;
                            }
                            if (task.IsFaulted)
                            {
                                Debug.LogError("Erreur de r�cup�ration du profil : " + task.Exception.Flatten().InnerExceptions[0]);
                                onComplete?.Invoke(null);
                                return;
                            }

                            // Convertit le JSON en objet Profile
                            DataSnapshot snapshot = task.Result;
                            string json = snapshot.GetRawJsonValue();
                            profile = JsonUtility.FromJson<Profile>(json);

                            Debug.Log("R�cup�ration du profil r�ussie");
                            isDone = true;
                            onComplete?.Invoke(profile);
                        });

        yield return new WaitUntil(predicate: () => isDone == true);
    }

    public IEnumerator DatabaseAddHistory(History history, Action<bool> onComplete = null)
    {
        string json = JsonUtility.ToJson(history);
        bool isDone = false;

        var NewHistory = dbReference
                        .Child("History")
                        .Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId)
                        .Push().SetRawJsonValueAsync(json).ContinueWith(task =>
                        {
                            if (task.IsCanceled)
                            {
                                Debug.LogError("Création de l'historique annulée.");
                                PopUp.PrepareMessagePopUp("Création de l'historique annulée.");
                                onComplete?.Invoke(false);
                                return;
                            }
                            else if (task.IsFaulted)
                            {
                                Debug.LogError("Erreur de création de l'historique : " + task.Exception.Flatten().InnerExceptions[0]);
                                onComplete?.Invoke(false);
                                return;
                            }
                            Debug.Log("Création de l'historique r�ussie");
                            isDone = true;
                            onComplete?.Invoke(true);
                        });

        yield return new WaitUntil(predicate: () => isDone == true);
    }

    public IEnumerator DatabaseGetHistory(Action<List<History>> onComplete)
    {
        List<History> history = null;
        bool isDone = false;

        var GetHistoryTask = dbReference
                        .Child("History")
                        .Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId)
                        .GetValueAsync().ContinueWith(task =>
                        {
                            if (task.IsCanceled)
                            {
                                Debug.LogError("R�cup�ration de l'historique annul�e.");
                                onComplete?.Invoke(null);
                                return;
                            }
                            if (task.IsFaulted)
                            {
                                Debug.LogError("Erreur de r�cup�ration de l'historique : " + task.Exception.Flatten().InnerExceptions[0]);
                                onComplete?.Invoke(null);
                                return;
                            }

                            // Convertit le JSON en objet History
                            DataSnapshot snapshot = task.Result;
                            string json = snapshot.GetRawJsonValue();
                            history = JsonUtility.FromJson<List<History>>(json);

                            Debug.Log("R�cup�ration de l'historique r�ussie");
                            isDone = true;
                            onComplete?.Invoke(history);
                        });

        yield return new WaitUntil(predicate: () => isDone == true);
    }
}
