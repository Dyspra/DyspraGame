using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class NewMovementManager : MonoBehaviour
{
    public Transform[] leftHandJoints;
    public Transform[] rightHandJoints;
    public float smoothingFactor = 10f;   // Smoothing factor for rotations

    private Quaternion initialLeftHandRotation;
    private Quaternion initialRightHandRotation;



    private void Awake()
    {
        // Initialize left hand joints
        leftHandJoints = new Transform[21];
        UnityEngine.Debug.Log("RootLeft: " + transform.Find("RootLeft/LeftHand/hand/root"));
        // wrist
        leftHandJoints[0] = transform.Find("RootLeft/LeftHand/hand/root");
        // thumb
        leftHandJoints[1] = leftHandJoints[0].Find("R_FK_Thumb_null/R_Thumb_null/R_NB_FK_Thumb_Root_jnt");
        leftHandJoints[2] = leftHandJoints[1].Find("R_NB_FK_Thumb_Base_jnt");
        leftHandJoints[3] = leftHandJoints[2].Find("R_NB_FK_Thumb_Mid_jnt");
        leftHandJoints[4] = leftHandJoints[3].Find("R_NB_FK_Thumb_End_jnt");
        // index
        leftHandJoints[5] = leftHandJoints[0].Find("R_FK_Finger_null/R_FK_Finger_null_2_3/R_NB_FK_Finger_Palm_Base_jnt/R_NB_FK_Finger_Base_jnt");
        leftHandJoints[6] = leftHandJoints[5].Find("R_NB_FK_Finger_Mid_jnt");
        leftHandJoints[7] = leftHandJoints[6].Find("R_NB_FK_Finger_Top_jnt");
        leftHandJoints[8] = leftHandJoints[7].Find("R_NB_FK_Finger_End_jnt");
        // middle
        leftHandJoints[9] = leftHandJoints[0].Find("R_FK_Finger_null_1/R_FK_Finger_null_1_2/R_NB_FK_Finger_Palm_Base_jnt_1/R_NB_FK_Finger_Base_jnt_1");
        leftHandJoints[10] = leftHandJoints[9].Find("R_NB_FK_Finger_Mid_jnt_1");
        leftHandJoints[11] = leftHandJoints[10].Find("R_NB_FK_Finger_Top_jnt_1");
        leftHandJoints[12] = leftHandJoints[11].Find("R_NB_FK_Finger_End_jnt_1");
        // ring
        leftHandJoints[13] = leftHandJoints[0].Find("R_FK_Finger_null_2/R_FK_Finger_null_2_2/R_NB_FK_Finger_Palm_Base_jnt_2/R_NB_FK_Finger_Base_jnt_2");
        leftHandJoints[14] = leftHandJoints[13].Find("R_NB_FK_Finger_Mid_jnt_2");
        leftHandJoints[15] = leftHandJoints[14].Find("R_NB_FK_Finger_Top_jnt_2");
        leftHandJoints[16] = leftHandJoints[15].Find("R_NB_FK_Finger_End_jnt_2");
        // pinky
        leftHandJoints[17] = leftHandJoints[0].Find("R_FK_Finger_null_3/R_FK_Finger_null_3_2/R_NB_FK_Finger_Palm_Base_jnt_3/R_NB_FK_Finger_Base_jnt_3");
        leftHandJoints[18] = leftHandJoints[17].Find("R_NB_FK_Finger_Mid_jnt_3");
        leftHandJoints[19] = leftHandJoints[18].Find("R_NB_FK_Finger_Top_jnt_3");
        leftHandJoints[20] = leftHandJoints[19].Find("R_NB_FK_Finger_End_jnt_3");

        // Initialize right hand joints
        rightHandJoints = new Transform[21];
        // wrist
        rightHandJoints[0] = transform.Find("RootRight/RightHand/hand/root");
        // thumb
        rightHandJoints[1] = rightHandJoints[0].Find("R_FK_Thumb_null/R_Thumb_null/R_NB_FK_Thumb_Root_jnt");
        rightHandJoints[2] = rightHandJoints[1].Find("R_NB_FK_Thumb_Base_jnt");
        rightHandJoints[3] = rightHandJoints[2].Find("R_NB_FK_Thumb_Mid_jnt");
        rightHandJoints[4] = rightHandJoints[3].Find("R_NB_FK_Thumb_End_jnt");
        // index
        rightHandJoints[5] = rightHandJoints[0].Find("R_FK_Finger_null/R_FK_Finger_null_2_3/R_NB_FK_Finger_Palm_Base_jnt/R_NB_FK_Finger_Base_jnt");
        rightHandJoints[6] = rightHandJoints[5].Find("R_NB_FK_Finger_Mid_jnt");
        rightHandJoints[7] = rightHandJoints[6].Find("R_NB_FK_Finger_Top_jnt");
        rightHandJoints[8] = rightHandJoints[7].Find("R_NB_FK_Finger_End_jnt");
        // middle
        rightHandJoints[9] = rightHandJoints[0].Find("R_FK_Finger_null_1/R_FK_Finger_null_1_2/R_NB_FK_Finger_Palm_Base_jnt_1/R_NB_FK_Finger_Base_jnt_1");
        rightHandJoints[10] = rightHandJoints[9].Find("R_NB_FK_Finger_Mid_jnt_1");
        rightHandJoints[11] = rightHandJoints[10].Find("R_NB_FK_Finger_Top_jnt_1");
        rightHandJoints[12] = rightHandJoints[11].Find("R_NB_FK_Finger_End_jnt_1");
        // ring
        rightHandJoints[13] = rightHandJoints[0].Find("R_FK_Finger_null_2/R_FK_Finger_null_2_2/R_NB_FK_Finger_Palm_Base_jnt_2/R_NB_FK_Finger_Base_jnt_2");
        rightHandJoints[14] = rightHandJoints[13].Find("R_NB_FK_Finger_Mid_jnt_2");
        rightHandJoints[15] = rightHandJoints[14].Find("R_NB_FK_Finger_Top_jnt_2");
        rightHandJoints[16] = rightHandJoints[15].Find("R_NB_FK_Finger_End_jnt_2");
        // pinky
        rightHandJoints[17] = rightHandJoints[0].Find("R_FK_Finger_null_3/R_FK_Finger_null_3_2/R_NB_FK_Finger_Palm_Base_jnt_3/R_NB_FK_Finger_Base_jnt_3");
        rightHandJoints[18] = rightHandJoints[17].Find("R_NB_FK_Finger_Mid_jnt_3");
        rightHandJoints[19] = rightHandJoints[18].Find("R_NB_FK_Finger_Top_jnt_3");
        rightHandJoints[20] = rightHandJoints[19].Find("R_NB_FK_Finger_End_jnt_3");
    }

    private void Start()
    {
        // Store the initial rotation of the hand
        initialLeftHandRotation = leftHandJoints[0].rotation;
        initialRightHandRotation = rightHandJoints[0].rotation;
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Debug.Log("wrist: " + HandTrackingManager.Instance.HandTracking.LeftHandLandmarks[0]);
        UnityEngine.Debug.Log("index: " + HandTrackingManager.Instance.HandTracking.LeftHandLandmarks[5]);
        UnityEngine.Debug.Log("pinky: " + HandTrackingManager.Instance.HandTracking.LeftHandLandmarks[17]);
        CalculateHandMovement(leftHandJoints, HandTrackingManager.Instance.HandTracking.LeftHandLandmarks, initialLeftHandRotation);
        CalculateHandMovement(rightHandJoints, HandTrackingManager.Instance.HandTracking.RightHandLandmarks, initialRightHandRotation);
    }

    void CalculateHandMovement(Transform[] handJoints, Vector3[] landmarks, Quaternion initialHandRotation)
    {
        Vector3 wristPosition = landmarks[0];
        Vector3 indexBasePosition = landmarks[5];
        Vector3 pinkyBasePosition = landmarks[17];
        

        // // Calculate the vectors from wrist to index base and thumb base
        // Vector3 wristToIndex = indexBasePosition - wristPosition;
        // Vector3 wristToPinky = pinkyBasePosition - wristPosition;

        // // Calculate the cross product to find the normal of the triangle
        // Vector3 triangleNormal = Vector3.Cross(wristToPinky, wristToIndex).normalized;

        // // Calculate the rotation based on the triangle normal
        // Quaternion wristRotation = Quaternion.LookRotation(-triangleNormal, Vector3.up);

        // // Calculate the position of the wrist joint
        // Vector3 wristJointPosition = (wristPosition + landmarks[0]) * 0.5f;

        // // Apply the wrist joint position to the hand position
        // handJoints[0].position = wristJointPosition;

        // // Apply smoothing
        // wristRotation = Quaternion.Slerp(initialHandRotation, wristRotation, Time.deltaTime * smoothingFactor);

        // // Apply the smoothed rotation to the hand
        // handJoints[0].rotation = wristRotation;



        // // Calculate wrist rotation
        // Vector3 wristDirection = indexBasePosition - pinkyBasePosition;
        // Quaternion wristRotation = Quaternion.LookRotation(wristDirection, Vector3.down);

        // handJoints[0].rotation = wristRotation;

        wristPosition.x -= 0.5f;
        wristPosition.y += 1f;
        handJoints[0].localPosition = Vector3.Lerp(handJoints[0].transform.localPosition, wristPosition, Time.deltaTime * smoothingFactor);;
    }
}
