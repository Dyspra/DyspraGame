using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarMenu : MonoBehaviour
{
    [SerializeField] ProfileUpdate profile;
    public Avatar displayedAvatar;
    
    [SerializeField] Image body;
    [SerializeField] Image face;
    [SerializeField] Image hair;
    [SerializeField] Image kit;
    [SerializeField] Text bodyPart;
    
    int selectedBodyPart;

    void Start()
    {
        displayedAvatar = profile.displayedAvatar;
        if (displayedAvatar == null)
        {
            displayedAvatar = new Avatar(0, 0, 0, 0);
        }
        UpdateAvatar(body, face, hair, kit, displayedAvatar);
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
                displayedAvatar.body += update;
                break;
            case 1:
                displayedAvatar.face += update;
                break;
            case 2:
                displayedAvatar.hair += update;
                break;
            case 3:
                displayedAvatar.kit += update;
                break;
        }
        UpdateAvatar(body, face, hair, kit, displayedAvatar);
    }

    public static void UpdateAvatar(Image body, Image face, Image hair, Image kit, Avatar displayedAvatar)
    {
        SetUpAvatarPart(body, "Bodies/Body_", ref displayedAvatar.body);
        SetUpAvatarPart(face, "Faces/Face_", ref displayedAvatar.face);
        SetUpAvatarPart(hair, "Hairs/Hair_", ref displayedAvatar.hair);
        SetUpAvatarPart(kit, "Kits/Kit_", ref displayedAvatar.kit);
    }

    static void SetUpAvatarPart(Image partImage, string path, ref int partNb)
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
