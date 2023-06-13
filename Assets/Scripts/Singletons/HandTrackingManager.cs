using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using Dyspra;

/// <summary>
///     HandTrackingManager is a singleton that manages all hand tracking solutions and choose only one to use.
/// </summary>
public class HandTrackingManager : SingletonGameObject<HandTrackingManager>
{
    private List<HandTrackingSolution> _handTrackingSolutions = new List<HandTrackingSolution>();
    public HandTrackingSolution handTracking;

    private void Awake()
    {
        // get all available devices
        this.GetSolutions();
        // select first device
        if (this._handTrackingSolutions.Count > 0)
        {
            this.device = this._connectedDevices[0];
        }
    }

    private void GetSolutions()
    {
        // get all available devices from all implementations

        // todo: MediapipePythonInterface

        // todo: MediaPipeUnityPlugin
    }

    public bool ChangeSelectedSolution(string id)
    {
        foreach (HandTrackingSolution solution in this._handTrackingSolutions)
        {
            if (solution.id == id)
            {
                this.handTracking = solution;
                return true;
            }
        }
        return false;
    }

    public List<HandTrackingSolution> GetAllSolutions()
    {
        return this._handTrackingSolutions;
    }
}
