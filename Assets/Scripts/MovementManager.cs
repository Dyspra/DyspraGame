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

        Debug.DrawRay(RightHandPoints[0].transform.position, GetTrianglePerpendicular(RightHandPoints[0].transform.position, RightHandPoints[5].transform.position, RightHandPoints[17].transform.position), Color.yellow);

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

        Vector3 R_perp = GetTrianglePerpendicular(RightHandPoints[0].transform.position, RightHandPoints[5].transform.position, RightHandPoints[17].transform.position);
        Vector3 L_perp = GetTrianglePerpendicular(LeftHandPoints[0].transform.position, LeftHandPoints[5].transform.position, LeftHandPoints[17].transform.position);

        R_0.transform.localRotation = Quaternion.Inverse(Quaternion.FromToRotation(RightHandPoints[0].transform.forward, R_perp));
        L_0.transform.localRotation = Quaternion.Inverse(Quaternion.FromToRotation(LeftHandPoints[0].transform.forward, L_perp));

        // Finger rotation

        RotateFinger(ref RightHandPoints, ref R_2, 0, 2, 2, 3);
        RotateFinger(ref RightHandPoints, ref R_3, 2, 3, 3, 4);
        RotateFinger(ref RightHandPoints, ref R_5, 0, 5, 5, 6);
        RotateFinger(ref RightHandPoints, ref R_6, 5, 6, 6, 7);
        RotateFinger(ref RightHandPoints, ref R_7, 6, 7, 7, 8);
        RotateFinger(ref RightHandPoints, ref R_9, 0, 9, 9, 10);
        RotateFinger(ref RightHandPoints, ref R_10, 9, 10, 10, 11);
        RotateFinger(ref RightHandPoints, ref R_11, 10, 11, 11, 12);
        RotateFinger(ref RightHandPoints, ref R_13, 0, 13, 13, 14);
        RotateFinger(ref RightHandPoints, ref R_14, 13, 14, 14, 15);
        RotateFinger(ref RightHandPoints, ref R_15, 14, 15, 15, 16);
        RotateFinger(ref RightHandPoints, ref R_17, 0, 17, 17, 18);
        RotateFinger(ref RightHandPoints, ref R_18, 17, 18, 18, 19);
        RotateFinger(ref RightHandPoints, ref R_19, 18, 19, 19, 20);

        RotateFinger(ref LeftHandPoints, ref L_2, 0, 2, 2, 3);
        RotateFinger(ref LeftHandPoints, ref L_3, 2, 3, 3, 4);
        RotateFinger(ref LeftHandPoints, ref L_5, 0, 5, 5, 6);
        RotateFinger(ref LeftHandPoints, ref L_6, 5, 6, 6, 7);
        RotateFinger(ref LeftHandPoints, ref L_7, 6, 7, 7, 8);
        RotateFinger(ref LeftHandPoints, ref L_9, 0, 9, 9, 10);
        RotateFinger(ref LeftHandPoints, ref L_10, 9, 10, 10, 11);
        RotateFinger(ref LeftHandPoints, ref L_11, 10, 11, 11, 12);
        RotateFinger(ref LeftHandPoints, ref L_13, 0, 13, 13, 14);
        RotateFinger(ref LeftHandPoints, ref L_14, 13, 14, 14, 15);
        RotateFinger(ref LeftHandPoints, ref L_15, 14, 15, 15, 16);
        RotateFinger(ref LeftHandPoints, ref L_17, 0, 17, 17, 18);
        RotateFinger(ref LeftHandPoints, ref L_18, 17, 18, 18, 19);
        RotateFinger(ref LeftHandPoints, ref L_19, 18, 19, 19, 20);
    }

    public void RotateFinger(ref GameObject[] objs, ref GameObject joint, int a, int b, int c, int d)
    {
        Vector3 newDir1 = objs[a].transform.position - objs[b].transform.position;
        Vector3 newDir2 = objs[c].transform.position - objs[d].transform.position;

        joint.transform.localRotation = Quaternion.Inverse(Quaternion.FromToRotation(newDir1, newDir2));
    }
    Vector3 GetTrianglePerpendicular(Vector3 a, Vector3 b , Vector3 c)
    {
        // Find vectors corresponding to two of the sides of the triangle.
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;

        // Cross the vectors to get a perpendicular vector, then normalize it.
        return Vector3.Cross(side1, side2);
    }

    Vector3 GetTriangleCenter(Vector3 a, Vector3 b, Vector3 c)
    {
        return new Vector3(((a.x + b.x + c.x) / 3), ((a.y + b.y + c.y) / 3), ((a.z + b.z + c.z) / 3));
    }
}
