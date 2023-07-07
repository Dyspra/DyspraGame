using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class HandTrackingSolutionSettings : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private List<IHandTrackingSolution> solutions;
    
    private IHandTrackingSolution selectedSolution;
    public GameObject solutionSettingsParent;
    private GameObject currentSettings;

    void Awake()
    {
        List<string> solutionNames = new List<string>();
        solutions = HandTrackingManager.Instance.GetAllSolutions();

        foreach (IHandTrackingSolution solution in solutions)
        {
            solutionNames.Add(solution.displayName);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(solutionNames);
        dropdown.onValueChanged.AddListener(ChangeSolution);
        
        selectedSolution = HandTrackingManager.Instance.HandTracking;
        dropdown.value = solutions.IndexOf(selectedSolution);
        UpdateSettings();
    }
    public void ChangeSolution(int value)
    {
        selectedSolution = solutions[value];
        HandTrackingManager.Instance.ChangeSelectedSolution(selectedSolution.id);
        UpdateSettings();
    }

    private void UpdateSettings()
    {
        if (selectedSolution != null)
        {
            if (currentSettings != null)
            {
                Destroy(currentSettings);
            }
            if (selectedSolution.settingsPrefab != null)
            {
                GameObject settings = Instantiate(selectedSolution.settingsPrefab, solutionSettingsParent.transform);
                settings.transform.localPosition = Vector3.zero;
                settings.transform.localRotation = Quaternion.identity;
                settings.transform.localScale = Vector3.one;
                // make the dimension of settings the same as the parent
                RectTransform rectTransform = settings.GetComponent<RectTransform>();
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
                currentSettings = settings;
            }
        }
    }
}
