using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public UDPServer server;
    // public ArmControl leftArm;
    // public ArmControl rightArm;
    public ArmSetter leftArm;
    public ArmSetter rightArm;
    //public HandPosition hp;
    [SerializeField] private GameObject[] LeftHandPoints;
    [SerializeField] private GameObject[] RightHandPoints;
    [SerializeField] private float speed = 10.0f;

    public static Vector2 RadianToVector2(float radian) {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
  
    public static Vector2 DegreeToVector2(float degree) {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    void Update()
    {
        HandPosition hp = server.HandsPosition;
        if (hp.packages.Count < 21)
        {
            Debug.Log("Count = " + hp.packages.Count);
            return;
        }

        Debug.Log("Count = " + hp.packages.Count);
        for (int i = 0; i < 21; i++)
        {
            Vector3 newPos = new Vector3(hp.packages[i].position.x * 5, hp.packages[i].position.y* 5, hp.packages[i].position.z);
            LeftHandPoints[i].transform.localPosition = Vector3.Lerp(LeftHandPoints[i].transform.localPosition, newPos, Time.deltaTime * speed);
            Debug.Log(hp.packages[i].landmark);
        }
        /*for (int i = 0; i < 21; i++)
        {
            RightHandPoints[i].transform.position = new Vector3(hp.packages[i + 21].position.x * 5, hp.packages[i + 21].position.y* 5, hp.packages[i + 21].position.z);
            //Debug.Log(RightHandPoints[i]);
        }*/

        //Vector3 dir = (leftArm.ArticulationsDict[Articulations.Hand].transform.position.normalized - new Vector3(hp.packages[0].position.x, hp.packages[0].position.y, 0.0f));
        //Debug.Log("hp x " + hp.packages[0].position.x.ToString("0." + new string('#', 339)) + " && hp y " + hp.packages[0].position.y.ToString("0." + new string('#', 339)) + " && hp z " + hp.packages[0].position.z.ToString("0." + new string('#', 339)));
        //float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.y);

        //leftArm.ArticulationsDict[Articulations.Hand].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Quaternion newRot = new Quaternion(hp.packages[0].position.x, hp.packages[0].position.y, hp.packages[0].position.z, 1.0f);
        //leftArm.ArticulationsDict[Articulations.Hand].transform.rotation = Quaternion.Lerp(leftArm.ArticulationsDict[Articulations.Hand].transform.rotation, newRot, speed * Time.deltaTime);
        //if (Input.GetKey(KeyCode.Space))
        //{
        //leftArm.ArticulationsDict[Articulations.Hand].transform.Rotate(hp.packages[0].position.x, hp.packages[0].position.y, hp.packages[0].position.z, Space.World);
        //leftArm.ArticulationsDict[Articulations.Hand].transform.position = new Vector3(hp.packages[0].position.x, hp.packages[0].position.y, hp.packages[0].position.z);

        /*// Determine which direction to rotate towards
        Vector3 targetDirection = new Vector3(hp.packages[0].position.x, hp.packages[0].position.y, hp.packages[0].position.z) - leftArm.ArticulationsDict[Articulations.Hand].transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(leftArm.ArticulationsDict[Articulations.Hand].transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        leftArm.ArticulationsDict[Articulations.Hand].transform.rotation = Quaternion.LookRotation(newDirection);*/
        //}
        // leftArm.Finger1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger1Angles, new Vector2(hp.packages[0].position.x, hp.packages[0].position.y)));
        // leftArm.Finger2Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger2Angles, new Vector2(hp.packages[5].position.x, hp.packages[5].position.y)));
        // leftArm.Finger2Joint1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger2Joint1Angles, new Vector2(hp.packages[6].position.x, hp.packages[6].position.y)));
        // leftArm.Finger2Joint2Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger2Joint2Angles, new Vector2(hp.packages[8].position.x, hp.packages[8].position.y)));
        // leftArm.Finger3Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger3Angles, new Vector2(hp.packages[9].position.x, hp.packages[9].position.y)));
        // leftArm.Finger3Joint1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger3Joint1Angles, new Vector2(hp.packages[10].position.x, hp.packages[10].position.y)));
        // leftArm.Finger3Joint2Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger3Joint2Angles, new Vector2(hp.packages[12].position.x, hp.packages[12].position.y)));
        // leftArm.Finger4Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger4Angles, new Vector2(hp.packages[13].position.x, hp.packages[13].position.y)));
        // leftArm.Finger4Joint1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger4Joint1Angles, new Vector2(hp.packages[14].position.x, hp.packages[14].position.y)));
        // leftArm.Finger4Joint2Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger4Joint2Angles, new Vector2(hp.packages[16].position.x, hp.packages[16].position.y)));
        // leftArm.Finger5Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger5Angles, new Vector2(hp.packages[17].position.x, hp.packages[17].position.y)));
        // leftArm.Finger5Joint1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger5Joint1Angles, new Vector2(hp.packages[18].position.x, hp.packages[18].position.y)));
        // leftArm.Finger5Joint2Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger5Joint2Angles, new Vector2(hp.packages[20].position.x, hp.packages[20].position.y)));

        // Debug.Log("x = " + hp.packages[0].position.x + " | y = " + hp.packages[0].position.y + " | z = " + hp.packages[0].position.z + " | landmark = " + hp.packages[0].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[1].position.x + " | y = " + hp.packages[1].position.y + " | z = " + hp.packages[1].position.z + " | landmark = " + hp.packages[1].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[2].position.x + " | y = " + hp.packages[2].position.y + " | z = " + hp.packages[2].position.z + " | landmark = " + hp.packages[2].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[3].position.x + " | y = " + hp.packages[3].position.y + " | z = " + hp.packages[3].position.z + " | landmark = " + hp.packages[3].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[4].position.x + " | y = " + hp.packages[4].position.y + " | z = " + hp.packages[4].position.z + " | landmark = " + hp.packages[4].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[5].position.x + " | y = " + hp.packages[5].position.y + " | z = " + hp.packages[5].position.z + " | landmark = " + hp.packages[5].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[6].position.x + " | y = " + hp.packages[6].position.y + " | z = " + hp.packages[6].position.z + " | landmark = " + hp.packages[6].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[7].position.x + " | y = " + hp.packages[7].position.y + " | z = " + hp.packages[7].position.z + " | landmark = " + hp.packages[7].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[8].position.x + " | y = " + hp.packages[8].position.y + " | z = " + hp.packages[8].position.z + " | landmark = " + hp.packages[8].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[9].position.x + " | y = " + hp.packages[9].position.y + " | z = " + hp.packages[9].position.z + " | landmark = " + hp.packages[9].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[10].position.x + " | y = " + hp.packages[10].position.y + " | z = " + hp.packages[10].position.z + " | landmark = " + hp.packages[10].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[11].position.x + " | y = " + hp.packages[11].position.y + " | z = " + hp.packages[11].position.z + " | landmark = " + hp.packages[11].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[12].position.x + " | y = " + hp.packages[12].position.y + " | z = " + hp.packages[12].position.z + " | landmark = " + hp.packages[12].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[13].position.x + " | y = " + hp.packages[13].position.y + " | z = " + hp.packages[13].position.z + " | landmark = " + hp.packages[13].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[14].position.x + " | y = " + hp.packages[14].position.y + " | z = " + hp.packages[14].position.z + " | landmark = " + hp.packages[14].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[15].position.x + " | y = " + hp.packages[15].position.y + " | z = " + hp.packages[15].position.z + " | landmark = " + hp.packages[15].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[21].position.x + " | y = " + hp.packages[21].position.y + " | z = " + hp.packages[21].position.z + " | landmark = " + hp.packages[21].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[17].position.x + " | y = " + hp.packages[17].position.y + " | z = " + hp.packages[17].position.z + " | landmark = " + hp.packages[17].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[16].position.x + " | y = " + hp.packages[16].position.y + " | z = " + hp.packages[16].position.z + " | landmark = " + hp.packages[16].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[18].position.x + " | y = " + hp.packages[18].position.y + " | z = " + hp.packages[18].position.z + " | landmark = " + hp.packages[18].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[19].position.x + " | y = " + hp.packages[19].position.y + " | z = " + hp.packages[19].position.z + " | landmark = " + hp.packages[19].landmark + " | date = " + hp.date);
        // Debug.Log("x = " + hp.packages[20].position.x + " | y = " + hp.packages[20].position.y + " | z = " + hp.packages[20].position.z + " | landmark = " + hp.packages[20].landmark + " | date = " + hp.date);
    }

    public void ApplyCoordinates(ref HandPosition hp)
    {
        //Debug.Log("x = " + hp.packages[1].position.x + " | y = " + hp.packages[1].position.y + " | z = " + hp.packages[1].position.z + " | landmark = " + hp.packages[1].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[1].position.x + " | y = " + hp.packages[1].position.y + " | z = " + hp.packages[1].position.z + " | landmark = " + hp.packages[1].landmark + " | date = " + hp.date);
        //leftArm.Finger1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger1Angles, new Vector2(hp.packages[0].position.x, hp.packages[0].position.y)));
        
        //leftArm.RotateArm();
        //Debug.Log("x = " + hp.packages[2].position.x + " | y = " + hp.packages[2].position.y + " | z = " + hp.packages[2].position.z + " | landmark = " + hp.packages[2].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[3].position.x + " | y = " + hp.packages[3].position.y + " | z = " + hp.packages[3].position.z + " | landmark = " + hp.packages[3].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[4].position.x + " | y = " + hp.packages[4].position.y + " | z = " + hp.packages[4].position.z + " | landmark = " + hp.packages[4].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[5].position.x + " | y = " + hp.packages[5].position.y + " | z = " + hp.packages[5].position.z + " | landmark = " + hp.packages[5].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[6].position.x + " | y = " + hp.packages[6].position.y + " | z = " + hp.packages[6].position.z + " | landmark = " + hp.packages[6].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[7].position.x + " | y = " + hp.packages[7].position.y + " | z = " + hp.packages[7].position.z + " | landmark = " + hp.packages[7].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[8].position.x + " | y = " + hp.packages[8].position.y + " | z = " + hp.packages[8].position.z + " | landmark = " + hp.packages[8].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[9].position.x + " | y = " + hp.packages[9].position.y + " | z = " + hp.packages[9].position.z + " | landmark = " + hp.packages[9].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[10].position.x + " | y = " + hp.packages[10].position.y + " | z = " + hp.packages[10].position.z + " | landmark = " + hp.packages[10].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[11].position.x + " | y = " + hp.packages[11].position.y + " | z = " + hp.packages[11].position.z + " | landmark = " + hp.packages[11].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[12].position.x + " | y = " + hp.packages[12].position.y + " | z = " + hp.packages[12].position.z + " | landmark = " + hp.packages[12].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[13].position.x + " | y = " + hp.packages[13].position.y + " | z = " + hp.packages[13].position.z + " | landmark = " + hp.packages[13].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[14].position.x + " | y = " + hp.packages[14].position.y + " | z = " + hp.packages[14].position.z + " | landmark = " + hp.packages[14].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[15].position.x + " | y = " + hp.packages[15].position.y + " | z = " + hp.packages[15].position.z + " | landmark = " + hp.packages[15].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[16].position.x + " | y = " + hp.packages[16].position.y + " | z = " + hp.packages[16].position.z + " | landmark = " + hp.packages[16].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[17].position.x + " | y = " + hp.packages[17].position.y + " | z = " + hp.packages[17].position.z + " | landmark = " + hp.packages[17].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[18].position.x + " | y = " + hp.packages[18].position.y + " | z = " + hp.packages[18].position.z + " | landmark = " + hp.packages[18].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[19].position.x + " | y = " + hp.packages[19].position.y + " | z = " + hp.packages[19].position.z + " | landmark = " + hp.packages[19].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[20].position.x + " | y = " + hp.packages[20].position.y + " | z = " + hp.packages[20].position.z + " | landmark = " + hp.packages[20].landmark + " | date = " + hp.date);
        //Debug.Log("x = " + hp.packages[21].position.x + " | y = " + hp.packages[21].position.y + " | z = " + hp.packages[21].position.z + " | landmark = " + hp.packages[21].landmark + " | date = " + hp.date);

    }
}






























//if (hp.packages.Count != 0)
//  leftArm.Finger1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger1Angles, new Vector2(hp.packages[3].position.x, hp.packages[3].position.y)));
//hp = server.HandsPosition;
//Debug.Log("x = " + hp.packages[0].position.x + " | y = " + hp.packages[0].position.y + " | z = " + hp.packages[0].position.z + " | landmark = " + hp.packages[0].landmark + " | date = " + hp.date);
//leftArm.Finger1Angles   =  DegreeToVector2(Vector2.Angle(leftArm.Finger1Angles, new Vector2(leftpoints[3].x, leftpoints[3].y)));
//Debug.Log("before : Actual pos = " + leftArm.Finger1Joint1Angles + " && new_pos = " + leftpoints[4]);
//if (leftpoints[4] != leftArm.Finger1Joint1Angles)
//    leftArm.Finger1Joint1Angles   =   leftpoints[4] - leftArm.Finger1Joint1Angles;
//Debug.Log("after = " + leftArm.Finger1Joint1Angles);
//leftArm.Finger2Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger2Angles, new Vector2(leftpoints[5].x, leftpoints[5].y)));
//leftArm.Finger2Joint1Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger2Joint1Angles, new Vector2(leftpoints[6].x, leftpoints[6].y)));
//leftArm.Finger2Joint2Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger2Joint2Angles, new Vector2(leftpoints[8].x, leftpoints[8].y)));
//leftArm.Finger3Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger3Angles, new Vector2(leftpoints[9].x, leftpoints[9].y)));
//leftArm.Finger3Joint1Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger3Joint1Angles, new Vector2(leftpoints[10].x, leftpoints[10].y)));
//leftArm.Finger3Joint2Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger3Joint2Angles, new Vector2(leftpoints[12].x, leftpoints[12].y)));
//leftArm.Finger4Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger4Angles, new Vector2(leftpoints[13].x, leftpoints[13].y)));
//leftArm.Finger4Joint1Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger4Joint1Angles, new Vector2(leftpoints[14].x, leftpoints[14].y)));
//leftArm.Finger4Joint2Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger4Joint2Angles, new Vector2(leftpoints[16].x, leftpoints[16].y)));
//leftArm.Finger5Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger5Angles, new Vector2(leftpoints[17].x, leftpoints[17].y)));
//leftArm.Finger5Joint1Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger5Joint1Angles, new Vector2(leftpoints[18].x, leftpoints[18].y)));
//leftArm.Finger5Joint2Angles   =   DegreeToVector2(Vector2.Angle(leftArm.Finger5Joint2Angles, new Vector2(leftpoints[20].x, leftpoints[20].y)));

//rightArm.Finger1Angles   =   new Vector2(rightpoints[3].x, rightpoints[3].y);
//rightArm.Finger1Joint1Angles   =   new Vector2(rightpoints[4].x, rightpoints[4].y);
//rightArm.Finger2Angles   =   new Vector2(rightpoints[5].x, rightpoints[5].y);
//rightArm.Finger2Joint1Angles   =   new Vector2(rightpoints[6].x, rightpoints[6].y);
//rightArm.Finger2Joint2Angles   =   new Vector2(rightpoints[8].x, rightpoints[8].y);
//rightArm.Finger3Angles   =   new Vector2(rightpoints[9].x, rightpoints[9].y);
//rightArm.Finger3Joint1Angles   =   new Vector2(rightpoints[10].x, rightpoints[10].y);
//rightArm.Finger3Joint2Angles   =   new Vector2(rightpoints[12].x, rightpoints[12].y);
//rightArm.Finger4Angles   =   new Vector2(rightpoints[13].x, rightpoints[13].y);
//rightArm.Finger4Joint1Angles   =   new Vector2(rightpoints[14].x, rightpoints[14].y);
//rightArm.Finger4Joint2Angles   =   new Vector2(rightpoints[16].x, rightpoints[16].y);
//rightArm.Finger5Angles   =   new Vector2(rightpoints[17].x, rightpoints[17].y);
//rightArm.Finger5Joint1Angles   =   new Vector2(rightpoints[18].x, rightpoints[18].y);
//rightArm.Finger5Joint2Angles   =   new Vector2(rightpoints[20].x, rightpoints[20].y);