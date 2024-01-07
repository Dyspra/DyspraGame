using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerEx4 : MonoBehaviour
{
    public float maxTime = 150f;
    public float currentTime;
    public TMP_Text scoreTextResult;
    private bool isRunning;
    public ScoreExercice4 ScoreUI;
    public TextMeshProUGUI timerText;
    public GameObject ResultUI;

    private void Start()
    {
        currentTime = maxTime;
    }
    private void OnEnable()
    {
        ResumeTimer();
    }
    private void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                ResultUI.SetActive(true);
                // cursor.SetActive(true);
                // cursor.GetComponent<CursorManager>().SearchLastCanvas();
                UpdateScoreResultUI();
            } else {
                UpdateTimerUI();
            }
        }
    }

    public void UpdateScoreResultUI()
    {
        if (scoreTextResult != null)
        {
            scoreTextResult.text = ScoreUI.score.ToString();
        }
    }
    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (currentTime <= 20f) {
            timerText.color = Color.red;
        }
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }
}