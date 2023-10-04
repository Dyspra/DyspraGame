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
        OpenedSettings.GetComponent<Canvas>().worldCamera = Camera.main;
        OpenedSettings.GetComponent<Canvas>().planeDistance = 0.5f;
        OpenedSettings.transform.SetAsLastSibling();
    }

    public void Close()
    {
        if (OpenedSettings != null)
            Destroy(OpenedSettings);
    }
}