using Firebase.Auth;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDDInteractor : SingletonGameObject<BDDInteractor>
{
    FirebaseBDDController firebaseBDD;
    FirebaseAuthController firebaseAuth;

    Profile profile = null;
    Profile waitingRegisterProfile = null;
    void Awake()
    {
        firebaseBDD = gameObject.AddComponent<FirebaseBDDController>();
        firebaseAuth = gameObject.AddComponent<FirebaseAuthController>();
    }

    void Update()
    {
        if (waitingRegisterProfile != null && firebaseAuth.GetIsConnected())
        {
            CreateProfile(waitingRegisterProfile.FirstName, waitingRegisterProfile.SurName);
            waitingRegisterProfile = null;
        }
    }

    #region Profile

    public void CreateProfile(string firstname = "", string surname = "", string username = "")
    {
        Profile newProfile = new Profile();
        newProfile.UserId = firebaseAuth.GetCurrentUserId();
        if (firstname != "") newProfile.FirstName = firstname;
        if (surname != "") newProfile.SurName = surname;
        if (username != "") newProfile.Username = username;

        StartCoroutine(firebaseBDD.DatabaseSetProfile(newProfile, (onComplete) =>
        {
            if (onComplete)
            {
                profile = newProfile;
            }
        }));
    }

    public void UpdateProfile(Profile updatedProfile)
    {
        StartCoroutine(firebaseBDD.DatabaseSetProfile(updatedProfile, (onComplete) =>
        {
            if (onComplete)
            {
                profile = updatedProfile;
            }
        }));
    }

    public void FetchProfile()
    {
        if (firebaseAuth.GetCurrentUserId() == "") return;
        StartCoroutine(firebaseBDD.DatabaseGetProfile((profileReceived) =>
        {
            if (profileReceived != null)
                profile = profileReceived;
        }));
    }

    public Profile getCachedProfile()
    {
        return profile;
    }

    public void RemoveProfile()
    {
        profile = null;
    }

    #endregion

    #region Authentification

    public void Register(string email, string password, string firstname, string surname)
    {
        firebaseAuth.Register(email, password);
        waitingRegisterProfile = new Profile();
        waitingRegisterProfile.FirstName = firstname;
        waitingRegisterProfile.SurName = surname;
    }

    public void LogIn(string email, string password)
    {
        firebaseAuth.LogIn(email, password);
    }

    public void LogOut()
    {
        firebaseAuth.LogOut();
    }

    public bool isUserAuthentified()
    {
        return firebaseAuth.GetIsConnected();
    }

    public string GetCurrentUserEmail()
    {
        return firebaseAuth.GetCurrentUserEmail();
    }

    public string GetCurrentUserId()
    {
        return firebaseAuth.GetCurrentUserId();
    }
    
    #endregion

    #region History

    public void AddHistory(string exerciceId, string score)
    {
        History history = new History();
        history.UserId = firebaseAuth.GetCurrentUserId();
        history.ExerciceId = exerciceId;
        history.Score = score;

        StartCoroutine(firebaseBDD.DatabaseAddHistory(history));
    }

    public void GetHistory()
    {
        StartCoroutine(firebaseBDD.DatabaseGetHistory((historyReceived) =>
        {
            if (historyReceived != null)
                HistoryManager.Instance.SetHistory(historyReceived);
        }));
    }

    #endregion
}
