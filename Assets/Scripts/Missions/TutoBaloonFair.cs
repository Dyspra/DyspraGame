using UnityEngine;

public class TutoBaloonFair : MonoBehaviour
{
    [SerializeField] private MissionBalloonFairScore Mission;

    [SerializeField] private GameObject TutoCalibLeft;
    [SerializeField] private GameObject TutoCalibRight;
    [SerializeField] private GameObject TutoAspireRight;
    [SerializeField] private GameObject TutoAspireLeft;
    [SerializeField] private GameObject BalloonWalls;
    [SerializeField] private GameObject LastBalloon;
    [SerializeField] private GameObject LeftBalloon;
    [SerializeField] private GameObject MiddleBalloon;

    [SerializeField] private int score = 0;
    private bool hasShowFirstTuto = false;

    void Start()
    {
    }

    void Update()
    {
        if (HandTrackingManager.Instance.HandTracking.LeftHandLandmarks.Length < 21 || HandTrackingManager.Instance.HandTracking.RightHandLandmarks.Length < 21)
            return;
        else
        {
            if (hasShowFirstTuto == false)
            {
                hasShowFirstTuto = true;
                TutoCalibLeft.SetActive(true);
                AnalyticsManager.Instance.LogEx1_CalibrationStarted();
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
            AnalyticsManager.Instance.LogEx1_TutorialFinished();
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
            AnalyticsManager.Instance.LogEx1_CalibrationFinished();
            AnalyticsManager.Instance.LogEx1_TutorialStarted();
        }
    }
}
