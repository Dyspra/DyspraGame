using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class NewMovementManager : MonoBehaviour
{
    public Transform leftHand;
    public Transform[] leftHandJoints;
    public Quaternion leftHandRotation;
    public Transform rightHand;
    public Transform[] rightHandJoints;
    public Quaternion rightHandRotation;
    public float smoothingFactor = 10f;   // Smoothing factor for rotations




    private void Awake()
    {
        // Initialize left hand joints
        UnityEngine.Debug.Log("RootLeft: " + transform.Find("RootLeft/LeftHand/hand/root"));
        // hand
        leftHand = transform.Find("RootLeft");
        leftHandJoints = new Transform[21];
        // wrist
        leftHandJoints[0] = leftHand.Find("LeftHand/hand/root");
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
        rightHand = transform.Find("RootRight");
        rightHandJoints = new Transform[21];
        // wrist
        rightHandJoints[0] = rightHand.Find("RightHand/hand/root");
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

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // UnityEngine.Debug.Log("wrist: " + HandTrackingManager.Instance.HandTracking.LeftHandLandmarks[0]);
        // UnityEngine.Debug.Log("index: " + HandTrackingManager.Instance.HandTracking.LeftHandLandmarks[5]);
        // UnityEngine.Debug.Log("pinky: " + HandTrackingManager.Instance.HandTracking.LeftHandLandmarks[17]);
        ApplyHandPosition(leftHand, HandTrackingManager.Instance.HandTracking.LeftHandPosition);
        ApplyHandPosition(rightHand, HandTrackingManager.Instance.HandTracking.RightHandPosition);
        CalculateHandRotation(leftHand, HandTrackingManager.Instance.HandTracking.LeftHandLandmarks, ref leftHandRotation, true);
        CalculateHandRotation(rightHand, HandTrackingManager.Instance.HandTracking.RightHandLandmarks, ref rightHandRotation, false);
        CalculateFingersRotation(leftHandJoints, HandTrackingManager.Instance.HandTracking.LeftHandLandmarks, true);
        CalculateFingersRotation(rightHandJoints, HandTrackingManager.Instance.HandTracking.RightHandLandmarks, false);
    }

    void ApplyHandPosition(Transform hand, Vector3 handPosition)
    {
        handPosition.x -= 0.5f;
        handPosition.y += 1f;
        hand.localPosition = Vector3.Lerp(hand.transform.localPosition, handPosition, Time.deltaTime * smoothingFactor);
    }

    void CalculateHandRotation(Transform hand, Vector3[] landmarks, ref Quaternion handRotation, bool isLeft)
    {
        Vector3 wristPosition = landmarks[0];
        Vector3 indexBasePosition = landmarks[5];
        Vector3 pinkyBasePosition = landmarks[17];

        // Get perpendicular vector
        Vector3 forY = GetTrianglePerpendicular(indexBasePosition, pinkyBasePosition, wristPosition);
        Vector3 forZ = Vector3.Normalize(indexBasePosition - wristPosition);
        // // Flip the vector if it's the left hand
        if (isLeft == true)
            forY *= -1;

        // Calculate hand rotation
        handRotation = Quaternion.LookRotation(forZ, forY);
        hand.localRotation = Quaternion.Lerp(hand.localRotation, handRotation, Time.deltaTime * smoothingFactor);
    }

    Vector3 GetTrianglePerpendicular(Vector3 a, Vector3 b, Vector3 c)
    {
        // Find vectors corresponding to two of the sides of the triangle.
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;

        // Cross the vectors to get a perpendicular vector, then normalize it.
        return Vector3.Cross(side1, side2);
    }

    private void CalculateFingersRotation(Transform[] handJoints, Vector3[] landmarks, bool isLeft)
    {
        // Thumb
        CalculateFingerRotation(handJoints, landmarks, 1, 2, 3, 4, isLeft, Vector3.up);
        // Index
        CalculateFingerRotation(handJoints, landmarks, 5, 6, 7, 8, isLeft, Vector3.right);
        // Middle
        CalculateFingerRotation(handJoints, landmarks, 9, 10, 11, 12, isLeft, Vector3.right);
        // Ring
        CalculateFingerRotation(handJoints, landmarks, 13, 14, 15, 16, isLeft, Vector3.right);
        // Pinky
        CalculateFingerRotation(handJoints, landmarks, 17, 18, 19, 20, isLeft, Vector3.right);
    }

    private void CalculateFingerRotation(Transform[] handJoints, Vector3[] landmarks, int baseIndex, int midIndex, int topIndex, int endIndex, bool isLeft, Vector3 planeDirection)
    {
        // Calculate base rotation
        CalculateFingerJointRotation(handJoints, landmarks, 0, baseIndex, midIndex, isLeft, planeDirection);

        // Calculate mid rotation
        CalculateFingerJointRotation(handJoints, landmarks, baseIndex, midIndex, topIndex, isLeft, planeDirection);

        // Calculate top rotation
        CalculateFingerJointRotation(handJoints, landmarks, midIndex, topIndex, endIndex, isLeft, planeDirection);
    }

    // Bend the finger, only on X axis
    // we need to calculate the rotation of the joint based on the direction of the next joint
    // it uses hand rotation to adjust the rotation
    private void CalculateFingerJointRotation(Transform[] handJoints, Vector3[] landmarks, int previousJointIndex, int jointIndex, int nextJointIndex, bool isLeft, Vector3 planeDirection)
    {
        Vector3 previousJointPosition = landmarks[previousJointIndex];
        Vector3 jointPosition = landmarks[jointIndex];
        Vector3 nextJointPosition = landmarks[nextJointIndex];

        // Define a plane using the hand rotation
        Plane plane;
        if (isLeft)
        {
            plane = new Plane(leftHandRotation * planeDirection, jointPosition);
        }
        else
        {
            plane = new Plane(rightHandRotation * planeDirection, jointPosition);
        }

        // Project the 3D points onto the plane to convert them into 2D points
        Vector3 previousJointPosition2D = plane.ClosestPointOnPlane(previousJointPosition);
        Vector3 jointPosition2D = plane.ClosestPointOnPlane(jointPosition);
        Vector3 nextJointPosition2D = plane.ClosestPointOnPlane(nextJointPosition);

        // Calculate the vectors formed by these 2D points
        Vector3 vectorA = jointPosition2D - previousJointPosition2D;
        Vector3 vectorB = nextJointPosition2D - jointPosition2D;

        // Calculate the angle between the vectors
        Quaternion rotation = Quaternion.Euler(-Vector3.Angle(vectorA, vectorB), 0, 0);
        if (isLeft == true && jointIndex == 1)
        {
            
            // Debug.Log("VectorA: " + vectorA.ToString("F5"));
            // Debug.Log("VectorB: " + vectorB.ToString("F5"));
            // Debug.Log("============");
            Debug.Log("rotation " + jointIndex + ": " + rotation);
            // Debug.Log("previousJointPosition2D: " + previousJointPosition2D.ToString("F5"));
            // Debug.Log("jointPosition2D: " + jointPosition2D.ToString("F5"));
            // Debug.Log("nextJointPosition2D: " + nextJointPosition2D.ToString("F5"));
        }
        if (jointIndex == 1)
        {
            rotation = Quaternion.Euler(rotation.eulerAngles.x, handJoints[jointIndex].localRotation.eulerAngles.y, handJoints[jointIndex].localRotation.eulerAngles.z);
        }


        // Flip the rotation if it's the left hand
        handJoints[jointIndex].localRotation = Quaternion.Lerp(handJoints[jointIndex].localRotation, rotation, Time.deltaTime * smoothingFactor);
    }
}
