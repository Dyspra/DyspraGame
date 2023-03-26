using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDDInteractor : MonoBehaviour
{
    private FirebaseBDDController firebaseBDD;
    private FirebaseAuthController firebaseAuth;
    public Profile profile;
    void Start()
    {
        firebaseBDD = FindObjectOfType<FirebaseBDDController>();
        firebaseAuth = FindObjectOfType<FirebaseAuthController>();
        profile = new Profile();
    }

    [Button("Create Profile")]
    public void CreateProfile()
    {
        profile.UserId = firebaseAuth.GetCurrentUserId();
        StartCoroutine(firebaseBDD.DatabaseSetProfile(profile, (onComplete) =>
        {
            if (onComplete)
            {
                //charger ici l'interface en rapport avec le profile
            }
        }));
    }

    [Button("Get Profile")]
    public void GetProfile()
    {
        profile.UserId = firebaseAuth.GetCurrentUserId();
        StartCoroutine(firebaseBDD.DatabaseGetProfile((profileReceived) =>
        {
            if (profileReceived != null)
                profile = profileReceived;
        }));
    }
}
