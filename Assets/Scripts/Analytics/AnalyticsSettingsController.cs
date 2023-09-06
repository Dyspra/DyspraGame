using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnalyticsSettingsController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdownScreenMode;
    [SerializeField] private TMP_Dropdown dropdownQuality;


    // Volume settings page

    void Start()
    {
        AnalyticsManager.Instance.LogOpenSettings();

        // Add listener for screen mode dropdown
        dropdownScreenMode.onValueChanged.AddListener(delegate {
            AnalyticsManager.Instance.LogS_ChangeScreenMode(dropdownScreenMode.options[dropdownScreenMode.value].text);
        });

        // Add listener for quality dropdown
        dropdownQuality.onValueChanged.AddListener(delegate {
            AnalyticsManager.Instance.LogS_ChangeQuality(dropdownQuality.options[dropdownQuality.value].text);
        });
    }

}
