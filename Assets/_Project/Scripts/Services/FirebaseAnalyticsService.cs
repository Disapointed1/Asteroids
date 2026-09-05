using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseAnalyticsService
{
    private bool _isReady;

    public FirebaseAnalyticsService()
    {
        InitializeAsync().Forget();
    }

    private async UniTaskVoid InitializeAsync()
    {
        DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
        if (status == DependencyStatus.Available)
        {
            _isReady = true;
        }
        else
        {
            Debug.LogError($"Firebase dependencies not available: {status}");
        }

    }

    public void LogEvent(string eventName)
    {
        if (!_isReady)
            return;
        FirebaseAnalytics.LogEvent(eventName);
    }

}
