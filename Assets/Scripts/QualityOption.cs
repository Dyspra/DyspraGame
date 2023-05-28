using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class QualityOption : MonoBehaviour
{
    public RenderPipelineAsset[] qualityLevels;
    public TMP_Dropdown dropdown;

    void Start()
    {
        dropdown.value = QualitySettings.GetQualityLevel();
    }

    public void ChangeLevel(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevels[value];
    }
}
