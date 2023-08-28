using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Analytics;
using Unity.Services.Core.Environments;	
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Constants;

public class AnalyticsManager : SingletonGameObject<AnalyticsManager>
{
    Task UnityServicesInitializationTask;
    async void Start()
    {
        UnityServicesInitializationTask = InitializeUnityServices();
        await UnityServicesInitializationTask;

        AskForConsent();
    }

    private Task InitializeUnityServices()
    {
        var options = new InitializationOptions();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        options.SetEnvironmentName("development");
#else
        options.SetEnvironmentName("production");
#endif
        return UnityServices.InitializeAsync(options);
    }
    private void AskForConsent()
    {
        // yes yes very consent
        ConsentGivent();
    }

    public void ConsentGivent()
    {
        AnalyticsService.Instance.StartDataCollection();
    }

    public void SetUserId(string userId)
    {
        UnityEngine.Debug.Log("SetUserId: " + userId);
        UnityServices.ExternalUserId = userId;
    }

    // detect when a scene is loaded
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UnityEngine.Debug.Log("OnSceneLoaded: " + scene.name);
        UnityEngine.Debug.Log(mode);
        LogEvent("sceneLoaded", new Dictionary<string, object> {
            { "sceneName", scene.name },
            { "sceneMode", mode.ToString() }
        });
    }

    /// <summary>
    ///  Log an event with a name and a dictionary of parameters.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventData"></param>
    private async Task LogEvent(string eventName, Dictionary<string, object> eventData)
    {
        UnityEngine.Debug.Log("LogEvent: " + eventName);
        UnityServicesInitializationTask ??= InitializeUnityServices();
        await UnityServicesInitializationTask;
        AnalyticsService.Instance.CustomData(eventName, eventData);
    }

    // ============= Built-in custom events:

    /// <summary>
    /// Log when the player starts an exercise.
    /// </summary>
    /// <param name="levelId"></param>
    public void LogExerciseStarted(string exerciseId)
    {
        LogEvent("exerciseStarted", new Dictionary<string, object> {
            { "exerciseId", exerciseId }
        });
    }

    /// <summary>
    /// Log when the player stops an exercise.
    /// </summary>
    /// <param name="levelId">The ID of the exercise level.</param>
    /// <param name="score">The score achieved by the player.</param>
    /// <param name="reason">The reason for stopping the exercise. Can be "quit", "complete", or "failed".</param>
    public void LogExerciseStop(string levelId, int score, ExerciseConstants.E_QuitReason reason)
    {
        LogEvent("exerciseStop", new Dictionary<string, object> {
            { "exerciseId", levelId },
            { "userScore", score },
            { "stopReason", reason.ToString() }
        });
    }

    public void LogOpenSettings()
    {
        LogEvent("openSettings", new Dictionary<string, object> {
        });
    }

    /// <summary>
    /// Logs when the player changes the screen mode.
    /// </summary>
    /// <param name="screenMode">The screen mode. Can be "FULLSCREEN" or "WINDOWED".</param>
    public void LogS_ChangeScreenMode(string screenMode)
    {
        LogEvent("S_changeScreenMode", new Dictionary<string, object> {
            { "screenMode", screenMode },
        });
    }

    public void LogS_ChangeScreenResolution(int screenWidth, int screenHeight)
    {
        LogEvent("S_changeScreenResolution", new Dictionary<string, object> {
            { "screenWidth", screenWidth },
            { "screenHeight", screenHeight }
        });
    }

    public void LogS_ChangeVolume(string volumeType, float volume)
    {
        LogEvent("S_changeVolume", new Dictionary<string, object> {
            { "volumeType", volumeType },
            { "volume", volume }
        });
    }
    public void LogS_ChangeQuality(string quality)
    {
        LogEvent("S_changeQuality", new Dictionary<string, object> {
            { "graphicQuality", quality }
        });
    }

    public void LogS_ChangeFPSLimit(int fpsLimit)
    {
        LogEvent("S_changeFPSLimit", new Dictionary<string, object> {
            { "fpsLimit", fpsLimit }
        });
    }

    public void LogS_ChangeHandTrackingSolution(string handTrackingSolution)
    {
        LogEvent("S_changeHandTrackingSolution", new Dictionary<string, object> {
            { "handTrackingSolution", handTrackingSolution }
        });
    }

    public void LogOpenExerciseList()
    {
        LogEvent("openExerciseList", new Dictionary<string, object> {
        });
    }

    public void LogOpenExerciseHistory()
    {
        LogEvent("openExerciseHistory", new Dictionary<string, object> {
        });
    }

    public void LogEx1_CalibrationStarted()
    {
        LogEvent("ex1_calibrationStart", new Dictionary<string, object> {
        });
    }

    public void LogEx1_CalibrationFinished()
    {
        LogEvent("ex1_calibrationFinish", new Dictionary<string, object> {
        });
    }

    public void LogEx1_TutorialStarted()
    {
        LogEvent("ex1_tutorialStart", new Dictionary<string, object> {
        });
    }

    public void LogEx1_TutorialFinished()
    {
        LogEvent("ex1_tutorialFinish", new Dictionary<string, object> {
        });
    }

    public void LogEx1_StepFinished(int step, int score)
    {
        LogEvent("ex1_stepFinish", new Dictionary<string, object> {
            { "exerciseStep", step },
            { "userScore", score }
        });
    }

    public void LogExerciseReplay(string exerciseId)
    {
        LogEvent("exerciseReplay", new Dictionary<string, object> {
            { "exerciseId", exerciseId }
        });
    }

    public void LogExerciseQuitAfterFinished(string exerciseId)
    {
        LogEvent("exerciseQuitAfterFinished", new Dictionary<string, object> {
            { "exerciseId", exerciseId }
        });
    }
}