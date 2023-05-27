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

    public void CreateProfile(string firstname = "", string surname = "", string username = "")
    {
        profile = new Profile();
        profile.UserId = firebaseAuth.GetCurrentUserId();
        if (firstname != "") profile.FirstName = firstname;
        if (surname != "") profile.SurName = surname;
        if (username != "") profile.Username = username;

        StartCoroutine(firebaseBDD.DatabaseSetProfile(profile, (onComplete) =>
        {
            if (onComplete)
            {
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

    public Profile getCachedProfile()
    {
        return profile;
    }

    public void RemoveProfile()
    {
        profile = null;
    }
}
