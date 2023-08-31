using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsBalloonFair : MonoBehaviour
{
    public void OnReplayExercise()
    {
        AnalyticsManager.Instance.LogExerciseReplay("1");
    }

    public void OnExerciseQuitAfterFinished()
    {
        AnalyticsManager.Instance.LogExerciseQuitAfterFinished("1");
    }
}
