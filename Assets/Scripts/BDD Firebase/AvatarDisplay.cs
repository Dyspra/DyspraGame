using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarDisplay : MonoBehaviour
{
    Image body;
    Image face;
    Image hair;
    Image kit;
    [SerializeField] Image logo;
    Profile profile;

    void Start()
    {
        body = transform.Find("Body").GetComponent<Image>();
        face = transform.Find("Face").GetComponent<Image>();
        hair = transform.Find("Hair").GetComponent<Image>();
        kit = transform.Find("Kit").GetComponent<Image>();
    }

    private void OnEnable()
    {
        BDDInteractor.Instance.RemoveCachedProfile();
        BDDInteractor.Instance.FetchProfile();
    }

    private void OnDisable()
    {
        logo.enabled = true;
        body.enabled = false;
        face.enabled = false;
        hair.enabled = false;
        kit.enabled = false;
        profile = null;
    }

    void Update()
    {
        if (profile == null || profile.Avatar == null)
        {
            profile = BDDInteractor.Instance.getCachedProfile();
            if (profile != null && profile.Avatar != null)
            {
                AvatarMenu.UpdateAvatar(body, face, hair, kit, profile.Avatar);
                logo.enabled = false;
                body.enabled = true;
                face.enabled = true;
                hair.enabled = true;
                kit.enabled = true;
            }
            else
            {
                logo.enabled = true;
                body.enabled = false;
                face.enabled = false;
                hair.enabled = false;
                kit.enabled = false;
            }
            BDDInteractor.Instance.RemoveCachedProfile();
        }
    }
}
