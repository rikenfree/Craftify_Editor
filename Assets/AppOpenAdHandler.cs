using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AppOpenManager : MonoBehaviour
{
    public static AppOpenManager Instance;

    private AppOpenAd appOpenAd;
    private bool isLoading = false;
    private DateTime loadTime;
    private Action onAdClosed;

    [Header("Use your actual AdMob AppOpenAd ID here")]
    public string appOpenAdUnitId = "ca-app-pub-1407232796132402/9627674043"; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadAppOpenAd(Action onLoaded = null)
    {
        if (isLoading || IsAppOpenAdAvailable())
            return;

        isLoading = true;

        AdRequest request = new AdRequest();

        AppOpenAd.Load(appOpenAdUnitId, request, (AppOpenAd ad, LoadAdError error) =>
        {
            isLoading = false;

            if (error != null || ad == null)
            {
                Debug.LogWarning($"❌ Failed to load AppOpenAd: {error?.GetMessage()}");
                return;
            }

            Debug.Log("✅ AppOpenAd loaded");
            appOpenAd = ad;
            loadTime = DateTime.UtcNow;
            onLoaded?.Invoke();
        });
    }


    public bool IsAppOpenAdAvailable()
    {
        return appOpenAd != null && (DateTime.UtcNow - loadTime).TotalHours < 4;
    }

    public void ShowAppOpenAd(Action onClosed)
    {
        if (!IsAppOpenAdAvailable())
        {
            onClosed?.Invoke();
            return;
        }

        onAdClosed = onClosed;

        appOpenAd.OnAdFullScreenContentClosed += OnAdClosed;
        appOpenAd.OnAdFullScreenContentFailed += OnAdFailed;

        appOpenAd.Show();
    }

    private void OnAdClosed()
    {
        Debug.Log("🎬 AppOpenAd closed");
        appOpenAd = null;
        onAdClosed?.Invoke();
    }

    private void OnAdFailed(AdError error)
    {
        Debug.LogWarning($"❌ AppOpenAd failed to show: {error.GetMessage()}");
        appOpenAd = null;
        onAdClosed?.Invoke();
    }
}
