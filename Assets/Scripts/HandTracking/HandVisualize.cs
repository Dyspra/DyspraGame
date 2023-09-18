using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVisualize : MonoBehaviour
{
    private GameObject[] leftHandBalls;
    private GameObject[] rightHandBalls;
    public float smoothingFactor = 10f;
    // Start is called before the first frame update
    void Start()
    {
        leftHandBalls = new GameObject[21];
        var LeftHandBallsContainer = new GameObject("LeftHandBalls");
        LeftHandBallsContainer.transform.parent = this.transform;
        LeftHandBallsContainer.transform.localPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < leftHandBalls.Length; i++)
        {
            leftHandBalls[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leftHandBalls[i].transform.localPosition = new Vector3(0, 0, 0);
            leftHandBalls[i].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            leftHandBalls[i].transform.parent = LeftHandBallsContainer.transform;
        }

        // Initialize right hand balls
        rightHandBalls = new GameObject[21];
        var RightHandBallsContainer = new GameObject("RightHandBalls");
        RightHandBallsContainer.transform.parent = this.transform;
        RightHandBallsContainer.transform.localPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < rightHandBalls.Length; i++)
        {
            rightHandBalls[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            rightHandBalls[i].transform.localPosition = new Vector3(0, 0, 0);
            rightHandBalls[i].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            rightHandBalls[i].transform.parent = RightHandBallsContainer.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = transform.position;

        for (int i = 0; i < leftHandBalls.Length; i++)
        {
            Vector3 leftHandLandmark = HandTrackingManager.Instance.HandTracking.LeftHandLandmarks[i];
            // decale x, y to -1
            leftHandLandmark.x -= 0.5f;
            leftHandLandmark.y += 1f;
            leftHandBalls[i].transform.localPosition = Vector3.Lerp(leftHandBalls[i].transform.localPosition, leftHandLandmark, Time.deltaTime * smoothingFactor);
        }

        for (int i = 0; i < rightHandBalls.Length; i++)
        {
            Vector3 rightHandLandmark = HandTrackingManager.Instance.HandTracking.RightHandLandmarks[i];
            rightHandLandmark.x -= 0.5f;
            rightHandLandmark.y += 1f;
            rightHandBalls[i].transform.localPosition = Vector3.Lerp(rightHandBalls[i].transform.localPosition, rightHandLandmark, Time.deltaTime * smoothingFactor);
        }
    }
}
