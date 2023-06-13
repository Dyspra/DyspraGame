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
            this.handTracking = this._handTrackingSolutions[0];
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

        // MediaPipePlugin
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
                return true;
            }
        }
        return false;
    }

    public List<IHandTrackingSolution> GetAllSolutions()
    {
        return this._handTrackingSolutions;
    }
}
