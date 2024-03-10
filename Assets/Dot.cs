using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dot : MonoBehaviour
{
    [SerializeField]
    private History history;
    [SerializeField]
    private RectTransform rectTransform;

    /// <summary>
    /// Propriété pour accéder et modifier l'historique associé au point.
    /// </summary>
    public History History
    {
        get { return history; }
        set { history = value; }
    }

    private void OnMouseOver()
    {
        UIGraph.DisplayHistoryInfos(history, rectTransform.anchoredPosition);
        Debug.Log("Mouse is over GameObject.");
    }

    private void OnMouseExit()
    {
        UIGraph.HideHistoryInfos();
        Debug.Log("Mouse has exited GameObject.");
    }
}
