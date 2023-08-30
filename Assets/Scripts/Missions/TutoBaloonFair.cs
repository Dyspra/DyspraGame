using System.Collections;
using UnityEngine;

public class TutoBaloonFair : MonoBehaviour
{
    [SerializeField] private MissionBalloonFairScore Mission;

    [SerializeField] private GameObject TempsUI;
    [SerializeField] private GameObject ScoreUI;
    [SerializeField] private GameObject TutoCalibLeft;
    [SerializeField] private GameObject TutoCalibRight;
    [SerializeField] private GameObject TutoAspireRight;
    [SerializeField] private GameObject TutoAspireLeft;
    [SerializeField] private GameObject BalloonWalls;
    [SerializeField] private GameObject LastBalloon;
    [SerializeField] private GameObject LeftBalloon;
    [SerializeField] private GameObject MiddleBalloon;
    [SerializeField] private GameObject UIFoireAuxBallons;
    [SerializeField] private float introCutsceneTime;
    private bool canStartTuto = false;
    [SerializeField] private GameObject _camToMove;
    private Vector3 finalCameraPos = new Vector3(0.0f, 0.3f, -0.55f);
    [SerializeField] private Transform startCameraPos;
    private Vector3 velocity;
    public float speed;

    [SerializeField] private int score = 0;
    private bool hasShowFirstTuto = false;

    private void Start()
    {
        _camToMove.transform.position = startCameraPos.position;
        _camToMove.transform.rotation = startCameraPos.rotation;
        StartCoroutine(MoveCameraBeforeTuto());
    }

    void Update()
    {
        if (HandTrackingManager.Instance.HandTracking.LeftHandLandmarks.Length < 21 || HandTrackingManager.Instance.HandTracking.RightHandLandmarks.Length < 21 || canStartTuto == false)
            return;
        else
        {
            if (hasShowFirstTuto == false)
            {
                hasShowFirstTuto = true;
                TutoCalibLeft.SetActive(true);
            }
        }
        score = Mission.GetScore();

        if (LastBalloon == null)
        {
            Destroy(TutoCalibLeft);
            Destroy(TutoCalibRight);
            Destroy(TutoAspireLeft);
            Destroy(TutoAspireRight);
            Destroy(this.gameObject);
        }

        if (LeftBalloon == null && MiddleBalloon == null)
        {
            Destroy(BalloonWalls);
            TutoAspireRight.SetActive(false);
            TutoAspireLeft.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TutoCalibLeft.SetActive(false);
            TutoCalibRight.SetActive(true);
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TutoAspireRight.SetActive(true);
            TutoCalibRight.SetActive(false);
        }
    }

    private IEnumerator MoveCameraBeforeTuto()
    {
        Vector3 posToGo = new Vector3(16.06f, 71.17001f, -127.482f);
        float timer = introCutsceneTime;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            //_camToMove.transform.position = Vector3.Lerp(_camToMove.transform.position, posToGo, Time.deltaTime * speed);
            _camToMove.transform.position = Vector3.SmoothDamp(_camToMove.transform.position, posToGo, ref velocity, introCutsceneTime /6 );
            _camToMove.transform.rotation = Quaternion.Lerp(_camToMove.transform.rotation, Quaternion.identity, Time.deltaTime * speed);
            yield return null;
        }
        Destroy(UIFoireAuxBallons);
        TempsUI.SetActive(true);
        ScoreUI.SetActive(true);
        _camToMove.transform.localPosition = finalCameraPos;
        _camToMove.transform.localRotation = Quaternion.identity;
        canStartTuto = true;
    }
}
