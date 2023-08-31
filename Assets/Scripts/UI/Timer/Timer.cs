using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TimeSelect timeselected;
    public GameObject tsui;
    public float maxTime = 60f;
    public float currentTime;
    private bool isRunning;
    public Text timerText;
    public GameObject ResultUI;
    public ScoreJellyfish score;

    private void Start()
    {
        maxTime = timeselected.selectedTime;
        currentTime = maxTime;
    }
    private void OnEnable()
    {
        ResumeTimer();
        maxTime = timeselected.selectedTime;
        currentTime = maxTime;
        tsui.SetActive(false);
    }
    private void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                ResultUI.SetActive(true);
                score.UpdateScoreResultUI();
            } else {
                UpdateTimerUI();
            }
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