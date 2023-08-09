using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Constants;

public class HistoryManager : MonoBehaviour
{
    public GameObject historyItemPrefab;
    public Transform historyListTransform;
    // public GameObject loadingSpinner;

    private List<History> historyList;

    private async void Start()
    {
        await DisplayHistoryAsync();
    }

    public async Task DisplayHistoryAsync()
    {
        // loadingSpinner.SetActive(true);

        historyList = await BDDInteractor.Instance.FetchHistory();
        UnityEngine.Debug.Log("History list count: " + historyList.Count);
        // reverse the list to have the most recent history at the top
        historyList.Reverse();

        // loadingSpinner.SetActive(false);

        foreach (History history in historyList)
        {
            GameObject historyItem = Instantiate(historyItemPrefab, historyListTransform);
            var exerciseName = history.ExerciseId;
            historyItem.transform.Find("ExerciseName").GetComponent<Text>().text = ExerciseConstants.Exercises[history.ExerciseId].Name;
            historyItem.transform.Find("Score").GetComponent<Text>().text = history.Score.ToString() + " points";
            historyItem.transform.Find("Date").GetComponent<Text>().text = history.CreationDate;
        }
    }
}
