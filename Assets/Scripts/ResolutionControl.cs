using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionControl : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resDropdown;

    private Resolution[] res;
    private List<Resolution> filteredRes;

    private float currentRefreshRate;
    private int currentResInd = 0;

    void Start()
    {
        res = Screen.resolutions;
        filteredRes = new List<Resolution>();

        resDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < res.Length; i++) {
            if (res[i].refreshRate == currentRefreshRate)
                filteredRes.Add(res[i]);
        }
        
        List<string> options = new List<string>();
        for (int i = 0; i < filteredRes.Count; i++) {
            string resolutionOption = filteredRes[i].width + "x" + filteredRes[i].height + " " + filteredRes[i].refreshRate + " Hz";
            options.Add(resolutionOption);
            if (filteredRes[i].width == Screen.width && filteredRes[i].height == Screen.height)
                currentResInd = i;
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentResInd;
        resDropdown.RefreshShownValue();
    }

    

    public void SetRes(int resInd)
    {
        Resolution resolution = filteredRes[resInd];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
}