using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsStatus : MonoBehaviour
{
    [SerializeField] GameObject Settings;
    GameObject OpenedSettings;
    public void Open()
    {
        Close();
        OpenedSettings = Instantiate(Settings);
    }

    public void Close()
    {
        if (OpenedSettings != null)
            Destroy(OpenedSettings);
    }
}