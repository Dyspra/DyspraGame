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
    
    const int X_RANGE = 1000;
    const int Y_RANGE = 500;
    const int MIN_DOTTED_LINE_DIST = 100;

    async void Start()
    {
        historyList = null;
        dots = new List<RectTransform>();
        dotted_lines = new List<RectTransform>();
        await GetHistory();
    }

    async Task GetHistory()
    {
        historyList = await BDDInteractor.Instance.FetchHistory();
        historyList = historyList.OrderBy(h => DateTime.ParseExact(h.CreationDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
        
        if (historyList == null || historyList.Count == 0) return;
        
        int i = 1;
        float interval = X_RANGE / (historyList.Count + 1);
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
