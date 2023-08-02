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
        options.SetEnvironmentName("development");
        await UnityServices.InitializeAsync(options);

        AskForConsent();
    }

    void AskForConsent()
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
        UnityServices.ExternalUserId = userId;
    }
}