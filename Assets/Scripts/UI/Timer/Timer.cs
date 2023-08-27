using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float maxTime = 60f;
    private float currentTime;
    private bool isRunning = true;

    public Text timerText;
    public GameObject ResultUI;
    public ScoreJellyfish score;

    private void Start()
    {
        currentTime = maxTime;
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
        if (currentTime <= 10f) {
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