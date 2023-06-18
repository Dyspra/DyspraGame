using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public interface IHandTrackingSolution {
    string id { get; }
    string displayName { get; }

    bool isTracking { get; }

    GameObject settingsPrefab { get; }

    Task<bool> StartTracking();
    Task<bool> StopTracking();
}
