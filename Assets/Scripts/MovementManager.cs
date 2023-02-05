using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public UDPServer server;
    [SerializeField] private GameObject[] LeftHandPoints;
    [SerializeField] private GameObject[] RightHandPoints;
    [SerializeField] private float speed = 10.0f;

    [Header("Left hand")]
    [SerializeField] private GameObject L_0;
    //[SerializeField] private GameObject L_1;
    [SerializeField] private GameObject L_2;
    [SerializeField] private GameObject L_3;
    [SerializeField] private GameObject L_5;
    [SerializeField] private GameObject L_6;
    [SerializeField] private GameObject L_7;
    [SerializeField] private GameObject L_9;
    [SerializeField] private GameObject L_10;
    [SerializeField] private GameObject L_11;
    [SerializeField] private GameObject L_13;
    [SerializeField] private GameObject L_14;
    [SerializeField] private GameObject L_15;
    [SerializeField] private GameObject L_17;
    [SerializeField] private GameObject L_18;
    [SerializeField] private GameObject L_19;

    [Header("Right hand")]
    [SerializeField] private GameObject R_0;
    //[SerializeField] private GameObject R_1;
    [SerializeField] private GameObject R_2;
    [SerializeField] private GameObject R_3;
    [SerializeField] private GameObject R_5;
    [SerializeField] private GameObject R_6;
    [SerializeField] private GameObject R_7;
    [SerializeField] private GameObject R_9;
    [SerializeField] private GameObject R_10;
    [SerializeField] private GameObject R_11;
    [SerializeField] private GameObject R_13;
    [SerializeField] private GameObject R_14;
    [SerializeField] private GameObject R_15;
    [SerializeField] private GameObject R_17;
    [SerializeField] private GameObject R_18;
    [SerializeField] private GameObject R_19;

    void Update()
    {
        HandPosition hp = server.HandsPosition;
        if (hp.packages.Count < 21)
        {
            return;
        }

        for (int i = 0; i < 21; i++)
        {
            Vector3 newPos = new Vector3(hp.packages[i].position.x * -5, hp.packages[i].position.y* 5, hp.packages[i].position.z * 5);
            RightHandPoints[i].transform.localPosition = Vector3.Lerp(RightHandPoints[i].transform.localPosition, newPos, Time.deltaTime * speed);
        }

        if (hp.packages.Count < 42)
        {
            return;
        }
        for (int i = 0; i < 21; i++)
        {
            Vector3 newPos = new Vector3(hp.packages[i + 21].position.x * -5, hp.packages[i + 21].position.y* 5, hp.packages[i + 21].position.z * 5);
            LeftHandPoints[i].transform.localPosition = Vector3.Lerp(LeftHandPoints[i].transform.localPosition, newPos, Time.deltaTime * speed);
        }
        RotateFingers();
    }

    public void RotateFingers()
    {
        Vector3 dir1 = RightHandPoints[0].transform.position - RightHandPoints[5].transform.position;
        Vector3 dir2 = RightHandPoints[5].transform.position - RightHandPoints[6].transform.position;

        R_5.transform.localRotation = Quaternion.Inverse(Quaternion.FromToRotation(dir1, dir2));
    }
}
