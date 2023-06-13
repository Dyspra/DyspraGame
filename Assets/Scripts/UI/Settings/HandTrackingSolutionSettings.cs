using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class HandTrackingSolutionSettings : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private List<IHandTrackingSolution> solutions;

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
    }
    public void ChangeSolution(int value)
    {
        IHandTrackingSolution selectedSolution = solutions[value];
        HandTrackingManager.Instance.ChangeSelectedSolution(selectedSolution.id);
    }
}
