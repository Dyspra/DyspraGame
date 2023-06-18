using UnityEngine;

public class TutoBaloonFair : MonoBehaviour
{
    [SerializeField] private MissionBalloonFairScore Mission;
    [SerializeField] private MovementManager MovManager;

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
        // if (MovManager.server.HandsPosition.packages.Count < 42)
        if (HandTrackingManager.Instance.handTracking.LeftHandLandmarks.Length < 21 || HandTrackingManager.Instance.handTracking.RightHandLandmarks.Length < 21)
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
}
