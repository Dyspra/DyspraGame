using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public UDPServer server;
    public ArmControl leftArm;
    public ArmControl rightArm;
    public HandPosition positions;

    // Update is called once per frame
    void Update()
    {
        positions = server.HandsPosition;
        Debug.Log("x = " + positions.packages[0].position.x + " | y = " + positions.packages[0].position.y + " | z = " + positions.packages[0].position.z + " | landmark = " + positions.packages[0].landmark + " | date = " + positions.date);
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
    }

    public static Vector2 RadianToVector2(float radian) {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
  
    public static Vector2 DegreeToVector2(float degree) {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
}
