using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WindowMode : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private bool isFullscreen = false;
    private Resolution currentResolution;

    private void Start()
    {
        currentResolution = Screen.currentResolution;

        dropdown.ClearOptions();
        dropdown.AddOptions(new List<TMP_Dropdown.OptionData> { new TMP_Dropdown.OptionData("Fenêtré"), new TMP_Dropdown.OptionData("Plein écran") });

        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        switch (index)
        {
            case 0:
                if (isFullscreen)
                {
                    ToggleFullscreen();
                    isFullscreen = false;
                }
                break;
            case 1:
                if (!isFullscreen)
                {
                    ToggleFullscreen();
                    isFullscreen = true;
                }
                break;
        }
    }

    private void ToggleFullscreen()
    {
        if (isFullscreen)
        {
            Screen.SetResolution(currentResolution.width, currentResolution.height, false);
        }
        else
        {
            Resolution maxResolution = Screen.resolutions[Screen.resolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
    }
}