using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;
// using UnityEngine.UI;


public class HandVisualize : MonoBehaviour
{
    private GameObject LeftHandBallsContainer;
    private GameObject[] leftHandBalls;
    private GameObject leftPlane;
    private GameObject RightHandBallsContainer;
    private GameObject[] rightHandBalls;
    private GameObject rightPlane;
    private NewMovementManager MovementManager;
    
    public float smoothingFactor = 10f;
    // Start is called before the first frame update

    void Start()
    {
        MovementManager = this.GetComponent<NewMovementManager>();
        LeftHandBallsContainer = InitContainer("LeftVisualizeContainer");
        RightHandBallsContainer = InitContainer("RightVisualizeContainer");
        InitHandBalls(ref leftHandBalls, LeftHandBallsContainer);
        InitHandPlane(ref leftPlane, LeftHandBallsContainer);
        InitHandBalls(ref rightHandBalls, RightHandBallsContainer);
        InitHandPlane(ref rightPlane, RightHandBallsContainer);
        // InitWebCamScreen();
    }

    private GameObject InitContainer(string containerName)
    {
        GameObject container = new GameObject(containerName);
        container.transform.parent = this.transform;
        container.transform.localPosition = new Vector3(0, 0, 0);
        return container;
    }

    private void InitHandBalls(ref GameObject[] handBalls, GameObject handBallsContainer)
    {
        handBalls = new GameObject[21];
        for (int i = 0; i < handBalls.Length; i++)
        {
            handBalls[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            handBalls[i].transform.localPosition = new Vector3(0, 0, 0);
            handBalls[i].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            handBalls[i].transform.parent = handBallsContainer.transform;
        }
    }

    private void InitHandPlane(ref GameObject plane, GameObject handBallsContainer)
    {
        plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        plane.transform.parent = handBallsContainer.transform;
        plane.transform.localPosition = new Vector3(0, 0, 0);
        plane.transform.localScale = new Vector3(0.3f, 0.3f, 0.0001f);
        plane.transform.localRotation = Quaternion.Euler(90, 0, 0);
        plane.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Update is called once per frame
    public void Update()
    {
        ApplyHandPosition(LeftHandBallsContainer.transform, HandTrackingManager.Instance.HandTracking.LeftHandPosition);
        ApplyBallsPosition(leftHandBalls, HandTrackingManager.Instance.HandTracking.LeftHandLandmarks);
        UpdateHandPlane(leftPlane, MovementManager.leftHandRotation, HandTrackingManager.Instance.HandTracking.LeftHandPosition);

        ApplyHandPosition(RightHandBallsContainer.transform, HandTrackingManager.Instance.HandTracking.RightHandPosition);
        ApplyBallsPosition(rightHandBalls, HandTrackingManager.Instance.HandTracking.RightHandLandmarks);
        UpdateHandPlane(rightPlane, MovementManager.rightHandRotation, HandTrackingManager.Instance.HandTracking.RightHandPosition);
    }

    private void ApplyBallsPosition(GameObject[] handBalls, Vector3[] handLandmarks)
    {
        for (int i = 0; i < handBalls.Length; i++)
        {
            Vector3 handLandmark = handLandmarks[i];
            handLandmark.x -= 0.5f;
            handLandmark.y += 1f;
            handBalls[i].transform.localPosition = Vector3.Lerp(handBalls[i].transform.localPosition, handLandmark, Time.deltaTime * smoothingFactor);
        }
    }

    private void ApplyHandPosition(Transform hand, Vector3 handPosition)
    {
        // handPosition.x -= 0.5f;
        // handPosition.y += 1f;
        hand.localPosition = Vector3.Lerp(hand.transform.localPosition, handPosition, Time.deltaTime * smoothingFactor);
    }

    // plane
    // Quaternion handRotation 
    // handPosition
    private void UpdateHandPlane(GameObject plane, Quaternion handRotation, Vector3 handPosition)
    {
        plane.transform.localRotation = Quaternion.Lerp(plane.transform.localRotation, handRotation, Time.deltaTime * smoothingFactor);
        Vector3 planePosition = handPosition;
        planePosition.x -= 1f;
        planePosition.y += 2f;
        plane.transform.localPosition = Vector3.Lerp(plane.transform.localPosition, planePosition, Time.deltaTime * smoothingFactor);
    }
}
