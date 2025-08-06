//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class IronSourceAdManager2 : MonoBehaviour
//{
//    public static IronSourceAdManager Instance;
//    public string androidAppKey;
//    public string iosAppKey;






//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//#if UNITY_ANDROID
//        string appKey = androidAppKey;
//#elif UNITY_IPHONE
//        string appKey = iosAppKey;
//#else
//        string appKey = "unexpected_platform";
//#endif
//        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
//        IronSource.Agent.validateIntegration();

//        Debug.Log("unity-script: unity version" + IronSource.unityVersion());

//        // SDK init
//        Debug.Log("unity-script: IronSource.Agent.init");
//        IronSource.Agent.init(appKey);

//        Invoke("LoadInterstitialAd", 5f);

//    }

//    void OnEnable()
//    {
//        //Add Rewarded Video Events
//        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
//        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
//        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
//        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
//        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
//        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
//        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
//        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;

//        // Add Interstitial Events
//        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
//        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
//        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
//        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
//        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
//        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
//        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

//        // Add Banner Events
//        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
//        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
//        IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
//        IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
//        IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
//        IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
//    }

//    void OnApplicationPause(bool isPaused)
//    {
//        Debug.Log("unity-script: OnApplicationPause = " + isPaused);
//        IronSource.Agent.onApplicationPause(isPaused);
//    }

//    #region RewardedAd callback handlers
//    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
//    {
//        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
//    }

//    void RewardedVideoAdOpenedEvent()
//    {
//        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
//    }

//    void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
//    {
//        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());
//        GiveRewardToUser();
//    }

//    void RewardedVideoAdClosedEvent()
//    {
//        Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
//    }

//    void RewardedVideoAdStartedEvent()
//    {
//        Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
//    }

//    void RewardedVideoAdEndedEvent()
//    {
//        Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
//    }

//    void RewardedVideoAdShowFailedEvent(IronSourceError error)
//    {
//        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
//    }

//    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
//    {
//        Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
//    }
//    #endregion

//    #region Interstitial callback handlers
//    void InterstitialAdReadyEvent()
//    {
//        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
//    }

//    void InterstitialAdLoadFailedEvent(IronSourceError error)
//    {
//        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
//        Invoke("LoadInterstitialAd", 1f);
//    }

//    void InterstitialAdShowSucceededEvent()
//    {
//        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");

//    }

//    void InterstitialAdShowFailedEvent(IronSourceError error)
//    {
//        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());

//    }

//    void InterstitialAdClickedEvent()
//    {
//        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
//    }

//    void InterstitialAdOpenedEvent()
//    {
//        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
//        if (AdManager.instance.mapDataAfterInterstitial != null)
//        {
//            DownloadManager.instance.DownloadMap(AdManager.instance.mapDataAfterInterstitial);
//            AdManager.instance.mapDataAfterInterstitial = null;
//        }
//        //if (AdManager.instance.shouldSwitchToMainScene)
//        //{
//        //    WelcomeManager.instance.welcomePanel.SetActive(value: false);
//        //    AdManager.instance.shouldSwitchToMainScene = false;
//        //}
//    }

//    void InterstitialAdClosedEvent()
//    {
//        Debug.Log("unity-script: I got InterstitialAdClosedEvent");

//        if (AdManager.instance.mapDataAfterInterstitial != null)
//        {
//            DownloadManager.instance.DownloadMap(AdManager.instance.mapDataAfterInterstitial);
//            AdManager.instance.mapDataAfterInterstitial = null;
//        }
//        //if (AdManager.instance.shouldSwitchToMainScene)
//        //{
//        //    WelcomeManager.instance.welcomePanel.SetActive(value: false);
//        //    AdManager.instance.shouldSwitchToMainScene = false;
//        //}
//        Invoke("LoadInterstitialAd", 5f);

//        if (BannerShowCount == 0)
//        {
//            InvokeRepeating("ShowBannerAd", 1, 60);
//        }
//    }
//    #endregion

//    #region Banner callback handlers
//    void BannerAdLoadedEvent()
//    {
//        Debug.Log("unity-script: I got BannerAdLoadedEvent");
//    }

//    void BannerAdLoadFailedEvent(IronSourceError error)
//    {
//        Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
//    }

//    void BannerAdClickedEvent()
//    {
//        Debug.Log("unity-script: I got BannerAdClickedEvent");
//    }

//    void BannerAdScreenPresentedEvent()
//    {
//        Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
//    }

//    void BannerAdScreenDismissedEvent()
//    {
//        Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
//    }

//    void BannerAdLeftApplicationEvent()
//    {
//        Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
//    }
//    #endregion

//    public int BannerShowCount = 0;
//    public void ShowBannerAd()
//    {
//        Debug.Log("Banner ad load");
//        if (BannerShowCount > 0)
//        {
//            HideBannerAd();
//        }
//        BannerShowCount += 1;
//        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.TOP);
//    }

//    public void HideBannerAd()
//    {
//        Debug.Log("Hide Banner Ad");
//        IronSource.Agent.destroyBanner();
//    }

//    public void LoadInterstitialAd()
//    {
//        Debug.Log("Load Interstitial AD");
//        IronSource.Agent.loadInterstitial();
//    }

//    public void ShowInterstitial()
//    {
//        Debug.Log("Show Intrestitial Ad");
//        if (IronSource.Agent.isInterstitialReady())
//        {
//            IronSource.Agent.showInterstitial();
//            AdManager.instance.lastShowTime = Time.time;
//        }
//        else
//        {
//            Invoke("LoadInterstitialAd", 1f);

//        }
//    }


//    string VideoFor = "";
//    public void ShowRewardVideo(string _rewardFor = "test")
//    {
//        Debug.Log("Show Reward video Ad");
//        VideoFor = _rewardFor;
//        if (IronSource.Agent.isRewardedVideoAvailable())
//        {
//            IronSource.Agent.showRewardedVideo();
//        }
//        else
//        {

//        }
//    }

//    void GiveRewardToUser()
//    {
//        Debug.LogError("watch video comepleted:" + VideoFor);
//        switch (VideoFor)
//        {
//            case "ButterflyTap":

//                break;
//            case "ExtraSpin":

//                break;
//            case "Add30Secound":

//                break;
//            case "NoMoreLife":

//                break;
//            case "BuyMoreLife":

//                break;
//            case "SurpriseGift20Coin":

//                break;
//            case "test":
//                Debug.LogError("this ad is for test");
//                break;
//            default:
//                break;
//        }
//    }
//}