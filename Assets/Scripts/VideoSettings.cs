using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VideoSettings : MonoBehaviour
{
    [Header("Dropdowns")]
    public Dropdown Resolution;
    public Dropdown display;

    private Resolution[] storeResolutions;
    private FullScreenMode screenMode;
    private int countRes;


    void AddResolution(Resolution[] res)
    {
        countRes = 0;

        for (int i = 0; i < res.Length; i++) {
            if (res[i].refreshRate == Screen.currentResolution.refreshRate
            && (res[i].width > 800 && res[i].height > 800)) {
                storeResolutions[countRes] = res[i];
                countRes++;
            }
        }

        for (int i = 0; i < countRes; i++) {
            Resolution.options.Add(new Dropdown.OptionData(ResolutionToString(storeResolutions[i])));
        }
    }

    void ScreenOptions(string mode)
    {
        if (mode == "Fullscreen")
            screenMode = FullScreenMode.ExclusiveFullScreen;
        else if (mode == "Windowed")
            screenMode = FullScreenMode.Windowed;
        else
            screenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreenMode = screenMode;
    }

    void ResolutionInitialize(Resolution[] res)
    {
        for (int i = 0; i < res.Length; i ++) {
            if (Screen.width == res[i].width && Screen.height == res[i].height)
                Resolution.value = i;
        }
        Resolution.RefreshShownValue();
    }

    void ScreenInitialize()
    {
        if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen) {
            display.value = 0;
            screenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (Screen.fullScreenMode == FullScreenMode.Windowed) {
            display.value = 1;
            screenMode = FullScreenMode.Windowed;
        }
        else {
            display.value = 2;
            screenMode = FullScreenMode.FullScreenWindow;
        }
        display.RefreshShownValue();
    }

    string ResolutionToString(Resolution screenRes)
    {
        return screenRes.width + " x " + screenRes.height;
    }

    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;
        Array.Reverse(resolutions);
        storeResolutions = new Resolution[resolutions.Length];

        ScreenInitialize();
        AddResolution(resolutions);
        ResolutionInitialize(storeResolutions);

        display.onValueChanged.AddListener(delegate {ScreenOptions(display.options[display.value].text);});
        Resolution.onValueChanged.AddListener(delegate
        {
            Screen.SetResolution(storeResolutions[Resolution.value].width,
            storeResolutions[Resolution.value].height, screenMode);
        });
    }
}
