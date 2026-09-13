using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseAnalyticsService
{
    private const string GameStartedEvent = "game_started";
    private const string GameOverEvent = "game_over";

    private bool _isReady;

    public FirebaseAnalyticsService()
    {
        InitializeAsync().Forget();
    }

    private async UniTaskVoid InitializeAsync()
    {
        var status = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
        if (status == DependencyStatus.Available)
        {
            _isReady = true;
            LogEvent(GameStartedEvent);
        }
        else
        {
            Debug.LogError($"Firebase dependencies not available: {status}");
        }
    }

    public void LogGameOver()
    {
        LogEvent(GameOverEvent);
    }

    private void LogEvent(string eventName)
    {
        if (!_isReady)
            return;
        FirebaseAnalytics.LogEvent(eventName);
    }
}