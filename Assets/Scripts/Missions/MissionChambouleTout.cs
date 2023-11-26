using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class MissionChambouleTout : MonoBehaviour
{
    private enum ChambouleToutState
    {
        TUTORIAL,
        LAUNCH_GAME,
        INGAME,
        FINISH,
        NONE
    }
    public Text scoreText;
    public Text timerText;
    public Text timerStartGameText;
    private int score = 0;
    private bool isPowerUp = false;
    [SerializeField] private int shotScore = 10;
    [SerializeField] private int powerUpDisableCanScore = 1;
    [SerializeField] private int powerUpEnableCanScore = 2;
    [SerializeField] private double timer = 70.0f;
    [SerializeField] private ChambouleToutState missionState = ChambouleToutState.TUTORIAL;


    // Sound
    [SerializeField] private AudioClip launchStartGameSound;
    [SerializeField] private AudioClip finishGameSound;
    [SerializeField] private AudioClip clapSound;
    [SerializeField] private AudioSource audioSource; // Source

    void Start()
    {
        timerStartGameText.text = "";
        UpdateScoreText();
    }
    // Update is called once per frame
    void Update()
    {
        switch (missionState)
        {
            case ChambouleToutState.TUTORIAL:
                break;
            case ChambouleToutState.LAUNCH_GAME:
                StartCoroutine(LaunchGameTimer());
                missionState = ChambouleToutState.NONE;
                break;
            case ChambouleToutState.INGAME:
                UpdateInGame();
                break;
            case ChambouleToutState.FINISH:
                break;
            default:
                break;
        }
    }


    private void UpdateInGame()
    {
        timer -= Time.deltaTime;
        timerText.text = TimeSpan.FromSeconds(timer).ToString(@"m\:ss");
        if (timer <= 0f)
        {
            missionState = ChambouleToutState.FINISH;
            audioSource.clip = clapSound;
            audioSource.Play();
            timerText.text = "0:00";
        }
    }

    public IEnumerator LaunchGameTimer()
    {
        float timer = 3.0f;
        float time = timer;
        int actuelSecond = 3;
        audioSource.clip = clapSound;
        audioSource.Play();
        timerStartGameText.text = "Prêt ?!";
        yield return new WaitForSeconds(3.0f);
        timerStartGameText.text = actuelSecond.ToString();
        audioSource.clip = launchStartGameSound;
        audioSource.Play();
        while (time > 0)
        {
            time -= Time.deltaTime;
            if (time < actuelSecond - 1)
            {
                actuelSecond--;
                timerStartGameText.text = actuelSecond.ToString();
            }
            yield return null;
        }
        timerStartGameText.text = "GO !";
        yield return new WaitForSeconds(1.0f);
        timerStartGameText.text = "";
        missionState = ChambouleToutState.INGAME;
        yield return null;
    }

    #region Scoring
    public void ShotThunderBolt()
    {
        switch (missionState)
        {
            case ChambouleToutState.TUTORIAL:
                missionState = ChambouleToutState.LAUNCH_GAME;
                break;
            case ChambouleToutState.INGAME:
                IncreaseScore(shotScore);
                break;
            case ChambouleToutState.FINISH:
                break;
            default:
                break;
        }
    }

    public void HitACan()
    {
        if (missionState != ChambouleToutState.INGAME)
            return;

        if (isPowerUp == false)
        {
            IncreaseScore(powerUpDisableCanScore);
        }
        else
        {
            IncreaseScore(powerUpEnableCanScore);
        }
    }

    public void IncreaseScore(int newScore)
    {
        score += newScore;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    #endregion
}
