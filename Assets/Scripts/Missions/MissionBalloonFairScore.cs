using System.Collections;
using UnityEngine;
using TMPro;

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
    [SerializeField] private GameObject completeTxt;
    [SerializeField] private GameObject completeMenu;
    [SerializeField] private TMP_Text finalScoreText;

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
    private float timer;
    private bool canTriggerNext = true;

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
        actualStep++;
        time = timerToTriggerStep3;
        StartCoroutine(WaitBeforeMove());
        score = 0;
        scoreTxt.text = score.ToString();
        isTriggered = true;
        MissionEventComplete();
        Debug.Log(actualStep);
    }

    public void Step2Validate()
    {
        if (canTriggerNext == false)
            return;
        actualStep++;
        time = timerToTriggerStep4;
        StartCoroutine(WaitBeforeMove());
        isTriggered = true;
        MissionEventComplete();
        Debug.Log(actualStep);
    }

    public void Step3Validate()
    {
        if (canTriggerNext == false)
            return;
        actualStep++;
        time = timerToTriggerStep5;
        StartCoroutine(WaitBeforeMove());
        isTriggered = true;
        MissionEventComplete();
        Debug.Log(actualStep);
    }

    public void Step4Validate()
    {
        if (canTriggerNext == false)
            return;
        actualStep++;
        time = timerToTriggerEnd;
        StartCoroutine(WaitBeforeMove());
        isTriggered = true;
        MissionEventComplete();
        Debug.Log(actualStep);
    }

    public void ValidateEndGame()
    {
        if (canTriggerNext == false)
            return;
        actualStep++;
        StartCoroutine(WaitBeforeMove());
        isTriggered = true;
        isTimerOn = false;
        MissionEventComplete();
        completeTxt.SetActive(true);
        completeMenu.SetActive(true);
        finalScoreText.text = score.ToString();
        BDDInteractor.Instance.AddHistory("1", score);
        if (PlayerPrefs.GetInt("HasClickedOnForm") != 2)
            PlayerPrefs.SetInt("HasClickedOnForm", 1);
        Debug.Log(actualStep);
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
}
