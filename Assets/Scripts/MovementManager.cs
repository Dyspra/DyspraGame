using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public UDPServer server;
    public ArmControl leftArm;
    public ArmControl rightArm;
    //public HandPosition positions;

    public static Vector2 RadianToVector2(float radian) {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
  
    public static Vector2 DegreeToVector2(float degree) {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public void ApplyCoordinates(ref HandPosition positions)
    {
        Debug.Log("APPLY COORDINATES");
        Debug.Log("x = " + positions.packages[0].position.x + " | y = " + positions.packages[0].position.y + " | z = " + positions.packages[0].position.z + " | landmark = " + positions.packages[0].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[1].position.x + " | y = " + positions.packages[1].position.y + " | z = " + positions.packages[1].position.z + " | landmark = " + positions.packages[1].landmark + " | date = " + positions.date);
        leftArm.Finger1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger1Angles, new Vector2(positions.packages[0].position.x, positions.packages[0].position.y)));
        //leftArm.armSetter.arm.Finger1.transform.Rotate(leftArm.Finger1Angles.x, leftArm.Finger1Angles.y, 0f, Space.Self);
        
        //leftArm.RotateArm();
        //Debug.Log("x = " + positions.packages[2].position.x + " | y = " + positions.packages[2].position.y + " | z = " + positions.packages[2].position.z + " | landmark = " + positions.packages[2].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[3].position.x + " | y = " + positions.packages[3].position.y + " | z = " + positions.packages[3].position.z + " | landmark = " + positions.packages[3].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[4].position.x + " | y = " + positions.packages[4].position.y + " | z = " + positions.packages[4].position.z + " | landmark = " + positions.packages[4].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[5].position.x + " | y = " + positions.packages[5].position.y + " | z = " + positions.packages[5].position.z + " | landmark = " + positions.packages[5].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[6].position.x + " | y = " + positions.packages[6].position.y + " | z = " + positions.packages[6].position.z + " | landmark = " + positions.packages[6].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[7].position.x + " | y = " + positions.packages[7].position.y + " | z = " + positions.packages[7].position.z + " | landmark = " + positions.packages[7].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[8].position.x + " | y = " + positions.packages[8].position.y + " | z = " + positions.packages[8].position.z + " | landmark = " + positions.packages[8].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[9].position.x + " | y = " + positions.packages[9].position.y + " | z = " + positions.packages[9].position.z + " | landmark = " + positions.packages[9].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[10].position.x + " | y = " + positions.packages[10].position.y + " | z = " + positions.packages[10].position.z + " | landmark = " + positions.packages[10].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[11].position.x + " | y = " + positions.packages[11].position.y + " | z = " + positions.packages[11].position.z + " | landmark = " + positions.packages[11].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[12].position.x + " | y = " + positions.packages[12].position.y + " | z = " + positions.packages[12].position.z + " | landmark = " + positions.packages[12].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[13].position.x + " | y = " + positions.packages[13].position.y + " | z = " + positions.packages[13].position.z + " | landmark = " + positions.packages[13].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[14].position.x + " | y = " + positions.packages[14].position.y + " | z = " + positions.packages[14].position.z + " | landmark = " + positions.packages[14].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[15].position.x + " | y = " + positions.packages[15].position.y + " | z = " + positions.packages[15].position.z + " | landmark = " + positions.packages[15].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[16].position.x + " | y = " + positions.packages[16].position.y + " | z = " + positions.packages[16].position.z + " | landmark = " + positions.packages[16].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[17].position.x + " | y = " + positions.packages[17].position.y + " | z = " + positions.packages[17].position.z + " | landmark = " + positions.packages[17].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[18].position.x + " | y = " + positions.packages[18].position.y + " | z = " + positions.packages[18].position.z + " | landmark = " + positions.packages[18].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[19].position.x + " | y = " + positions.packages[19].position.y + " | z = " + positions.packages[19].position.z + " | landmark = " + positions.packages[19].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[20].position.x + " | y = " + positions.packages[20].position.y + " | z = " + positions.packages[20].position.z + " | landmark = " + positions.packages[20].landmark + " | date = " + positions.date);
        //Debug.Log("x = " + positions.packages[21].position.x + " | y = " + positions.packages[21].position.y + " | z = " + positions.packages[21].position.z + " | landmark = " + positions.packages[21].landmark + " | date = " + positions.date);
        /*leftArm.Finger2Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger2Angles, new Vector2(positions.packages[5].position.x, positions.packages[5].position.y)));
        leftArm.Finger2Joint1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger2Joint1Angles, new Vector2(positions.packages[6].position.x, positions.packages[6].position.y)));
        leftArm.Finger2Joint2Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger2Joint2Angles, new Vector2(positions.packages[8].position.x, positions.packages[8].position.y)));
        leftArm.Finger3Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger3Angles, new Vector2(positions.packages[9].position.x, positions.packages[9].position.y)));
        leftArm.Finger3Joint1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger3Joint1Angles, new Vector2(positions.packages[10].position.x, positions.packages[10].position.y)));
        leftArm.Finger3Joint2Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger3Joint2Angles, new Vector2(positions.packages[12].position.x, positions.packages[12].position.y)));
        leftArm.Finger4Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger4Angles, new Vector2(positions.packages[13].position.x, positions.packages[13].position.y)));
        leftArm.Finger4Joint1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger4Joint1Angles, new Vector2(positions.packages[14].position.x, positions.packages[14].position.y)));
        leftArm.Finger4Joint2Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger4Joint2Angles, new Vector2(positions.packages[16].position.x, positions.packages[16].position.y)));
        leftArm.Finger5Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger5Angles, new Vector2(positions.packages[17].position.x, positions.packages[17].position.y)));
        leftArm.Finger5Joint1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger5Joint1Angles, new Vector2(positions.packages[18].position.x, positions.packages[18].position.y)));
        leftArm.Finger5Joint2Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger5Joint2Angles, new Vector2(positions.packages[20].position.x, positions.packages[20].position.y)));*/
    }
}






























//if (positions.packages.Count != 0)
//  leftArm.Finger1Angles = DegreeToVector2(Vector2.Angle(leftArm.Finger1Angles, new Vector2(positions.packages[3].position.x, positions.packages[3].position.y)));
//positions = server.HandsPosition;
//Debug.Log("x = " + positions.packages[0].position.x + " | y = " + positions.packages[0].position.y + " | z = " + positions.packages[0].position.z + " | landmark = " + positions.packages[0].landmark + " | date = " + positions.date);
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