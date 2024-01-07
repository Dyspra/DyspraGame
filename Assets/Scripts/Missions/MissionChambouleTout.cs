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
    [SerializeField] private AudioClip music;

    // UI ------------------------------------------
    // Text 
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text timerStartGameText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalScoreTextAvatar;
    // UI Toggling
    [SerializeField] private GameObject _scoreGO;
    [SerializeField] private GameObject _timerGO;
    [SerializeField] private GameObject _bandeauGO;
    [SerializeField] private GameObject _finalScoreGO;
    [SerializeField] private GameObject _tutoGo;

    // Difficulté
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _currentPlayerPos = Vector3.zero;
    [SerializeField] private Vector3 _centerPos = Vector3.zero;
    [SerializeField] private Vector3 _leftPos = Vector3.zero;
    [SerializeField] private Vector3 _rightPos = Vector3.zero;
    [SerializeField] private float _deltaPos = 5.0f;

    public bool _canMove = false;
    public bool _isMoving = true;
    private bool _isScoreHighEnoughFor3Pos = false;
    [HideInInspector] public int _nbrOfLeftBolt = 0;
    [HideInInspector] public int _nbrOfRightBolt = 0;
    [SerializeField] private float _difficultyFactor = 0.5f;
    private int _currentDifficulty = 0;
    private int innerScoreToDifficulty = 0;
    [SerializeField] private int _minScoreToMove = 100;
    [SerializeField] private float _minMoveDelay = 7.0f;
    [SerializeField] private float _maxMoveDelay = 8.0f;
    [SerializeField] private float nextTimeToWait = 7.0f;
    int currentPosIndex = 2; // 1 = left && 2 = center && 3 = right

    void Start()
    {
        _camToMove.transform.position = startCameraPos.position;
        _camToMove.transform.rotation = startCameraPos.rotation;
        StartCoroutine(MoveCameraBeforeTuto());
        timerStartGameText.text = "";
        UpdateScoreText();
        _leftPos = new Vector3(_centerPos.x + _deltaPos, _centerPos.y, _centerPos.z);
        _rightPos = new Vector3(_centerPos.x - _deltaPos, _centerPos.y, _centerPos.z);
        // Mettre le truc pour mettre faire pop le tuto ecrit
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
                if (_isMoving == false)
                    nextTimeToWait -= Time.deltaTime;
                if (_isMoving == false && nextTimeToWait <= 0.0f)
                {
                    StartCoroutine(MovePlayer());
                }
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
            finalScoreTextAvatar.text = score.ToString();
            BDDInteractor.Instance.AddHistory("3", score);
        }
    }

    public IEnumerator LaunchGameTimer()
    {
        float timer = 3.0f;
        float time = timer;
        int actuelSecond = 3;
        _SFXAudioSource.clip = clapSound;
        _SFXAudioSource.Play();
        _tutoGo.SetActive(false);
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
        _MusicAudioSource.Play();
        yield return null;
    }

    private IEnumerator MoveCameraBeforeTuto()
    {
        Vector3 posToGo = new Vector3(0f, 0.3f, -0.55f);
        float timer = introCutsceneTime;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            _camToMove.transform.position = Vector3.SmoothDamp(_camToMove.transform.position, posToGo, ref velocity, introCutsceneTime / 6);
            _camToMove.transform.rotation = Quaternion.Lerp(_camToMove.transform.rotation, Quaternion.identity, Time.deltaTime * speed);
            yield return null;
        }
        Destroy(_bandeauGO);

        _tutoGo.SetActive(true);
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
        innerScoreToDifficulty += newScore;
        score += newScore;
        if (innerScoreToDifficulty >= _minScoreToMove)
            IncreaseDifficulty();
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
    #endregion

    #region Difficulty
    void IncreaseDifficulty()
    {
        if (_currentDifficulty >= 10)
            return;
        if (_canMove == false)
        {
            _canMove = true;
            StartCoroutine(MovePlayer());
            return;
        }
        _currentDifficulty++;
        _minMoveDelay -= _difficultyFactor; // delay - 0.5
        _maxMoveDelay -= _difficultyFactor;
        innerScoreToDifficulty -= _minScoreToMove; // innerscore - 100
    }

    private IEnumerator MovePlayer()
    {
        Vector3 nextPos = GetNextPos();
        _isMoving = true;
        float timer = 1.9f;
        float time = 0.0f;
        nextTimeToWait = UnityEngine.Random.Range(_minMoveDelay, _maxMoveDelay);
        _currentPlayerPos = _player.transform.position;
        while (time < timer)
        {
            time += Time.deltaTime;
            float percentage = time / timer;
            _player.transform.position = Vector3.Lerp(_currentPlayerPos, nextPos, percentage);
            yield return null;
        }
        _player.transform.position = nextPos;
        _currentPlayerPos = nextPos;
        _isMoving = false;
        yield return null;
    }

    Vector3 GetNextPos()
    {
        if (_currentDifficulty >= 3 || (_nbrOfRightBolt > 0 && _nbrOfLeftBolt > 0))
        {
            int randomNumber = 1;
            do
            {
                randomNumber = UnityEngine.Random.Range(1, 4); // Génère un nombre aléatoire entre 1 et 3 inclus
            } while (randomNumber == currentPosIndex);
            currentPosIndex = randomNumber;
            switch (currentPosIndex)
            {
                case 1:
                    return _leftPos;
                    break;
                case 2:
                    return _centerPos;
                    break;
                case 3:
                    return _rightPos;
                    break;
                default:
                    return Vector3.zero;
                    break;
            }
        }
        if (_nbrOfLeftBolt > _nbrOfRightBolt)
        {
            if (currentPosIndex == 2)
            {
                currentPosIndex = 3;
                return _leftPos;
            }
        }
        else
        {
            if (currentPosIndex == 2)
            {
                currentPosIndex = 1;
                return _rightPos;
            }
        }
        currentPosIndex = 2;
        return _centerPos;
    }
    #endregion
}