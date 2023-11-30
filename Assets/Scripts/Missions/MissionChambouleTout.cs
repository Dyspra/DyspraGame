using System.Collections;
using System;
using UnityEngine;
using TMPro;

public class MissionChambouleTout : MonoBehaviour
{
    private enum ChambouleToutState
    {
        TRAVELING,
        TUTORIAL,
        LAUNCH_GAME,
        INGAME,
        FINISH,
        NONE
    }
    private int score = 0;
    private bool isPowerUp = false;
    [SerializeField] private int shotScore = 10;
    [SerializeField] private int powerUpDisableCanScore = 1;
    [SerializeField] private int powerUpEnableCanScore = 2;
    [SerializeField] private double timer = 70.0f;
    [SerializeField] private ChambouleToutState missionState = ChambouleToutState.TRAVELING;

    // Camera traveling
    [SerializeField] private GameObject _camToMove;
    [SerializeField] private Transform startCameraPos;
    private Vector3 finalCameraPos = new Vector3(0.0f, 0.3f, -0.55f);
    [SerializeField] private float introCutsceneTime;
    private Vector3 velocity;
    public float speed;

    // Sound
    [SerializeField] private AudioSource _ambientAudioSource;
    [SerializeField] private AudioSource _MusicAudioSource;
    [SerializeField] private AudioSource _SFXAudioSource;
    [SerializeField] private AudioClip launchStartGameSound;
    [SerializeField] private AudioClip finishGameSound;
    [SerializeField] private AudioClip clapSound;

    // UI ------------------------------------------
    // Text
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text timerStartGameText;
    [SerializeField] private TMP_Text finalScoreText;
    // UI Toggling
    [SerializeField] private GameObject _scoreGO;
    [SerializeField] private GameObject _timerGO;
    [SerializeField] private GameObject _bandeauGO;
    [SerializeField] private GameObject _finalScoreGO;

    void Start()
    {
        _camToMove.transform.position = startCameraPos.position;
        _camToMove.transform.rotation = startCameraPos.rotation;
        StartCoroutine(MoveCameraBeforeTuto());
        timerStartGameText.text = "";
        UpdateScoreText();
    }
    // Update is called once per frame
    void Update()
    {
        switch (missionState)
        {
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
            _finalScoreGO.SetActive(true);
            missionState = ChambouleToutState.FINISH;
            _SFXAudioSource.clip = clapSound;
            _SFXAudioSource.Play();
            _MusicAudioSource.Stop();
            _MusicAudioSource.loop = false;
            _MusicAudioSource.clip = finishGameSound;
            _MusicAudioSource.Play();
            timerText.text = "0:00";
            finalScoreText.text = score.ToString();
        }
    }

    public IEnumerator LaunchGameTimer()
    {
        float timer = 3.0f;
        float time = timer;
        int actuelSecond = 3;
        _SFXAudioSource.clip = clapSound;
        _SFXAudioSource.Play();
        timerStartGameText.text = "Prêt ?";
        yield return new WaitForSeconds(3.0f);
        timerStartGameText.text = actuelSecond.ToString();
        _SFXAudioSource.clip = launchStartGameSound;
        _SFXAudioSource.Play();
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

    private IEnumerator MoveCameraBeforeTuto()
    {
        Vector3 posToGo = new Vector3(0f, 0.3f, -0.55f);
        float timer = introCutsceneTime;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            //_camToMove.transform.position = Vector3.Lerp(_camToMove.transform.position, posToGo, Time.deltaTime * speed);
            _camToMove.transform.position = Vector3.SmoothDamp(_camToMove.transform.position, posToGo, ref velocity, introCutsceneTime / 6);
            _camToMove.transform.rotation = Quaternion.Lerp(_camToMove.transform.rotation, Quaternion.identity, Time.deltaTime * speed);
            yield return null;
        }
        Destroy(_bandeauGO);
        _timerGO.SetActive(true);
        _scoreGO.SetActive(true);
        _camToMove.transform.localPosition = finalCameraPos;
        _camToMove.transform.localRotation = Quaternion.identity;
        missionState = ChambouleToutState.TUTORIAL;
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
        scoreText.text = score.ToString();
    }
    #endregion
}
