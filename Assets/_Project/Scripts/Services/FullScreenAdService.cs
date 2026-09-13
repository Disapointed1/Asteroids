using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class FullScreenAdService : MonoBehaviour
{
    private const string AdUnitId = "ca-app-pub-3940256099942544/1033173712";
    private InterstitialAd _interstitialAd;

    private void Start()
    {
        MobileAds.Initialize(initStatus => { LoadAd(); });
    }

    public event Action OnAdClosed;

    private void LoadAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        var request = new AdRequest();

        InterstitialAd.Load(AdUnitId, request, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Interstitial ad failed to load: " + error);
                return;
            }

            _interstitialAd = ad;
            _interstitialAd.OnAdFullScreenContentClosed += HandleAdClosed;
        });
    }

    public void ShowAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
            _interstitialAd.Show();
        else
            OnAdClosed?.Invoke();
    }

    private void HandleAdClosed()
    {
        OnAdClosed?.Invoke();
        LoadAd();
    }
}