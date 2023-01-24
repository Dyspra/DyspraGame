using UnityEngine;
using TMPro;


// This mission triggers the player movement and update the UI
public class MissionBalloonFair : Dyspra.AbstractMission
{
    [SerializeField] private GameObject wagon;
    [SerializeField] private float distanceMinToChange;
    [SerializeField] private float[] timesToMove;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private int score;
    [SerializeField] private TMP_Text timeTxt;
    [SerializeField] private TMP_Text scoreTxt;

    private Vector3[] transPoint;
    private Vector3 velocity;
    private float time = 0.0f;
    private int currentPoint;
    private bool isTriggered = false;
    private bool isTimerOn = true;

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
            Debug.Log("Arrived");
            return;
        }
        wagon.transform.position = Vector3.SmoothDamp(wagon.transform.position, transPoint[currentPoint], ref velocity, timesToMove[currentPoint]);
        wagon.transform.rotation = Quaternion.Lerp(wagon.transform.rotation, waypoints[currentPoint].transform.rotation, Time.deltaTime * timesToMove[currentPoint]);
    }

    public void Step1Validate()
    {
        isTriggered = true;
        MissionEventComplete();
    }

    public void Step2Validate()
    {
        isTriggered = true;
        MissionEventComplete();
    }

    public void Step3ValidateEndGame()
    {
        isTriggered = true;
        isTimerOn = false;
        MissionEventComplete();
    }

    public void GetBalloon()
    {
        score++;
        scoreTxt.text = score.ToString();
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
}
