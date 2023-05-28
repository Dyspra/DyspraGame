using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBackButton : MonoBehaviour
{
    [SerializeField] GameObject Settings;
    public void CloseSettings()
    {
        SettingsStatus settingsStatus = FindObjectOfType<SettingsStatus>();
        if (settingsStatus != null)
            settingsStatus.Close();
        else
            Destroy(Settings);
    }
}
