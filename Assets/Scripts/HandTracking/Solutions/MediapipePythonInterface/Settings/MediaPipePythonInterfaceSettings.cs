using System.Collections;
using System.Collections.Generic;
using System.Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity;


public class MediaPipePythonInterfaceSettings : MonoBehaviour
{
    public TMP_Dropdown webCamDropdown;
    private PythonProcess mediaPipePlugin;

    private void Awake()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return new WaitUntil(() => mediaPipePlugin.isFinished());
        InitWebCamDropdown();
    }
    public static List<string> GetAllConnectedCameras()
    {
        var cameraNames = new List<string>();
        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
        {
            foreach (var device in searcher.Get())
            {
                cameraNames.Add(device["Caption"].ToString());
            }
        }

        return cameraNames;
    }
    private void InitWebCamDropdown()
    {
        webCamDropdown.ClearOptions();
        webCamDropdown.onValueChanged.RemoveAllListeners();

        var sourceNames = GetAllConnectedCameras();
 
        foreach (var sourceName in imageSource.sourceCandidateNames)
        {
            UnityEngine.Debug.Log(sourceName);
        }

        if (sourceNames == null)
        {
            webCamDropdown.enabled = false;
            return;
        }
        var options = new List<string>(sourceNames);
        webCamDropdown.AddOptions(options);

        var defaultValue = 0;

        if (defaultValue >= 0)
        {
            webCamDropdown.value = defaultValue;
        }

        webCamDropdown.onValueChanged.AddListener(delegate
        {
            if (webCamDropdown.value >= 0) {
                mediaPipePlugin.StopProcess();
                mediaPipePlugin.LaunchPythonScript(webCamDropdown.value); // to restart the solution
            }
        });
    }
}
