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
        this.GetSolutionsImplementations();
        // select first device
        if (this._handTrackingSolutions.Count > 0)
        {
            this.handTracking = this._handTrackingSolutions[0];
            this.handTracking.StartTracking();
        }
    }

    private void GetSolutionsImplementations()
    {
        // get all available solutions from all implementations

        // MediaPipePythonInterface
        try
        {
            var mediaPipePythonInterface = new GameObject("MediaPipePythonInterface").AddComponent<MediaPipePythonInterface>();
            mediaPipePythonInterface.transform.parent = this.transform;
            _handTrackingSolutions.Add(mediaPipePythonInterface);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error while initializing MediaPipePythonInterface: {ex.Message}");
        }


        // MediaPipePlugin
        try
        {
            var mediaPipePlugin = new GameObject("MediaPipePlugin").AddComponent<MediaPipePlugin>();
            mediaPipePlugin.transform.parent = this.transform;
            _handTrackingSolutions.Add(mediaPipePlugin);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error while initializing MediaPipeUnityPlugin: {ex.Message}");
        }
    }

    public IHandTrackingSolution GetSolution(string id)
    {
        foreach (IHandTrackingSolution solution in this._handTrackingSolutions)
        {
            if (solution.id == id)
            {
                return solution;
            }
        }
        return null;
    }

    public bool ChangeSelectedSolution(string id)
    {
        foreach (IHandTrackingSolution solution in this._handTrackingSolutions)
        {
            if (solution.id == id)
            {
                this.handTracking.StopTracking();
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
        if (pauseStatus == true && this.handTracking.isTracking) {
            this.handTracking.StopTracking();
        } else if (pauseStatus == false && !this.handTracking.isTracking) {
            this.handTracking.StartTracking();
        }
    }
}
