using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class ExercisesManager : MonoBehaviour
{
    public void PlayExercise(string id)
    {
        AnalyticsManager.Instance.LogExerciseStarted(id);
        UnityEngine.SceneManagement.SceneManager.LoadScene(ExerciseConstants.Exercises[id].Scene);
    }
}
