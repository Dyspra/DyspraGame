using UnityEngine;

public class AnalyticsExerciseHistoryOpen : MonoBehaviour {
    private void OnEnable() {
        AnalyticsManager.Instance.LogOpenExerciseHistory();
    }   
}
