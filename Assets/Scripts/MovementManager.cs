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
        RotateFingers(hp);
    }

    public void RotateFingers(HandPosition hp)
    {   
        Vector3 newPos = new Vector3(hp.packages[0].position.x * -1, hp.packages[0].position.y, hp.packages[0].position.z);
        R_0.transform.position = Vector3.Lerp(R_0.transform.position, newPos, Time.deltaTime * speed);
        RotateFinger(R_2,  hp.packages[0].position - hp.packages[2].position,  hp.packages[2].position -  hp.packages[3].position);
        //RotateFinger(R_3,  hp.packages[2].position - hp.packages[3].position,  hp.packages[3].position -  hp.packages[4].position);
        RotateFinger(R_5,  hp.packages[0].position - hp.packages[5].position,  hp.packages[5].position -  hp.packages[6].position);        
        //RotateFinger(R_6,  hp.packages[5].position - hp.packages[6].position,  hp.packages[6].position -  hp.packages[7].position);
        //RotateFinger(R_7,  hp.packages[6].position - hp.packages[7].position,  hp.packages[7].position -  hp.packages[8].position);
        RotateFinger(R_9,  hp.packages[0].position - hp.packages[9].position,  hp.packages[9].position -  hp.packages[10].position);
        //RotateFinger(R_10, hp.packages[9].position - hp.packages[10].position, hp.packages[10].position - hp.packages[11].position);
        //RotateFinger(R_11, hp.packages[10].position -hp.packages[11].position, hp.packages[11].position - hp.packages[12].position);
        RotateFinger(R_13, hp.packages[0].position - hp.packages[13].position, hp.packages[13].position - hp.packages[14].position);
        //RotateFinger(R_14, hp.packages[13].position -hp.packages[14].position, hp.packages[14].position - hp.packages[15].position);
        //RotateFinger(R_15, hp.packages[14].position -hp.packages[15].position, hp.packages[15].position - hp.packages[16].position);
        RotateFinger(R_17, hp.packages[0].position - hp.packages[17].position, hp.packages[17].position - hp.packages[18].position);
        //RotateFinger(R_18, hp.packages[17].position -hp.packages[18].position, hp.packages[18].position - hp.packages[19].position);
        //RotateFinger(R_19, hp.packages[18].position -hp.packages[19].position, hp.packages[19].position - hp.packages[20].position);
    }

    public void RotateFinger(GameObject joint, Vector3 dir1, Vector3 dir2)
    {
        Vector3 newDir1 = new Vector3(dir1.x * -5, dir1.y * 5, dir1.z * 5);
        Vector3 newDir2 = new Vector3(dir1.x * -5, dir1.y * 5, dir1.z * 5);

        joint.transform.localRotation = Quaternion.Inverse(Quaternion.FromToRotation(newDir1, newDir2));
    }
}
