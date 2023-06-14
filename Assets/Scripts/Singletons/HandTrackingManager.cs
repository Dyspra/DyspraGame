using System;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using Dyspra;

/// <summary>
///     HandTrackingManager is a singleton that manages all hand tracking solutions and choose only one to use.
/// </summary>
public class HandTrackingManager : SingletonGameObject<HandTrackingManager>
{
    private List<IHandTrackingSolution> _handTrackingSolutions = new List<IHandTrackingSolution>();
    public IHandTrackingSolution handTracking;

    private void Awake()
    {
        // get all available devices
        this.GetSolutions();
        // select first device
        if (this._handTrackingSolutions.Count > 0)
        {
            UnityEngine.Debug.Log($"HandTrackingManager: {this._handTrackingSolutions.Count} solutions found.");
            this.handTracking = this._handTrackingSolutions[0];
            this.handTracking.StartTracking();
        }
    }

    private void GetSolutions()
    {
        // get all available solutions from all implementations

        // MediaPipePythonInterface
        try
        {
            var mediaPipePythonInterface = new MediaPipePythonInterface();
            _handTrackingSolutions.Add(mediaPipePythonInterface);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error while initializing MediaPipePythonInterface: {ex.Message}");
        }

        // MediaPipePlugin (is mono behaviour)
        try
        {
            var mediaPipePlugin = new MediaPipePlugin();
            _handTrackingSolutions.Add(mediaPipePlugin);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error while initializing MediaPipeUnityPlugin: {ex.Message}");
        }
    }

    public bool ChangeSelectedSolution(string id)
    {
        foreach (IHandTrackingSolution solution in this._handTrackingSolutions)
        {
            if (solution.id == id)
            {
                this.handTracking = solution;
                this.handTracking.StartTracking();
                return true;
            }
        }
        return false;
    }

    public List<IHandTrackingSolution> GetAllSolutions()
    {
        return this._handTrackingSolutions;
    }

    private void OnDestroy() {
        this.handTracking.StopTracking();
    }

    private void OnApplicationQuit() {
        this.handTracking.StopTracking();
    }

    private void OnApplicationPause(bool pauseStatus) {
        if (pauseStatus) {
            this.handTracking.StopTracking();
        } else {
            this.handTracking.StartTracking();
        }
    }
}
