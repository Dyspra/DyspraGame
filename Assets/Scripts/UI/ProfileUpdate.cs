using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUpdate : MonoBehaviour
{
    [SerializeField] InputField emailField;
    [SerializeField] InputField firstNameField;
    [SerializeField] InputField surnameField;
    [SerializeField] InputField usernameField;
    [SerializeField] Text creationDate;

    const string creationDateMessage = "Compte créé le ";
    bool isSavingProfile = false;

    void OnEnable()
    {
        BDDInteractor.Instance.RemoveCachedProfile();
        BDDInteractor.Instance.FetchProfile();
        FindObjectOfType<ConnectionUI>().EnableLoadingAnimate();
    }
    void Update()
    {
        Profile fetchedProfile = BDDInteractor.Instance.getCachedProfile();
        if (fetchedProfile != null)
        {
            if (isSavingProfile)
            {
                PopUp.DisplayPopUp("Profil sauvegardé !");
                isSavingProfile = false;
            }
            FindObjectOfType<ConnectionUI>().DisableLoadingAnimate();
            DisplayProfile(fetchedProfile);
            BDDInteractor.Instance.RemoveCachedProfile();
        }
    }

    void DisplayProfile(Profile profile)
    {
        emailField.text = BDDInteractor.Instance.GetCurrentUserEmail();
        firstNameField.text = profile.FirstName;
        surnameField.text = profile.SurName;
        usernameField.text = profile.Username;
        creationDate.text = creationDateMessage + profile.CreationDate;
    }

    public void SaveProfile()
    {
        Profile newProfile = new Profile(BDDInteractor.Instance.GetCurrentUserId(), usernameField.text, firstNameField.text, surnameField.text, creationDate.text.Replace(creationDateMessage, ""));

        BDDInteractor.Instance.UpdateProfile(newProfile);
        FindObjectOfType<ConnectionUI>().EnableLoadingAnimate();
        isSavingProfile = true;
    }
}