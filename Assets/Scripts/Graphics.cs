using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Graphics : MonoBehaviour
{
    public void SetQuality(int i)
    {
        QualitySettings.SetQualityLevel(i);
    }

    public void SetAntialiasing(int i)
    {
        QualitySettings.antiAliasing = i * 2;
    }

    public void GetShadowsRes(int i)
    {
        if (i == 0)
            SetShadowsRes(ShadowResolution.Low);
        else if (i == 1)
            SetShadowsRes(ShadowResolution.Medium);
        else if (i == 2)
            SetShadowsRes(ShadowResolution.High);
        else
            SetShadowsRes(ShadowResolution.VeryHigh);
    }

    public void GetFPS(int i)
    {
        if (i == 0)
            SetFPS(30);
        else if (i == 1)
            SetFPS(60);
        else if (i == 2)
            SetFPS(120);
        else
            SetFPS(180);
    }

    void SetShadowsRes(ShadowResolution i)
    {
        QualitySettings.shadowResolution = i;
    }

    void SetFPS(int i)
    {
        Application.targetFrameRate = i;
    }
}
