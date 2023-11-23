using System;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.SceneManagement;
using Dyspra;

/// <summary>
///     HandTrackingManager is a singleton that manages all hand tracking solutions and choose only one to use.
/// </summary>
public class HandTrackingManager : SingletonGameObject<HandTrackingManager>
{
    private List<IHandTrackingSolution> _handTrackingSolutions = new List<IHandTrackingSolution>();
    public IHandTrackingSolution HandTracking;

    private void Awake()
    {
        // get all available devices
        this.GetSolutionsImplementations();
        // select first device
        if (this._handTrackingSolutions.Count > 0)
        {
            this.HandTracking = this._handTrackingSolutions[0];
            this.HandTracking.StartTracking();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // HOTFIX: restart the handtracking on each scene change, because the handtracking is not working after a scene change
        if (this.HandTracking != null)
        {
            this.HandTracking.StopTracking();
            this.HandTracking.StartTracking();
        }
    }
    private void GetSolutionsImplementations()
    {
        // get all available solutions from all implementations

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
                this.HandTracking.StopTracking();
                this.HandTracking = solution;
                this.HandTracking.StartTracking();
                AnalyticsManager.Instance.LogS_ChangeHandTrackingSolution(solution.id);
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
        this.HandTracking.StopTracking();
    }

    private void OnApplicationQuit() {
        this.HandTracking.StopTracking();
    }

    private void OnApplicationPause(bool pauseStatus) {
        if (pauseStatus == true && this.HandTracking.isTracking) {
            this.HandTracking.StopTracking();
        } else if (pauseStatus == false && !this.HandTracking.isTracking) {
            this.HandTracking.StartTracking();
        }
    }
}
