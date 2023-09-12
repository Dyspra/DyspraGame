using UnityEngine;

public class QuestionnaireGoogle : MonoBehaviour
{

    [SerializeField] private GameObject m_mainCanvas;
    [SerializeField] private GameObject m_windowTutoForm;
    [SerializeField] private GameObject m_windowFormAsked;

    private int m_currentPlayerPrefHasClickedOnForm = 0;

    private void Start()
    {
        m_currentPlayerPrefHasClickedOnForm = PlayerPrefs.GetInt("HasClickedOnForm");
        Debug.Log("Current click = " + m_currentPlayerPrefHasClickedOnForm);

        if (m_currentPlayerPrefHasClickedOnForm == 1)
        {
            m_mainCanvas.SetActive(false);
            m_windowFormAsked.SetActive(true);
            m_windowTutoForm.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            m_currentPlayerPrefHasClickedOnForm = 0;
            PlayerPrefs.SetInt("HasClickedOnForm", m_currentPlayerPrefHasClickedOnForm);
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
        m_mainCanvas.SetActive(true);
        m_windowFormAsked.SetActive(false);
    }

    public void OnClickedOnReturnButton()
    {
        if (m_mainCanvas == null || m_windowTutoForm == null)
            return;

        if (m_currentPlayerPrefHasClickedOnForm != 2)
        {
            m_windowTutoForm.SetActive(true);
            m_mainCanvas.SetActive(false);
            m_windowFormAsked.SetActive(false);
            PlayerPrefs.SetInt("HasClickedOnForm", 0);
        } else
        {
            m_mainCanvas.SetActive(true);
        }
    }

    public void OnClickedOnGoToFormWindow()
    {
        if (m_currentPlayerPrefHasClickedOnForm != 2)
        {
            PlayerPrefs.SetInt("HasClickedOnForm", 0);
        }
    }
}
