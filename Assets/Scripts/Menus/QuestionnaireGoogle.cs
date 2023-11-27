using NaughtyAttributes;
using UnityEngine;

public class QuestionnaireGoogle : MonoBehaviour
{
    [SerializeField] private GameObject m_windowExercices;
    [SerializeField] private GameObject m_windowForms;

    private int m_currentPlayerPrefHasClickedOnForm = 0; //0: default, 1: played but didn't open form menu, 2: opened form
    public static bool allowQuestionnaire = false;

    private void Start()
    {
        m_currentPlayerPrefHasClickedOnForm = PlayerPrefs.GetInt("HasClickedOnForm");

        if (m_currentPlayerPrefHasClickedOnForm == 1 && allowQuestionnaire == true)
        {
            allowQuestionnaire = false;
            m_windowExercices.SetActive(false);
            m_windowForms.SetActive(true);
        }
    }

    public void OpenDyspraxicForm()
    {
        ClickedOnForm();
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSd1v9PpncA7xwtDMpAPq0s2dBVIvI0EFHf4ANXWfCEkD6NFGw/viewform?usp=sf_link");
    }

    public void OpenNonDyspraxicForm()
    {
        ClickedOnForm();
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSfxtVfsj-gJgjg5qHk7D15l34jfu5Ol4CDW3gAV9lf2YV4-Hg/viewform?usp=sf_link");
    }

    private void ClickedOnForm()
    {
        if (m_currentPlayerPrefHasClickedOnForm != 2)
        {
            m_currentPlayerPrefHasClickedOnForm = 2;
            PlayerPrefs.SetInt("HasClickedOnForm", 2);
        }
    }

    public void OnClickedGame()
    {
        if (m_currentPlayerPrefHasClickedOnForm != 2)
        {
            PlayerPrefs.SetInt("HasClickedOnForm", 1);
        }
    }

    [Button]
    private void ResetQuestionnairePref()
    {
        m_currentPlayerPrefHasClickedOnForm = 0;
        PlayerPrefs.SetInt("HasClickedOnForm", m_currentPlayerPrefHasClickedOnForm);
    }
}