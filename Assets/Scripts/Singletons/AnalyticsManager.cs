using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using Unity.Services.Core.Environments;	

public class AnalyticsManager : SingletonGameObject<AnalyticsManager>
{
    async void Start()
    {
        var options = new InitializationOptions();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        options.SetEnvironmentName("development");
#else
        options.SetEnvironmentName("production");
#endif
        await UnityServices.InitializeAsync(options);

        AskForConsent();
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

    /// <summary>
    ///  Log an event with a name and a dictionary of parameters.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventData"></param>
    public void LogEvent(string eventName, Dictionary<string, object> eventData)
    {
        UnityEngine.Debug.Log("LogEvent: " + eventName);
        AnalyticsService.Instance.SendCustomEvent(eventName, eventData);
    }

    // Built-in custom events:

    /// <summary>
    /// Log when the player starts an exercise.
    /// </summary>
    /// <param name="levelId"></param>
    public void LogExerciseStarted(string levelId)
    {
        LogEvent("exerciseStarted", new Dictionary<string, object> {
            { "levelId", levelId },
            { "date", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") }
        });
    }

    /// <summary>
    /// Log when the player stops an exercise.
    /// </summary>
    /// <param name="levelId">The ID of the exercise level.</param>
    /// <param name="score">The score achieved by the player.</param>
    /// <param name="reason">The reason for stopping the exercise. Can be "quit", "complete", or "failed".</param>
    public void LogExerciseStop(string levelId, int score, string reason)
    {
        LogEvent("exerciseStop", new Dictionary<string, object> {
            { "levelId", levelId },
            { "score", score },
            { "reason", reason },
            { "date", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") }
        });
    }
}