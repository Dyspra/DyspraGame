using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarMenu : MonoBehaviour
{
    [SerializeField] ProfileUpdate profile;
    public Avatar DisplayedAvatar;
    
    [SerializeField] Image body;
    [SerializeField] Image face;
    [SerializeField] Image hair;
    [SerializeField] Image kit;
    [SerializeField] Text bodyPart;
    
    int selectedBodyPart;

    void Start()
    {
        DisplayedAvatar = profile.displayedAvatar;
        if (DisplayedAvatar == null)
        {
            DisplayedAvatar = new Avatar();
        }
        UpdateAvatar();
    }

    public void changeSelectedBodyPart(int update)
    {
        selectedBodyPart += update;
        if (selectedBodyPart > 3) selectedBodyPart = 0;
        if (selectedBodyPart < 0) selectedBodyPart = 3;

        bodyPart.text =
            (selectedBodyPart == 0) ? "Corps" :
            (selectedBodyPart == 1) ? "Visage" :
            (selectedBodyPart == 2) ? "Cheveux" :
            "Vêtements";
    }

    public void changeBodyPart(int update)
    {
        switch (selectedBodyPart)
        {
            case 0:
                DisplayedAvatar.body += update;
                break;
            case 1:
                DisplayedAvatar.face += update;
                break;
            case 2:
                DisplayedAvatar.hair += update;
                break;
            case 3:
                DisplayedAvatar.kit += update;
                break;
        }
        UpdateAvatar();
    }

    void UpdateAvatar()
    {
        SetUpAvatarPart(body, "Bodies/Body_", ref DisplayedAvatar.body);
        SetUpAvatarPart(face, "Faces/Face_", ref DisplayedAvatar.face);
        SetUpAvatarPart(hair, "Hairs/Hair_", ref DisplayedAvatar.hair);
        SetUpAvatarPart(kit, "Kits/Kit_", ref DisplayedAvatar.kit);
    }

    void SetUpAvatarPart(Image partImage, string path, ref int partNb)
    {
        Sprite spriteToLoad = Resources.Load<Sprite>(path + (partNb + 1));
        if (spriteToLoad != null)
        {
            partImage.sprite = spriteToLoad;
        }
        else
        {
            partNb = 0;
            partImage.sprite = Resources.Load<Sprite>(path + 1);
        }
    }
}
