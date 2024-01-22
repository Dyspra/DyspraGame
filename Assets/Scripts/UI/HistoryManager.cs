using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Constants;
using System.Linq;
using System;
using System.Globalization;

public class HistoryManager : MonoBehaviour
{
    public GameObject historyItemPrefab;
    public Transform historyListTransform;
    // public GameObject loadingSpinner;

    private List<History> historyList;

    [SerializeField] Text exerciseTitle;

    private async void Start()
    {
        await DisplayHistoryAsync();
    }

    public async Task DisplayHistoryAsync()
    {
        // loadingSpinner.SetActive(true);

        historyList = await BDDInteractor.Instance.FetchHistory();

        try
        {
            historyList = historyList
                .OrderBy(h => DateTime.TryParseExact(h.CreationDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : DateTime.MaxValue)
                .ToList();
        }
        catch (FormatException)
        {
            foreach (History h in historyList)
            {
                Debug.Log("Erreur de format de date pour l'historique.");
            }
        }
        Debug.Log("History list count: " + historyList.Count);
        // reverse the list to have the most recent history at the top
        historyList.Reverse();

        // loadingSpinner.SetActive(false);

        foreach (History history in historyList)
        {
            GameObject historyItem = Instantiate(historyItemPrefab, historyListTransform);
            historyItem.transform.Find("ExerciseName").GetComponent<Text>().text = ExerciseConstants.Exercises[history.ExerciseId].Name;
            historyItem.transform.Find("Score").GetComponent<Text>().text = history.Score.ToString() + " points";
            historyItem.transform.Find("Date").GetComponent<Text>().text = history.CreationDate;
        }
    }

    public void SelectHistory(string exerciseId)
    {
        exerciseTitle.text = ExerciseConstants.Exercises[exerciseId].Name;
        ExercisesManager.exerciseId = exerciseId;
        for (int i = 0; i < historyListTransform.childCount; i++)
        {
            Transform historyItem = historyListTransform.GetChild(i);
            Transform exercicename = historyItem.transform.Find("ExerciseName");
            historyItem.gameObject.SetActive(exercicename.GetComponent<Text>().text == ExerciseConstants.Exercises[exerciseId].Name);
        }
    }
}