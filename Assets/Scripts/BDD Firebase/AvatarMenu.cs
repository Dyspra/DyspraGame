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
    int totalBodies, totalFaces, totalHairs, totalKits;

    void Start()
    {
        displayedAvatar = profile.displayedAvatar;
        if (displayedAvatar == null)
        {
            displayedAvatar = new Avatar(0, 0, 0, 0);
        }

        totalBodies = GetTotalItems("Bodies/Body_");
        totalFaces = GetTotalItems("Faces/Face_");
        totalHairs = GetTotalItems("Hairs/Hair_");
        totalKits = GetTotalItems("Kits/Kit_");

        UpdateAvatar(body, face, hair, kit, displayedAvatar);
    }

    int GetTotalItems(string resourcePath)
    {
        int count = 0;
        while (Resources.Load<Sprite>(resourcePath + (count + 1)) != null)
        {
            count++;
        }
        return count;
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
                displayedAvatar.body = CalculateNewPartIndex(displayedAvatar.body, update, totalBodies);
                break;
            case 1:
                displayedAvatar.face = CalculateNewPartIndex(displayedAvatar.face, update, totalFaces);
                break;
            case 2:
                displayedAvatar.hair = CalculateNewPartIndex(displayedAvatar.hair, update, totalHairs);
                break;
            case 3:
                displayedAvatar.kit = CalculateNewPartIndex(displayedAvatar.kit, update, totalKits);
                break;
        }

        UpdateAvatar(body, face, hair, kit, displayedAvatar);
    }

    int CalculateNewPartIndex(int current, int update, int total)
    {
        int newIndex = current + update;
        if (newIndex >= total) newIndex = 0;
        if (newIndex < 0) newIndex = total - 1;
        return newIndex;
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
