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
        RotateWrist(ref LeftHandPoints, ref L_0, false);
        RotateWrist(ref RightHandPoints, ref R_0, true);

        //Quaternion r = Quaternion.FromToRotation(LeftHandPoints[0].transform.forward, L_perp);
        //Quaternion r2 = Quaternion.FromToRotation(LeftHandPoints[0].transform.forward, forZ);
        //Quaternion r3 = Quaternion.FromToRotation(LeftHandPoints[0].transform.forward, forX);

        // Création des quaternions de rotation pour chaque axe
        //Quaternion r = Quaternion.AngleAxis(L_0.transform.localRotation.x, forX * 180);
        //Quaternion r2 = Quaternion.AngleAxis(L_0.transform.localRotation.y, L_perp * 180);
        //Quaternion r3 = Quaternion.AngleAxis(L_0.transform.localRotation.z, forZ * 180);

        // Combinaison des quaternions de rotation pour les trois axes
        //Quaternion finalRotation = xQuaternion * yQuaternion * zQuaternion;

        // Application de la rotation à l'objet
        //L_0.transform.localRotation = finalRotation;

        //Vector3 MyEuler = new Vector3(r3.z * 180, r.y * 180, r2.y * 180);
        //Quaternion rot = Quaternion.Euler(MyEuler);
        //Quaternion rot = r * r2 * r3;
        //L_0.transform.localRotation = rot;
        //L_0.transform.localEulerAngles = MyEuler;


        //MyEuler = new Vector3(r.x * 180, r.y * 180, L_0.transform.localEulerAngles.z);
        //Debug.Log("Left Local Euler = " + L_0.transform.localEulerAngles);
        //L_0.transform.localEulerAngles = MyEuler;

        //Vector3 MyEuler1 = Vector3.Lerp(L_0.transform.localEulerAngles, new Vector3(r.x * 180, L_0.transform.localEulerAngles.y, L_0.transform.localEulerAngles.z), Time.deltaTime * speed);
        //Vector3 MyEuler2 = Vector3.Lerp(L_0.transform.localEulerAngles, new Vector3(L_0.transform.localEulerAngles.x, r.y * 180, L_0.transform.localEulerAngles.z), Time.deltaTime * speed);
        //Vector3 MyEuler3 = Vector3.Lerp(L_0.transform.localEulerAngles, new Vector3(L_0.transform.localEulerAngles.x, L_0.transform.localEulerAngles.y, r2.z * 180), Time.deltaTime * speed);
        //Vector3 MyEuler = new Vector3(MyEuler1.x, MyEuler2.y, MyEuler3.z);
        //float rotX = Mathf.Lerp(L_0.transform.localRotation.x, r.x + L_0.transform.localRotation.x, Time.deltaTime * speed);
        //float rotY = Mathf.Lerp(L_0.transform.localRotation.y, r.y + L_0.transform.localRotation.y, Time.deltaTime * speed);
        //float rotZ = Mathf.Lerp(L_0.transform.localRotation.z, r2.y + L_0.transform.localRotation.z, Time.deltaTime * speed);
        //L_0.transform.localRotation = Quaternion.RotateTowards(L_0.transform.localRotation, rot, speed * 100);
        //R_0.transform.localRotation = new Quaternion(R_perp.x, R_perp.y, R_perp.z, 1.0f);
        //R_0.transform.localRotation = Quaternion.LookRotation(R_perp, Vector3.up);
        //R_0.transform.localRotation = Quaternion.RotateTowards(R_0.transform.localRotation, Quaternion.FromToRotation(R_perp, R_0.transform.forward), speed);
        //R_0.transform.rotation = Quaternion.FromToRotation(RightHandPoints[0].transform.forward, R_perp);
        //L_0.transform.localRotation = Quaternion.FromToRotation(LeftHandPoints[0].transform.forward, L_perp);
        //L_0.transform.localRotation = Quaternion.Slerp(L_0.transform.localRotation, rot, Time.deltaTime * speed);
        //L_0.transform.localRotation = new Quaternion(L_0.transform.localRotation.x, r.y, L_0.transform.localRotation.z, L_0.transform.localRotation.w);
        //rot = new Quaternion(L_0.transform.localRotation.x, L_0.transform.localRotation.y, r2.y, r2.w);
        //L_0.transform.localRotation = Quaternion.Lerp(L_0.transform.localRotation, Quaternion.Euler(MyEuler), Time.deltaTime * speed);
        //L_0.transform.localEulerAngles = Vector3.Lerp(L_0.transform.localEulerAngles, MyEuler, Time.deltaTime * speed);
        //BaseX.transform.localEulerAngles = rotX;
        //BaseY.transform.localEulerAngles = rotY;
        //BaseZ.transform.localEulerAngles = rotZ;
        //L_0.transform.eulerAngles = MyEuler;
        //L_0.transform.localEulerAngles = MyEuler;

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

    private void RotateWrist(ref GameObject[] wrist, ref GameObject hand, bool shouldInverse)
    {
        Vector3 forY = GetTrianglePerpendicular(wrist[0].transform.position, wrist[5].transform.position, wrist[17].transform.position);
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
        //if (shouldInverse == true)
        //    hand.transform.localRotation = Quaternion.Inverse(Quaternion.Lerp(hand.transform.localRotation, FromRotationMatrix(rotationMatrix), Time.deltaTime * speed));
        //else
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
