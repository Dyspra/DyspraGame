using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using NaughtyAttributes;
using UnityEngine;
// using UnityEngine.UI;


public class HandVisualize : MonoBehaviour
{
    private GameObject LeftVisualizeContainer;
    private GameObject LeftHandBallsContainer;
    private GameObject[] leftHandBalls;
    private GameObject leftPlane;
    private GameObject RightVisualizeContainer;
    private GameObject RightHandBallsContainer;
    private GameObject[] rightHandBalls;
    private GameObject rightPlane;
    private NewMovementManager MovementManager;
    
    public float smoothingFactor = 10f;

    [Button("Toggle Balls", EButtonEnableMode.Playmode)]
    private void ToggleBalls()
    {
        LeftHandBallsContainer.SetActive(!LeftHandBallsContainer.activeSelf);
        RightHandBallsContainer.SetActive(!RightHandBallsContainer.activeSelf);
    }

    [Button("Toggle Plane", EButtonEnableMode.Playmode)]
    private void TogglePlane()
    {
        leftPlane.SetActive(!leftPlane.activeSelf);
        rightPlane.SetActive(!rightPlane.activeSelf);
    }

    void Start()
    {
        MovementManager = this.GetComponent<NewMovementManager>();
        LeftVisualizeContainer = InitContainer("LeftVisualizeContainer");
        LeftHandBallsContainer = InitContainer("LeftHandBallsContainer", LeftVisualizeContainer.transform);
        LeftHandBallsContainer.SetActive(false);
        RightVisualizeContainer = InitContainer("RightVisualizeContainer");
        RightHandBallsContainer = InitContainer("RightHandBallsContainer", RightVisualizeContainer.transform);
        RightHandBallsContainer.SetActive(false);
        InitHandBalls(ref leftHandBalls, LeftHandBallsContainer);
        InitHandPlane(ref leftPlane, LeftVisualizeContainer);
        leftPlane.SetActive(false);
        InitHandBalls(ref rightHandBalls, RightHandBallsContainer);
        InitHandPlane(ref rightPlane, RightVisualizeContainer);
        rightPlane.SetActive(false);
    }

    private GameObject InitContainer(string containerName, Transform parent = null)
    {
        GameObject container = new GameObject(containerName);
        if (parent != null)
        {
            container.transform.parent = parent;
        }
        else
        {
            container.transform.parent = this.transform;
        }
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

    private void InitHandPlane(ref GameObject plane, GameObject parent)
    {
        plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        plane.transform.parent = parent.transform;
        plane.transform.localPosition = new Vector3(0, 0, 0);
        plane.transform.localScale = new Vector3(0.3f, 0.3f, 0.0001f);
        plane.transform.localRotation = Quaternion.Euler(90, 0, 0);
        plane.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Update is called once per frame
    public void Update()
    {
        if (LeftHandBallsContainer.activeSelf)
        {
            ApplyHandPosition(LeftHandBallsContainer.transform, HandTrackingManager.Instance.HandTracking.LeftHandPosition);
            ApplyBallsPosition(leftHandBalls, HandTrackingManager.Instance.HandTracking.LeftHandLandmarks);
        }
        if (RightHandBallsContainer.activeSelf)
        {
            ApplyHandPosition(RightHandBallsContainer.transform, HandTrackingManager.Instance.HandTracking.RightHandPosition);
            ApplyBallsPosition(rightHandBalls, HandTrackingManager.Instance.HandTracking.RightHandLandmarks);
        }

        if (leftPlane.activeSelf)
        {
            UpdateHandPlane(leftPlane, MovementManager.leftHandRotation, HandTrackingManager.Instance.HandTracking.LeftHandPosition);
            UpdateHandPlane(rightPlane, MovementManager.rightHandRotation, HandTrackingManager.Instance.HandTracking.RightHandPosition);
        }
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
        hand.localPosition = Vector3.Lerp(hand.transform.localPosition, handPosition, Time.deltaTime * smoothingFactor);
    }

    private void UpdateHandPlane(GameObject plane, Quaternion handRotation, Vector3 handPosition)
    {
        plane.transform.localRotation = Quaternion.Lerp(plane.transform.localRotation, handRotation, Time.deltaTime * smoothingFactor);
        Vector3 planePosition = handPosition;
        planePosition.x -= 1f;
        planePosition.y += 2f;
        plane.transform.localPosition = Vector3.Lerp(plane.transform.localPosition, planePosition, Time.deltaTime * smoothingFactor);
    }
}
