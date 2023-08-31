using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FPSSwitcher : MonoBehaviour
{
    public TMP_Dropdown fpsDropdown;

    private Dictionary<string, int> fpsOptions = new Dictionary<string, int>
    {
        { "60 FPS", 60 },
        { "120 FPS", 120 },
        { "180 FPS", 180 },
        { "320 FPS", 320 }
    };

    private void Start()
    {
        fpsDropdown.AddOptions(new List<string>(fpsOptions.Keys));
        fpsDropdown.SetValueWithoutNotify(GetFPSIndex(Application.targetFrameRate));
        fpsDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        string selectedOption = fpsDropdown.options[index].text;
        int selectedFPS = fpsOptions[selectedOption];

        Application.targetFrameRate = selectedFPS;
        AnalyticsManager.Instance.LogS_ChangeFPSLimit(selectedFPS);
    }

    private int GetFPSIndex(int fps)
    {
        foreach (var kvp in fpsOptions)
        {
            if (kvp.Value == fps)
                return fpsDropdown.options.FindIndex(option => option.text == kvp.Key);
        }

        return 0;
    }
}
