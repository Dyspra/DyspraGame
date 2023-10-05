using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IHandTrackingSolution {
    string id { get; }
    string displayName { get; }

    bool isTracking { get; }

    GameObject settingsPrefab { get; }

    // Must be a Vector3[] of length 21
    Vector3[] LeftHandLandmarks { get; }
    Vector3 LeftHandPosition { get; }

    // Must be a Vector3[] of length 21
    Vector3[] RightHandLandmarks { get; }
    Vector3 RightHandPosition { get; }

    Task<bool> StartTracking();
    Task<bool> StopTracking();
}
