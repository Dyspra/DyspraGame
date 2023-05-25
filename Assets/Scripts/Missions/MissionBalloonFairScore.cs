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
    [SerializeField] private SpawnerBehaviour spawner1;

    private Vector3[] transPoint;
    private Vector3 velocity;
    private float time = 0.0f;
    private int currentPoint;
    private bool isTriggered = false;
    private bool isTimerOn = false;

    [SerializeField] private int actualStep = 1;
    [SerializeField] private int nbrToTriggerStep2 = 10;
    [SerializeField] private int nbrToTriggerStep3 = 20;
    [SerializeField] private int nbrToTriggerEnd = 30;
    [SerializeField] private float timeToWaitBeforeTrigger = 5;
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
        score = 0;
        scoreTxt.text = score.ToString();
        StartCoroutine(WaitBeforeMove());
        score = nbrToTriggerStep2;
        isTriggered = true;
        MissionEventComplete();
        Debug.Log(1);
    }

    public void Step2Validate()
    {
        if (canTriggerNext == false)
            return;
        actualStep++;
        StartCoroutine(WaitBeforeMove());
        score = nbrToTriggerStep3;
        isTriggered = true;
        MissionEventComplete();
    }

    public void Step3ValidateEndGame()
    {
        if (canTriggerNext == false)
            return;
        actualStep++;
        StartCoroutine(WaitBeforeMove());
        score = nbrToTriggerEnd;
        isTriggered = true;
        isTimerOn = false;
        MissionEventComplete();
        completeTxt.SetActive(true);
    }

    public void GetBalloon(int scoreToAdd)
    {
        if (actualStep >= 4)
            return;
        score += scoreToAdd;
        scoreTxt.text = score.ToString();
        switch (actualStep)
        {
            case 1:
                if (score >= nbrToTriggerStep2 && canTriggerNext == true)
                {
                    LaunchNextEvent();
                }
                break;
            case 2:
                if (score >= nbrToTriggerStep3 && canTriggerNext == true)
                {
                    LaunchNextEvent();
                }
                break;
            case 3:
                if (score >= nbrToTriggerEnd && canTriggerNext == true)
                {
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
        time += Time.deltaTime;

        sec = (int)(time % 60);
        min = (int)((time / 60) % 60);
        timeTxt.text = min.ToString("00") + ":" + sec.ToString("00");
    }

    private IEnumerator WaitBeforeMove()
    {
        canTriggerNext = false;
        StopAllSpawners();
        yield return new WaitForSeconds(timeToWaitBeforeTrigger);
        TriggerSpawners();
        isTimerOn = true;
        canTriggerNext = true;
    }

    private void TriggerSpawners()
    {
        if (actualStep == 2)
        {
            spawner1.enabled = true;
        }
    }

    private void StopAllSpawners()
    {
        spawner1.enabled = false;
    }
}
