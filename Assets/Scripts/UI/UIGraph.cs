using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIGraph : MonoBehaviour
{
    [SerializeField] GameObject dot;
    [SerializeField] GameObject dotted_line;
    List<History> historyList;
    List<RectTransform> dots;
    List<RectTransform> dotted_lines;
    Vector2 origin = new Vector2(-500, 0);
    [SerializeField] RectTransform firstDotSeparator;
    [SerializeField] Toggle toggleDottedLines;
    
    //HistoryInfos displays the date and score of each dot
    [SerializeField] RectTransform HistoryInfos;
    static RectTransform HistoryInfosStatic;

    //ExerciceSwapper
    [SerializeField] int[] exercicesOrder = new int[4];
    [SerializeField] List<Image> exerciceImages = new List<Image>();
    int exerciceOrderIndex = 0;
    [SerializeField] Text exerciceTitle;

    const int X_RANGE = 1000;
    const int Y_RANGE = 500;
    const int MIN_DOTTED_LINE_DIST = 100;

    void Start()
    {
        historyList = null;
        dots = new List<RectTransform>();
        dotted_lines = new List<RectTransform>();
        HistoryInfosStatic = HistoryInfos;

        DisplayExerciceProgression();
    }

    public void ToggleDottedLine()
    {
        dotted_lines.ForEach(d => d.gameObject.SetActive(toggleDottedLines.isOn));
    }

    public void SwapExercice(int indexModifier)
    {
        exerciceOrderIndex += indexModifier;
        if (exerciceOrderIndex < 0)
        {
            exerciceOrderIndex = exercicesOrder.Length - 1;
        }
        else if (exerciceOrderIndex >= exercicesOrder.Length)
        {
            exerciceOrderIndex = 0;
        }
        DisplayExerciceProgression();
    }

    async void DisplayExerciceProgression()
    {
        SetUpDotSeparator(firstDotSeparator, origin, origin); //Cache le premier séparateur (au cas où l'historique serait nul)
        dots.ForEach(dot => Destroy(dot.gameObject));
        dots.Clear();
        dotted_lines.ForEach(dotted_line => Destroy(dotted_line.gameObject));
        dotted_lines.Clear();
        exerciceTitle.text = ExerciseConstants.Exercises[exercicesOrder[exerciceOrderIndex].ToString()].Name;
        exerciceImages.ForEach(image => image.gameObject.SetActive(image.gameObject.name == exerciceOrderIndex.ToString()));

        await GetHistory();
    }

    public static void DisplayHistoryInfos(History history, Vector2 dotPos)
    {
        HistoryInfosStatic.gameObject.SetActive(true);
        HistoryInfosStatic.anchoredPosition = new Vector2(dotPos.x + 25, dotPos.y - 250);
        HistoryInfosStatic.GetComponentInChildren<Text>().text = "Fait le " + history.CreationDate + "\r\nScore : " + history.Score;
    }

    public static void HideHistoryInfos()
    {
        HistoryInfosStatic.gameObject.SetActive(false);
    }

    async Task GetHistory()
    {
        historyList = await BDDInteractor.Instance.FetchHistory();
        historyList = historyList.OrderBy(h => DateTime.ParseExact(h.CreationDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
        
        if (historyList == null) return;
        string correctExerciceId = exercicesOrder[exerciceOrderIndex].ToString();
        historyList.RemoveAll(history => history.ExerciseId != correctExerciceId); //Supprime les historiques des autres exercices
        if (historyList.Count == 0) return;


        int i = 1;
        float interval = X_RANGE / (historyList.Count + 1); //interval entre les points sur l'axe X
        int max_score = historyList.Count == 1 ? historyList[0].Score * 2 : historyList.Max(h => h.Score); //Calcule le score le plus élevé de la liste, pour l'échelle du graphique


        foreach (History history in historyList)
        {
            RectTransform newDot = Instantiate(dot, transform).GetComponent<RectTransform>();
            Debug.Log(historyList.Count);
            newDot.anchoredPosition = new Vector2(interval * i - (X_RANGE / 2), history.Score / (float)max_score * Y_RANGE);

            if (dots.Count == 0) //Si premier point, le séparateur est celui déjà présent dans la scène
                SetUpDotSeparator(firstDotSeparator, origin, newDot.anchoredPosition);
            
            else //Si point déjà existant, le séparateur est présent dans la prefab du point
                SetUpDotSeparator(dots[dots.Count - 1].transform.Find("Dot_separator").GetComponent<RectTransform>(), dots[dots.Count - 1].anchoredPosition, newDot.anchoredPosition);

            newDot.GetComponent<Dot>().History = history;
            dots.Add(newDot);

            if (dotted_lines.Count == 0 || !dotted_lines.Find(d => Mathf.Abs(d.anchoredPosition.y - newDot.anchoredPosition.y) <= MIN_DOTTED_LINE_DIST))
            {
                RectTransform newDottedLine = Instantiate(dotted_line, transform).GetComponent<RectTransform>();
                newDottedLine.transform.Find("Text - ScoreValue").GetComponent<Text>().text = history.Score.ToString();
                newDottedLine.anchoredPosition = new Vector2(newDottedLine.anchoredPosition.x, newDot.anchoredPosition.y - 1);
                dotted_lines.Add(newDottedLine);
            }

            i++;
        }
        toggleDottedLines.isOn = true;
    }

    void SetUpDotSeparator(RectTransform dotSeparator, Vector2 a, Vector2 b)
    {
        //Calcul de la longueur du séparateur
        float dotSeparatorWidth = Vector2.Distance(a, b);
        dotSeparator.sizeDelta = new Vector2(dotSeparatorWidth, dotSeparator.sizeDelta.y);

        //Calcul de l'angle
        ApplyRotationToRectTransform(dotSeparator, CalculateRotationZ(a, b));
    }

    float CalculateRotationZ(Vector2 a, Vector2 b)
    {
        Vector2 direction = b - a;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }

    void ApplyRotationToRectTransform(RectTransform rectTransform, float angleZ)
    {
        if (rectTransform != null)
        {
            Vector3 currentRotation = rectTransform.localEulerAngles;
            currentRotation.z = angleZ;
            rectTransform.localEulerAngles = currentRotation;
        }
    }
}
