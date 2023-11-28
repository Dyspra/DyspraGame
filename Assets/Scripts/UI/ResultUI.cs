using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [SerializeField] GameObject hasAvatarWindow;
    [SerializeField] GameObject noAvatarWindow;
    [SerializeField] AvatarDisplay display;
    void Update()
    {
        if (hasAvatarWindow.activeSelf != display.isAvatarDisplayed)
        {
            hasAvatarWindow.SetActive(display.isAvatarDisplayed);
            noAvatarWindow.SetActive(!display.isAvatarDisplayed);
        }
    }
}
