using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public UDPServer server;
    [SerializeField] private GameObject[] LeftHandPoints;
    [SerializeField] private GameObject[] RightHandPoints;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float fingerDistanceMultiplier = 5;

    [Header("Left hand")]
    [SerializeField] private GameObject L_0;
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
            Vector3 newPos = new Vector3(hp.packages[i].position.x * -fingerDistanceMultiplier, hp.packages[i].position.y* fingerDistanceMultiplier, hp.packages[i].position.z * fingerDistanceMultiplier);
            RightHandPoints[i].transform.localPosition = Vector3.Lerp(RightHandPoints[i].transform.localPosition, newPos, Time.deltaTime * speed);
        }

        if (hp.packages.Count < 42)
        {
            return;
        }
        for (int i = 0; i < 21; i++)
        {
            Vector3 newPos = new Vector3(hp.packages[i + 21].position.x * -fingerDistanceMultiplier, hp.packages[i + 21].position.y* fingerDistanceMultiplier, hp.packages[i + 21].position.z * fingerDistanceMultiplier);
            LeftHandPoints[i].transform.localPosition = Vector3.Lerp(LeftHandPoints[i].transform.localPosition, newPos, Time.deltaTime * speed);
        }
        UpdateHandModels(hp);
    }

    public void UpdateHandModels(HandPosition hp)
    {
        // Wrist position
        Vector3 newPos = new Vector3(hp.packages[0].position.x * -1, hp.packages[0].position.y, hp.packages[0].position.z);
        R_0.transform.position = Vector3.Lerp(R_0.transform.position, newPos, Time.deltaTime * speed);

        newPos = new Vector3(hp.packages[21].position.x * -1, hp.packages[21].position.y, hp.packages[21].position.z);
        L_0.transform.position = Vector3.Lerp(L_0.transform.position, newPos, Time.deltaTime * speed);

        // Wrist rotation

        // Finger rotation

        RotateRFinger(R_2, 0, 2, 2, 3);
        RotateRFinger(R_3, 2, 3, 3, 4);
        RotateRFinger(R_5, 0, 5, 5, 6);
        RotateRFinger(R_6, 5, 6, 6, 7);
        RotateRFinger(R_7, 6, 7, 7, 8);
        RotateRFinger(R_9, 0, 9, 9, 10);
        RotateRFinger(R_10, 9, 10, 10, 11);
        RotateRFinger(R_11, 10, 11, 11, 12);
        RotateRFinger(R_13, 0, 13, 13, 14);
        RotateRFinger(R_14, 13, 14, 14, 15);
        RotateRFinger(R_15, 14, 15, 15, 16);
        RotateRFinger(R_17, 0, 17, 17, 18);
        RotateRFinger(R_18, 17, 18, 18, 19);
        RotateRFinger(R_19, 18, 19, 19, 20);

        RotateLFinger(L_2, 0, 2, 2, 3);
        RotateLFinger(L_3, 2, 3, 3, 4);
        RotateLFinger(L_5, 0, 5, 5, 6);
        RotateLFinger(L_6, 5, 6, 6, 7);
        RotateLFinger(L_7, 6, 7, 7, 8);
        RotateLFinger(L_9, 0, 9, 9, 10);
        RotateLFinger(L_10, 9, 10, 10, 11);
        RotateLFinger(L_11, 10, 11, 11, 12);
        RotateLFinger(L_13, 0, 13, 13, 14);
        RotateLFinger(L_14, 13, 14, 14, 15);
        RotateLFinger(L_15, 14, 15, 15, 16);
        RotateLFinger(L_17, 0, 17, 17, 18);
        RotateLFinger(L_18, 17, 18, 18, 19);
        RotateLFinger(L_19, 18, 19, 19, 20);
    }

    public void RotateRFinger(GameObject joint, int a, int b, int c, int d)
    {
        Vector3 newDir1 = RightHandPoints[a].transform.position - RightHandPoints[b].transform.position;
        Vector3 newDir2 = RightHandPoints[c].transform.position - RightHandPoints[d].transform.position;

        joint.transform.localRotation = Quaternion.Inverse(Quaternion.FromToRotation(newDir1, newDir2));
    }

    public void RotateLFinger(GameObject joint, int a, int b, int c, int d)
    {
        Vector3 newDir1 = LeftHandPoints[a].transform.position - LeftHandPoints[b].transform.position;
        Vector3 newDir2 = LeftHandPoints[c].transform.position - LeftHandPoints[d].transform.position;

        joint.transform.localRotation = Quaternion.Inverse(Quaternion.FromToRotation(newDir1, newDir2));
    }
}
