using UnityEngine;


public class MovementManager : MonoBehaviour
{
    public struct FingerJoint
    {
        public FingerJoint(ref GameObject[] Hand, ref GameObject Joint, ref float[] CalibDist, float Offset, float CalibRatio, float FingerDist, int I, int A, int B)
        {
            hand = Hand;
            joint = Joint;
            calibDist = CalibDist;
            offset = Offset;
            calibRatio = CalibRatio;
            fingerDistanceRatio = FingerDist;
            i = I;
            a = A;
            b = B;
        }
        public GameObject[] hand;
        public GameObject joint;
        public float[] calibDist; public float offset; public float calibRatio; public float fingerDistanceRatio;
        public int i; public int a; public int b;
    }

    private FingerJoint[] fingerJoints = new FingerJoint[28];

    public UDPServer server;
    [SerializeField] private GameObject[] LeftHandPoints;
    [SerializeField] private GameObject[] RightHandPoints;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float fingerDistanceMultiplier = 5;

    [SerializeField] private float middleOffset = 270;
    [SerializeField] private float endOffset = 300;
    [SerializeField] private float offset = 90;
    [SerializeField] private float baseOffset = 360;

    [Header("FingersDistances")]
    private float[] rightCalibratedDistances = new float[14];
    private float[] leftCalibratedDistances = new float[14];
    private float fingerRDistanceRatio = 0;
    private float fingerLDistanceRatio = 0;
    private float calibratedPercentage = 0;
    private float RcalibratedRatio = 0;
    private float LcalibratedRatio = 0;
    public int isCalibrated = 0;
    float jointDistance = 0;
    float percentageDistance = 0;
    float angle = 0;
    bool isLeftCalibrated = false;
    bool isRightCalibrated = false;

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

    private void Start()
    {
        // Right hand
        fingerJoints[0] = new FingerJoint(ref RightHandPoints, ref R_2, ref rightCalibratedDistances    , 1,            RcalibratedRatio, fingerRDistanceRatio, 0, 1, 3);
        fingerJoints[1] = new FingerJoint(ref RightHandPoints, ref R_3, ref rightCalibratedDistances    , middleOffset, RcalibratedRatio, fingerRDistanceRatio, 1, 2, 4);
        fingerJoints[2] = new FingerJoint(ref RightHandPoints, ref R_5, ref rightCalibratedDistances    , 1,            RcalibratedRatio, fingerRDistanceRatio, 2, 0, 6);
        fingerJoints[3] = new FingerJoint(ref RightHandPoints, ref R_6, ref rightCalibratedDistances    , middleOffset, RcalibratedRatio, fingerRDistanceRatio, 3, 5, 7);
        fingerJoints[4] = new FingerJoint(ref RightHandPoints, ref R_7, ref rightCalibratedDistances    , endOffset,    RcalibratedRatio, fingerRDistanceRatio, 4, 6, 8);
        fingerJoints[5] = new FingerJoint(ref RightHandPoints, ref R_9, ref rightCalibratedDistances    , 1,            RcalibratedRatio, fingerRDistanceRatio, 5, 0, 10);
        fingerJoints[6] = new FingerJoint(ref RightHandPoints, ref R_10, ref rightCalibratedDistances   , middleOffset, RcalibratedRatio, fingerRDistanceRatio, 6, 9, 11);
        fingerJoints[7] = new FingerJoint(ref RightHandPoints, ref R_11, ref rightCalibratedDistances   , endOffset,   RcalibratedRatio, fingerRDistanceRatio, 7, 10, 12);
        fingerJoints[8] = new FingerJoint(ref RightHandPoints, ref R_13, ref rightCalibratedDistances   , 1,           RcalibratedRatio, fingerRDistanceRatio, 8, 0, 14);
        fingerJoints[9] = new FingerJoint(ref RightHandPoints, ref R_14, ref rightCalibratedDistances   , middleOffset, RcalibratedRatio, fingerRDistanceRatio, 9, 13, 15);
        fingerJoints[10] = new FingerJoint(ref RightHandPoints, ref R_15, ref rightCalibratedDistances  , endOffset,  RcalibratedRatio, fingerRDistanceRatio, 10, 14, 16);
        fingerJoints[11] = new FingerJoint(ref RightHandPoints, ref R_17, ref rightCalibratedDistances  , 1,          RcalibratedRatio, fingerRDistanceRatio, 11, 0, 18);
        fingerJoints[12] = new FingerJoint(ref RightHandPoints, ref R_18, ref rightCalibratedDistances  , middleOffset, RcalibratedRatio, fingerRDistanceRatio, 12, 17, 19);
        fingerJoints[13] = new FingerJoint(ref RightHandPoints, ref R_19, ref rightCalibratedDistances  , endOffset,  RcalibratedRatio, fingerRDistanceRatio, 13, 18, 20);

        //// Left hand
        fingerJoints[14] = new FingerJoint(ref LeftHandPoints, ref L_2 , ref leftCalibratedDistances, 1,            LcalibratedRatio, fingerLDistanceRatio, 0, 1, 3);
        fingerJoints[15] = new FingerJoint(ref LeftHandPoints, ref L_3 , ref leftCalibratedDistances, middleOffset, LcalibratedRatio, fingerLDistanceRatio, 1, 2, 4);
        fingerJoints[16] = new FingerJoint(ref LeftHandPoints, ref L_5 , ref leftCalibratedDistances, 1,            LcalibratedRatio, fingerLDistanceRatio, 2, 0, 6);
        fingerJoints[17] = new FingerJoint(ref LeftHandPoints, ref L_6 , ref leftCalibratedDistances, middleOffset, LcalibratedRatio, fingerLDistanceRatio, 3, 5, 7);
        fingerJoints[18] = new FingerJoint(ref LeftHandPoints, ref L_7 , ref leftCalibratedDistances, endOffset,    LcalibratedRatio, fingerLDistanceRatio, 4, 6, 8);
        fingerJoints[19] = new FingerJoint(ref LeftHandPoints, ref L_9 , ref leftCalibratedDistances, 1,            LcalibratedRatio, fingerLDistanceRatio, 5, 0, 10);
        fingerJoints[20] = new FingerJoint(ref LeftHandPoints, ref L_10, ref leftCalibratedDistances, middleOffset, LcalibratedRatio, fingerLDistanceRatio, 6, 9, 11);
        fingerJoints[21] = new FingerJoint(ref LeftHandPoints, ref L_11, ref leftCalibratedDistances, endOffset,    LcalibratedRatio, fingerLDistanceRatio, 7, 10, 12);
        fingerJoints[22] = new FingerJoint(ref LeftHandPoints, ref L_13, ref leftCalibratedDistances, 1,            LcalibratedRatio, fingerLDistanceRatio, 8, 0, 14);
        fingerJoints[23] = new FingerJoint(ref LeftHandPoints, ref L_14, ref leftCalibratedDistances, middleOffset, LcalibratedRatio, fingerLDistanceRatio, 9, 13, 15);
        fingerJoints[24] = new FingerJoint(ref LeftHandPoints, ref L_15, ref leftCalibratedDistances, endOffset,    LcalibratedRatio, fingerLDistanceRatio, 10, 14, 16);
        fingerJoints[25] = new FingerJoint(ref LeftHandPoints, ref L_17, ref leftCalibratedDistances, 1,            LcalibratedRatio, fingerLDistanceRatio, 11, 0, 18);
        fingerJoints[26] = new FingerJoint(ref LeftHandPoints, ref L_18, ref leftCalibratedDistances, middleOffset, LcalibratedRatio, fingerLDistanceRatio, 12, 17, 19);
        fingerJoints[27] = new FingerJoint(ref LeftHandPoints, ref L_19, ref leftCalibratedDistances, endOffset,    LcalibratedRatio, fingerLDistanceRatio, 13, 18, 20);

    }

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
            if (hp.packages[i].landmark > 20)
            {
                Debug.Log("Left first landmark n°" + hp.packages[i].landmark);
                LeftHandPoints[i].transform.localPosition = Vector3.Lerp(LeftHandPoints[i].transform.localPosition, newPos, Time.deltaTime * speed);
            } else
            {
                Debug.Log("Right first landmark n°" + hp.packages[i].landmark);
                RightHandPoints[i].transform.localPosition = Vector3.Lerp(RightHandPoints[i].transform.localPosition, newPos, Time.deltaTime * speed);
            }
        }

        if (hp.packages.Count < 42) 
        {
            return;
        }
        for (int i = 0; i < 21; i++)
        {
            Vector3 newPos = new Vector3(hp.packages[i + 21].position.x * -fingerDistanceMultiplier, hp.packages[i + 21].position.y* fingerDistanceMultiplier, hp.packages[i + 21].position.z * fingerDistanceMultiplier);
            if (hp.packages[i + 21].landmark > 20)
            {
                Debug.Log("Left second landmark n°" + hp.packages[i + 21].landmark);
                LeftHandPoints[i].transform.localPosition = Vector3.Lerp(LeftHandPoints[i].transform.localPosition, newPos, Time.deltaTime * speed);

            } else
            {
                Debug.Log("Right second landmark n°" + hp.packages[i + 21].landmark);
                RightHandPoints[i].transform.localPosition = Vector3.Lerp(RightHandPoints[i].transform.localPosition, newPos, Time.deltaTime * speed);
            }
        }

        fingerRDistanceRatio = Vector3.Distance(RightHandPoints[0].transform.position, RightHandPoints[9].transform.position);
        fingerLDistanceRatio = Vector3.Distance(LeftHandPoints[0].transform.position, LeftHandPoints[9].transform.position);
        if (Input.GetKeyDown("right"))
        {
            isRightCalibrated = true;
            CalibrateHands(ref rightCalibratedDistances, ref RightHandPoints, ref RcalibratedRatio);
            Debug.Log("Right hand calibrated");
        }
        if (Input.GetKeyDown("left"))
        {
            isLeftCalibrated = true;
            CalibrateHands(ref leftCalibratedDistances, ref LeftHandPoints, ref LcalibratedRatio);
            Debug.Log("Left hand calibrated");
        }
        UpdateHandModels(hp);
    }


    private void CalibrateHands(ref float[] distToCalibrate, ref GameObject[] hand, ref float calibratedRatio)
    {
        //  0    1      2    3    4      5     6      7        8     9     10       11     12    13
        // 3-1, 4-2 || 0-6, 7-5, 8-6 || 10-0, 11-9, 12-10 || 14-0, 15-13, 16-14 || 18-0, 19-17, 20-18
        distToCalibrate[0] = Vector3.Distance(hand[1].transform.position, hand[3].transform.position);
        distToCalibrate[1] = Vector3.Distance(hand[2].transform.position, hand[4].transform.position);
        distToCalibrate[2] = Vector3.Distance(hand[0].transform.position, hand[6].transform.position);
        distToCalibrate[3] = Vector3.Distance(hand[5].transform.position, hand[7].transform.position);
        distToCalibrate[4] = Vector3.Distance(hand[6].transform.position, hand[8].transform.position);
        distToCalibrate[5] = Vector3.Distance(hand[0].transform.position, hand[10].transform.position);
        distToCalibrate[6] = Vector3.Distance(hand[9].transform.position, hand[11].transform.position);
        distToCalibrate[7] = Vector3.Distance(hand[10].transform.position, hand[12].transform.position);
        distToCalibrate[8] = Vector3.Distance(hand[0].transform.position, hand[14].transform.position);
        distToCalibrate[9] = Vector3.Distance(hand[13].transform.position, hand[15].transform.position);
        distToCalibrate[10] = Vector3.Distance(hand[14].transform.position, hand[16].transform.position);
        distToCalibrate[11] = Vector3.Distance(hand[0].transform.position, hand[18].transform.position);
        distToCalibrate[12] = Vector3.Distance(hand[17].transform.position, hand[19].transform.position);
        distToCalibrate[13] = Vector3.Distance(hand[18].transform.position, hand[20].transform.position);

        calibratedRatio = Vector3.Distance(hand[0].transform.position, hand[9].transform.position);
    }

    public void UpdateHandModels(HandPosition hp)
    {
        // THE WRIST POSITION SHOULD BE REMAKE
        //if (hp.packages[0].landmark < 20)
        //{
        //    // Wrist position
        //    Vector3 newPos = new Vector3(hp.packages[0].position.x * -1, hp.packages[0].position.y, hp.packages[0].position.z);
        //    R_0.transform.position = Vector3.Lerp(R_0.transform.position, newPos, Time.deltaTime * speed);

        //    newPos = new Vector3(hp.packages[21].position.x * -1, hp.packages[21].position.y, hp.packages[21].position.z);
        //    L_0.transform.position = Vector3.Lerp(L_0.transform.position, newPos, Time.deltaTime * speed);
        //} else {
        //    // Wrist position
        //    Vector3 newPos = new Vector3(hp.packages[0].position.x * -1, hp.packages[0].position.y, hp.packages[0].position.z);
        //    L_0.transform.position = Vector3.Lerp(L_0.transform.position, newPos, Time.deltaTime * speed);

        //    newPos = new Vector3(hp.packages[21].position.x * -1, hp.packages[21].position.y, hp.packages[21].position.z);
        //    R_0.transform.position = Vector3.Lerp(R_0.transform.position, newPos, Time.deltaTime * speed);
        //}

        // Wrist rotation
        RotateWrist(ref LeftHandPoints, ref L_0, true);
        RotateWrist(ref RightHandPoints, ref R_0, false);


        // Finger rotation
        // 3-1, 4-2 || 0-6, 7-5, 8-6 || 10-0, 11-9, 12-10 || 14-0, 15-13, 16-14 || 18-0, 19-17, 20-18
        if (isRightCalibrated == true)
        {
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_2, ref rightCalibratedDistances, 0, 1, 3, 1, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_3, ref rightCalibratedDistances, 1, 2, 4, middleOffset, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_5, ref rightCalibratedDistances, 2, 0, 6, 1, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_6, ref rightCalibratedDistances, 3, 5, 7, middleOffset, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_7, ref rightCalibratedDistances, 4, 6, 8, endOffset, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_9, ref rightCalibratedDistances, 5, 0, 10, 1, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_10, ref rightCalibratedDistances, 6, 9, 11, middleOffset, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_11, ref rightCalibratedDistances, 7, 10, 12, endOffset, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_13, ref rightCalibratedDistances, 8, 0, 14, 1, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_14, ref rightCalibratedDistances, 9, 13, 15, middleOffset, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_15, ref rightCalibratedDistances, 10, 14, 16, endOffset, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_17, ref rightCalibratedDistances, 11, 0, 18, 1, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_18, ref rightCalibratedDistances, 12, 17, 19, middleOffset, RcalibratedRatio);
            RotateFingerJoint(fingerRDistanceRatio, ref RightHandPoints, ref R_19, ref rightCalibratedDistances, 13, 18, 20, endOffset, RcalibratedRatio);
        }

        if (isLeftCalibrated == true)
        {
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_2, ref leftCalibratedDistances, 0, 1, 3, 1, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_3, ref leftCalibratedDistances, 1, 2, 4, middleOffset, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_5, ref leftCalibratedDistances, 2, 0, 6, 1, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_6, ref leftCalibratedDistances, 3, 5, 7, middleOffset, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_7, ref leftCalibratedDistances, 4, 6, 8, endOffset, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_9, ref leftCalibratedDistances, 5, 0, 10, 1, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_10, ref leftCalibratedDistances, 6, 9, 11, middleOffset, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_11, ref leftCalibratedDistances, 7, 10, 12, endOffset, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_13, ref leftCalibratedDistances, 8, 0, 14, 1, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_14, ref leftCalibratedDistances, 9, 13, 15, middleOffset, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_15, ref leftCalibratedDistances, 10, 14, 16, endOffset, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_17, ref leftCalibratedDistances, 11, 0, 18,1, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_18, ref leftCalibratedDistances, 12, 17, 19, middleOffset, LcalibratedRatio);
            RotateFingerJoint(fingerLDistanceRatio, ref LeftHandPoints, ref L_19, ref leftCalibratedDistances, 13, 18, 20, endOffset, LcalibratedRatio);
        }
    }

    private void RotateFingerJoint(float fingerDistanceRatio, ref GameObject[] hand, ref GameObject joint, ref float[] calibDist, int i, int a, int b, float offset = 1, float calibRatio = 1)
    {
        jointDistance = Vector3.Distance(hand[a].transform.position, hand[b].transform.position);
        float maxDistance = calibDist[i] * (fingerDistanceRatio / calibRatio);

        if (jointDistance >= maxDistance)
            return;

        percentageDistance = ((jointDistance / maxDistance) * 100);
        angle = percentageDistance;

        if (offset != 1) // if the joint isn't a base
        {
            angle = ((360 * percentageDistance) / 100);
            if (angle < offset)
                angle = offset;
        }
        else
        {
            angle = ((360 * percentageDistance) / 100);
        }

        joint.transform.localRotation = Quaternion.Euler(new Vector3(angle, 0.0f, 0.0f));
    }

    private void RotateWrist(ref GameObject[] wrist, ref GameObject hand, bool isLeft)
    {
        Vector3 forY;
        if (isLeft == false)
            forY = GetTrianglePerpendicular(wrist[0].transform.position, wrist[5].transform.position, wrist[17].transform.position);
        else
            forY = GetTrianglePerpendicular(wrist[0].transform.position, wrist[5].transform.position, wrist[17].transform.position) * -1;
        Vector3 forZ = Vector3.Normalize(wrist[9].transform.position - wrist[0].transform.position);
        Vector3 forX = Vector3.Cross(forY, forZ);

        Debug.DrawRay(wrist[0].transform.position, forY, Color.green); //forY
        Debug.DrawRay(wrist[0].transform.position, forZ, Color.blue);
        Debug.DrawRay(wrist[0].transform.position, forX, Color.red);

        Matrix4x4 rotationMatrix = new Matrix4x4();
        rotationMatrix.SetColumn(0, forX);
        rotationMatrix.SetColumn(1, forY);
        rotationMatrix.SetColumn(2, forZ);

        if (Mathf.Approximately(Vector3.Dot(rotationMatrix.GetColumn(0), rotationMatrix.GetColumn(1)), 0) &&
        Mathf.Approximately(Vector3.Dot(rotationMatrix.GetColumn(0), rotationMatrix.GetColumn(2)), 0) &&
        Mathf.Approximately(Vector3.Dot(rotationMatrix.GetColumn(1), rotationMatrix.GetColumn(2)), 0))
            rotationMatrix.SetColumn(0, rotationMatrix.GetColumn(0).normalized);

        rotationMatrix.SetColumn(1, (rotationMatrix.GetColumn(1) - Vector3.Dot(rotationMatrix.GetColumn(0), rotationMatrix.GetColumn(1)) * rotationMatrix.GetColumn(0)).normalized);
        rotationMatrix.SetColumn(2, (rotationMatrix.GetColumn(2) - Vector3.Dot(rotationMatrix.GetColumn(0), rotationMatrix.GetColumn(2)) * rotationMatrix.GetColumn(0) - Vector3.Dot(rotationMatrix.GetColumn(1), rotationMatrix.GetColumn(2)) * rotationMatrix.GetColumn(1)).normalized);

        hand.transform.localRotation = Quaternion.Lerp(hand.transform.localRotation, FromRotationMatrix(rotationMatrix), Time.deltaTime * speed);
    }

    private void RotateFinger(ref GameObject[] objs, ref GameObject joint, int a, int b, int c, int d)
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

    private Quaternion FromRotationMatrix(Matrix4x4 m)
    {
        Quaternion q = new Quaternion();
        q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m.m00 + m.m11 + m.m22)) / 2;
        q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m.m00 - m.m11 - m.m22)) / 2;
        q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m.m00 + m.m11 - m.m22)) / 2;
        q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m.m00 - m.m11 + m.m22)) / 2;
        q.x *= Mathf.Sign(q.x * (m.m21 - m.m12));
        q.y *= Mathf.Sign(q.y * (m.m02 - m.m20));
        q.z *= Mathf.Sign(q.z * (m.m10 - m.m01));
        return q;
    }

}
