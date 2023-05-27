using UnityEngine;

public class MoveCubeGhetto : MonoBehaviour
{
    public GameObject cubeGhetto;
    public int step = 0;
    public Transform[] pos = new Transform[6];
    public float timerTrigger = 5.0f;

    private float timer = 0.0f;
    private bool isTimerOn = false;

    public TriggerBalloonFair tbf;

    private void Start()
    {
        MoveCube();
    }

    void Update()
    {
        if (isTimerOn == true)
        {
            timer += Time.deltaTime;
            if (timer >= timerTrigger)
            {
                isTimerOn = false;
                MoveCube();
            }
        }
    }

    public void MoveCube()
    {
        if (isTimerOn == true)
            return;
        cubeGhetto.transform.position = pos[step].position;
        if (step < pos.Length - 1)
            step++;
    }

    public void LaunchTimer()
    {
        if (isTimerOn == true)
            return;
        timer = 0.0f;
        isTimerOn = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            tbf.GetBalloon();
            MoveCube();
            LaunchTimer();
            Debug.Log("TOUCH");
        }
    }
}
