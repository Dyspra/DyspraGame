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
        // Initialise la base de données Firebase
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
                                Debug.LogError("Création de profile annulée.");
                                onComplete?.Invoke(false);
                                return;
                            }
                            if (task.IsFaulted)
                            {
                                Debug.LogError("Erreur de création de profile : " + task.Exception.Flatten().InnerExceptions[0]);
                                onComplete?.Invoke(false);
                                return;
                            }
                            Debug.Log("Création de profile réussie");
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
                                Debug.LogError("Récupération du profil annulée.");
                                onComplete?.Invoke(null);
                                return;
                            }
                            if (task.IsFaulted)
                            {
                                Debug.LogError("Erreur de récupération du profil : " + task.Exception.Flatten().InnerExceptions[0]);
                                onComplete?.Invoke(null);
                                return;
                            }

                            // Convertit le JSON en objet Profile
                            DataSnapshot snapshot = task.Result;
                            string json = snapshot.GetRawJsonValue();
                            profile = JsonUtility.FromJson<Profile>(json);

                            Debug.Log("Récupération du profil réussie");
                            isDone = true;
                            onComplete?.Invoke(profile);
                        });

        yield return new WaitUntil(predicate: () => isDone == true);
    }

}
