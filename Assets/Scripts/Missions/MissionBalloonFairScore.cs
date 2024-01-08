using System.Collections;
using UnityEngine;
using TMPro;
using Constants;

// This mission triggers the player movement and update the UI
public class MissionBalloonFairScore : Dyspra.AbstractMission
{
    [SerializeField] private GameObject wagon;
    [SerializeField] private float distanceMinToChange;
    [SerializeField] private float[] timesToMove;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private int score;
    [SerializeField] private TMP_Text timeTxt;
    [SerializeField] private TMP_Text scoreTxt;
    [SerializeField] private TMP_Text countDownUi;
    [SerializeField] private GameObject completeMenu;
	[SerializeField] private GameObject cursor;
	[SerializeField] private TMP_Text finalScoreText;
	[SerializeField] private TMP_Text finalScoreTextAvatar;
    [SerializeField] private AudioClip _ambient;
    [SerializeField] private AudioClip _countdown;
    [SerializeField] private AudioClip _endingGameJingle1;
    [SerializeField] private AudioClip _endingGameJingle2;
    [SerializeField] private AudioClip _endingStepJingle;
    [SerializeField] private AudioClip _music;

    private Vector3[] transPoint;
    private Vector3 velocity;
    private float time = 0.0f;
    private int currentPoint;
    private bool isTriggered = false;
    private bool isTimerOn = false;

    [SerializeField] private int actualStep = 1;
    [SerializeField] private int scoreToTriggerStep2 = 3;
    [SerializeField] private int timerToTriggerStep3 = 5;
    [SerializeField] private int timerToTriggerStep4 = 5;
    [SerializeField] private int timerToTriggerStep5 = 5;
    [SerializeField] private int timerToTriggerEnd = 5;
    [SerializeField] private float timeToWaitBeforeTrigger = 5;
    [SerializeField] private SpawnerBehaviour spawner1;
    [SerializeField] private SpawnerBehaviour spawner2;
    [SerializeField] private SpawnerBehaviour spawner3;
    [SerializeField] private SpawnerBehaviour spawner4;
    [SerializeField] private SpawnerBehaviour spawner5;
    [SerializeField] private SpawnerBehaviour spawner6;
    [SerializeField] private AudioSource _ambientAudioSource;
    [SerializeField] private AudioSource _MusicAudioSource;
    [SerializeField] private AudioSource _SFXAudioSource;
    private float timer;
    private bool canTriggerNext = true;

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
        if (waypoints.Length == 0 || timesToMove.Length == 0)
            return;
        scoreTxt.text = "0";
        transPoint = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            transPoint[i] = new Vector3(waypoints[i].transform.position.x, waypoints[i].transform.position.y, waypoints[i].transform.position.z);
        }
        currentPoint = 0;
        _ambientAudioSource.clip = _ambient;
        _ambientAudioSource.Play();
        _SFXAudioSource.clip = _endingGameJingle1;
    }

    private void Update()
    {
        UpdateTimer();
        if (isTriggered == false || waypoints.Length == 0 || timesToMove.Length == 0)
            return;
        if (Vector3.Distance(transPoint[currentPoint], wagon.transform.position) < distanceMinToChange)
        {
            currentPoint++;
            isTriggered = false;
            return;
        }
        wagon.transform.position = Vector3.SmoothDamp(wagon.transform.position, transPoint[currentPoint], ref velocity, timesToMove[currentPoint]);
        wagon.transform.rotation = Quaternion.Lerp(wagon.transform.rotation, waypoints[currentPoint].transform.rotation, Time.deltaTime * timesToMove[currentPoint]);

    }

    public void Step1Validate()
    {
        if (canTriggerNext == false)
            return;
        _SFXAudioSource.Play();
        actualStep++;
        time = timerToTriggerStep3;
        StartCoroutine(WaitBeforeMove());
        score = 0;
        scoreTxt.text = score.ToString();
        isTriggered = true;
        MissionEventComplete();
        Debug.Log(actualStep);
        AnalyticsManager.Instance.LogEx1_StepFinished(1, score);
    }

    public void Step2Validate()
    {
        if (canTriggerNext == false)
            return;
        _SFXAudioSource.Play();
        actualStep++;
        time = timerToTriggerStep4;
        StartCoroutine(WaitBeforeMove());
        isTriggered = true;
        MissionEventComplete();
        Debug.Log(actualStep);
        AnalyticsManager.Instance.LogEx1_StepFinished(2, score);
    }

    public void Step3Validate()
    {
        if (canTriggerNext == false)
            return;
        _SFXAudioSource.Play();
        actualStep++;
        time = timerToTriggerStep5;
        StartCoroutine(WaitBeforeMove());
        isTriggered = true;
        MissionEventComplete();
        Debug.Log(actualStep);
        AnalyticsManager.Instance.LogEx1_StepFinished(3, score);
    }

    public void Step4Validate()
    {
        if (canTriggerNext == false)
            return;
        _SFXAudioSource.Play();
        actualStep++;
        time = timerToTriggerEnd;
        StartCoroutine(WaitBeforeMove());
        isTriggered = true;
        MissionEventComplete();
        Debug.Log(actualStep);
        AnalyticsManager.Instance.LogEx1_StepFinished(4, score);
    }

    public void ValidateEndGame()
    {
        if (canTriggerNext == false)
            return;
        _SFXAudioSource.clip = _endingGameJingle2;
        _MusicAudioSource.Stop();
        _SFXAudioSource.Play();
        actualStep++;
        StartCoroutine(WaitBeforeMove());
        isTriggered = true;
        isTimerOn = false;
        MissionEventComplete();
        completeMenu.SetActive(true);
        cursor.SetActive(true);
        cursor.GetComponent<CursorManager>().SearchLastCanvas();
        finalScoreText.text = score.ToString();
        finalScoreTextAvatar.text = score.ToString();

        if (timerToTriggerStep3 < timerToTriggerStep4)
            BDDInteractor.Instance.AddHistory("1", score); // Score pour BalloonFair difficile
        else
            BDDInteractor.Instance.AddHistory("4", score); // Score pour BalloonFair facile

        if (PlayerPrefs.GetInt("HasClickedOnForm") != 2)
            PlayerPrefs.SetInt("HasClickedOnForm", 1);
        Debug.Log(actualStep);
        AnalyticsManager.Instance.LogEx1_StepFinished(5, score);
        AnalyticsManager.Instance.LogExerciseStop("1", score, ExerciseConstants.E_QuitReason.Complete);
    }

    public void GetBalloon(int scoreToAdd)
    {
        if (actualStep >= 6)
            return;
        if (score + scoreToAdd <=0)
            score = 0;
        else
            score += scoreToAdd;
        scoreTxt.text = score.ToString();
        switch (actualStep)
        {
            case 1:
                if (score >= scoreToTriggerStep2 && canTriggerNext == true)
                {
                    isTimerOn = false;
                    LaunchNextEvent();
                }
                break;
            case 2:
                if (time <= 0 && canTriggerNext == true)
                {
                    isTimerOn = false;
                    LaunchNextEvent();
                }
                break;
            case 3:
                if (time <= 0 && canTriggerNext == true)
                {
                    isTimerOn = false;
                    LaunchNextEvent();
                }
                break;
            case 4:
                if (time <= 0 && canTriggerNext == true)
                {
                    isTimerOn = false;
                    LaunchNextEvent();
                }
                break;
            case 5:
                if (time <= 0 && canTriggerNext == true)
                {
                    isTimerOn = false;
                    LaunchNextEvent();
                }
                break;
            default:
                break;
        }
    }

    public void UpdateTimer()   
    {
        if (isTimerOn == false)
            return;
        float sec, min;
        time -= Time.deltaTime;

        sec = (int)(time % 60);
        min = (int)((time / 60) % 60);
        timeTxt.text = min.ToString("00") + ":" + sec.ToString("00");
        GetBalloon(0);
    }

    private IEnumerator WaitBeforeMove()
    {
        canTriggerNext = false;
        StopAllSpawners();
        yield return new WaitForSeconds(timeToWaitBeforeTrigger);
        if (actualStep != 6)
        {
            int toPass = 3;
            float timer = 4.0f;
            bool canPass = true;
            countDownUi.gameObject.SetActive(true);
            countDownUi.text = toPass.ToString();
            _SFXAudioSource.clip = _countdown;
            _SFXAudioSource.Play();
            
            countDownUi.gameObject.SetActive(true);
            while (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                if (timer <= toPass)
                {
                    if(toPass <= 1)
                    {
                        countDownUi.text = "GO!";
                    }
                    else
                    {
                        toPass--;
                        countDownUi.text = toPass.ToString();
                    }
                }
                yield return null;
            }
            countDownUi.gameObject.SetActive(false);
            _SFXAudioSource.clip = _endingGameJingle1;
        }
        TriggerSpawners();
        isTimerOn = false;
        if (actualStep < 6)
            isTimerOn = true;
        if (actualStep == 2)
        {
            score = 0;
            scoreTxt.text = score.ToString();
        }
        canTriggerNext = true;
    }

    private void TriggerSpawners()
    {
        if (actualStep == 2)
        {
            spawner1.enabled = true;
        }
        else if (actualStep == 3)
        {
            spawner2.enabled = true;
            spawner3.enabled = true;
        }
        else if (actualStep == 4)
        {
            spawner4.enabled = true;
        }
        else if (actualStep == 5)
        {
            spawner5.enabled = true;
            spawner6.enabled = true;
        }
    }

    private void StopAllSpawners()
    {
        spawner1.enabled = false;
        spawner2.enabled = false;
        spawner3.enabled = false;
        spawner4.enabled = false;
        spawner5.enabled = false;
        spawner6.enabled = false;
    }

    public int GetScore()
    {
        return score;
    }

    public void BeforeQuit()
    {
        AnalyticsManager.Instance.LogExerciseStop("1", score, ExerciseConstants.E_QuitReason.Quit);
    }

	private void OnGameStateChanged(GameState newGameState)
	{
		enabled = newGameState == GameState.Gameplay;
	}
}
