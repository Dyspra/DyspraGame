using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity;


public class MediaPipePluginSettings : MonoBehaviour
{
    public Mediapipe.Unity.Screen webCamScreen;
    public TMP_Dropdown webCamDropdown;

    private MediaPipePlugin mediaPipePlugin;


    private void Awake()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        mediaPipePlugin = HandTrackingManager.Instance.GetSolution("mediapipe-plugin") as MediaPipePlugin;
        yield return new WaitUntil(() => mediaPipePlugin.bootstrap != null && mediaPipePlugin.bootstrap.isFinished);
        mediaPipePlugin.SetScreen(webCamScreen);
        InitWebCamDropdown();
    }

    private void InitWebCamDropdown()
    {
        webCamDropdown.ClearOptions();
        webCamDropdown.onValueChanged.RemoveAllListeners();

        var imageSource = ImageSourceProvider.ImageSource;
        var sourceNames = imageSource.sourceCandidateNames;

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

        var currentSourceName = imageSource.sourceName;
        var defaultValue = options.FindIndex(option => option == currentSourceName);

        if (defaultValue >= 0)
        {
            webCamDropdown.value = defaultValue;
        }

        webCamDropdown.onValueChanged.AddListener(delegate
        {
            imageSource.SelectSource(webCamDropdown.value);
            mediaPipePlugin.solution.Play(); // to restart the solution
        });
    }
}
