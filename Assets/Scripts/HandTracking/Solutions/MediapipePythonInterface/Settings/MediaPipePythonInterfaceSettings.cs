using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity;


public class MediaPipePythonInterfaceSettings : MonoBehaviour
{
    public TMP_Dropdown webCamDropdown;
    private MediaPipePythonInterface mediaPipePlugin;  
    private List<string> cameraNames = new List<string>();

    private void Awake()
    {
        mediaPipePlugin = HandTrackingManager.Instance.GetSolution("mediapipe-python-interface") as MediaPipePythonInterface;
        InitWebCamDropdown();
    }

/*     private IEnumerator Init()
    {
        yield return new WaitUntil(() => mediaPipePlugin.process.isFinished());
    } */
    private void GetAllConnectedCameras()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++) {
            cameraNames.Add(devices[i].name);
        }
    }
    private void InitWebCamDropdown()
    {
        webCamDropdown.ClearOptions();
        webCamDropdown.onValueChanged.RemoveAllListeners();

        GetAllConnectedCameras();
        foreach (var sourceName in cameraNames)
        {
            UnityEngine.Debug.LogWarning(sourceName);
        }

        if (cameraNames.Count == 0)
        {
            webCamDropdown.enabled = false;
            return;
        }
/*         var options = new List<string>(sourceNames); */
        webCamDropdown.AddOptions(cameraNames);

        var defaultValue = 0;

        if (defaultValue >= 0)
        {
            webCamDropdown.value = defaultValue;
        }

        webCamDropdown.onValueChanged.AddListener(delegate
        {
            if (webCamDropdown.value >= 0) {
                mediaPipePlugin.process.LaunchPythonScript(webCamDropdown.value); // to restart the solution
            }
        });
    }
}
