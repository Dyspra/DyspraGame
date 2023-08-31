using UnityEngine;

public class AnalyticsExerciseListOpen : MonoBehaviour {
    private void OnEnable() {
        AnalyticsManager.Instance.LogOpenExerciseList();
    }   
}
