using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class ExercisesManager : MonoBehaviour
{
    public static string exerciseId;
    public void PlayExercise()
    {
        AnalyticsManager.Instance.LogExerciseStarted(exerciseId);
        UnityEngine.SceneManagement.SceneManager.LoadScene(ExerciseConstants.Exercises[exerciseId].Scene);
    }
}
