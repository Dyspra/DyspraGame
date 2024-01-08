using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TimeSelect timeselected;
    public GameObject tsui;
    public GameObject cursor;
    public float maxTime = 60f;
    public float currentTime;
    private bool isRunning;
    public TextMeshProUGUI timerText;
    public GameObject ResultUI;
    public ScoreJellyfish score;
    public AudioSource bgm;

	private bool isInitiallyActive;

	private void Awake()
	{
		GameStateManager.Instance.onGameStateChange += OnGameStateChanged;
	}

	private void OnDestroy()
	{
		GameStateManager.Instance.onGameStateChange -= OnGameStateChanged;
	}

	private void Start()
    {
        maxTime = timeselected.selectedTime;
        currentTime = maxTime;
    }
    private void OnEnable()
    {
        ResumeTimer();
    }
    public void DeactivateObjects(string keyword)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains(keyword))
            {
                obj.SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                DeactivateObjects("Jellyfish");
                DeactivateObjects("Root");
                ResultUI.SetActive(true);
                cursor.SetActive(true);
                cursor.GetComponent<CursorManager>().SearchLastCanvas();
                score.StopScoreUpdate();
                score.UpdateScoreResultUI();
                bgm.enabled = false;
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

	private void OnGameStateChanged(GameState newGameState)
	{
		if (newGameState == GameState.Gameplay && isInitiallyActive == true)
		{
			enabled = true;
		}
		else if (newGameState == GameState.Paused)
		{
			isInitiallyActive = enabled;
			enabled = false;
		}
	}
}