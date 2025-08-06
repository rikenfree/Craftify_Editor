using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using SuperStarSdk;
using GoogleMobileAds.Common;

public class AdmobManager : MonoBehaviour
{

    public bool isTestAdmobAds = false;
    List<string> TestAAdmobBID = new List<string>() { "ca-app-pub-3940256099942544/6300978111" };
    List<string> TestAAdmobInID = new List<string>() { "ca-app-pub-3940256099942544/1033173712" };
    List<string> TestAAdmobReID = new List<string>() { "ca-app-pub-3940256099942544/5224354917" };
    List<string> TestAAdmobOpenID = new List<string>() { "ca-app-pub-3940256099942544/9257395921" };
    List<string> TestAAdmobNativeID = new List<string>() { "ca-app-pub-3940256099942544/2247696110" };

    List<string> TestIAdmobBID = new List<string>() { "ca-app-pub-3940256099942544/2934735716" };
    List<string> TestIAdmobInID = new List<string>() { "ca-app-pub-3940256099942544/4411468910" };
    List<string> TestIAdmobReID = new List<string>() { "ca-app-pub-3940256099942544/1712485313" };
    List<string> TestIAdmobOpenID = new List<string>() { "ca-app-pub-3940256099942544/5575463023" };
    List<string> TestIAdmobNativeID = new List<string>() { "ca-app-pub-3940256099942544/3986624511" };

    public static AdmobManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

    }

    private void OnDestroy()
    {
        // Always unlisten to events when complete

        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;

    }
    BannerView _bannerView;
  //  public BannerView bannerView;
    public InterstitialAd interstitial;
    public RewardedAd rewardedAd;
    // public string gameId;
    //[Header("Android")]
    //public string BannerIdAndroid;
    //public string InterstitialIdAndroid;
    //public string RewardVideoIdAndroid;
    //[Header("ios")]
    //public string BannerIdios;
    //public string InterstitialIdios;
    //public string RewardVideoIdios;
    bool isNetConnected = false;


    //[Header("test ids android")]
    //public string testBannerIdAndroid;
    //public string testInterstitialIdAndroid;
    //public string testRewardVideoIdAndroid;

    //[Header("test ids ios")]
    //public string testBannerIdios;
    //public string testInterstitialIdios;
    //public string testRewardVideoIdios;
    //public bool isTestMode;

    public List<string> BannerAdsIds = new List<string>();
    public List<string> IntrestitialAdsIds = new List<string>();
    public List<string> RewardAdsIds = new List<string>();
    public List<string> AppOpenAdsIds = new List<string>();
    public List<string> AppNativeAdsIds = new List<string>();


    public int BannerIdIndex = 0;
    public int IntrestitialIdIndex = 0;
    public int RewardIdIndex = 0;
    public int AppOpenIdIndex = 0;
    public int AppNativeIdIndex = 0;

    //[Header("coin reward")]
    //DateTime startTime;
    //TimeSpan currentTime;
    //TimeSpan hour = new TimeSpan(4, 0, 0);

    public Action SetupAds;

    void Start()
    {

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => { });
        InvokeRepeating("CheckNet", 1, 10);

        //   StartTime();
    }

    public void SetUp()
    {

        if (isTestAdmobAds)
        {
#if UNITY_ANDROID
            AppOpenAdsIds = TestAAdmobOpenID;
            BannerAdsIds = TestAAdmobBID;
            IntrestitialAdsIds = TestAAdmobInID;
            RewardAdsIds = TestAAdmobReID;
            AppNativeAdsIds = TestAAdmobNativeID;
#else
            AppOpenAdsIds = TestIAdmobOpenID;
            BannerAdsIds = TestIAdmobBID;
            IntrestitialAdsIds = TestIAdmobInID;
            RewardAdsIds = TestIAdmobReID;
            AppNativeAdsIds= TestIAdmobNativeID;
#endif
        }
        else
        {

            AppOpenAdsIds = SuperStarSdkManager.Instance.crossPromoAssetsRoot.AdmobOpenID;
            BannerAdsIds = SuperStarSdkManager.Instance.crossPromoAssetsRoot.AdmobBID;
            IntrestitialAdsIds = SuperStarSdkManager.Instance.crossPromoAssetsRoot.AdmobInID;
            RewardAdsIds = SuperStarSdkManager.Instance.crossPromoAssetsRoot.AdmobReID;
            AppNativeAdsIds = SuperStarSdkManager.Instance.crossPromoAssetsRoot.AdmobReID;

        }



        AppOpenAdsIds.RemoveAll(item => item == "");
        BannerAdsIds.RemoveAll(item => item == "");
        IntrestitialAdsIds.RemoveAll(item => item == "");
        RewardAdsIds.RemoveAll(item => item == "");

        // RequestBanner();
        //UnHideBannerAd();
        RequestAppOpenAds();
        LoadInterstitialAd();
        RequestRewardAd();
        SetupAds?.Invoke();
        //if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_ads == 1)
        //{

        //   ShowAppOpenAdIfAvailable();

        //}
    }


    void CheckNet()
    {
        StartCoroutine(NetCheckerCoroutine());
        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    if (isNetConnected)
        //    {
        //        isNetConnected = false;
        //    }
        //}
        //else
        //{
        //    if (!isNetConnected)
        //    {
        //        isNetConnected = true;
        //        ReloadAds();
        //    }
        //}
    }
    IEnumerator NetCheckerCoroutine()
    {
        //Debug.LogError("net checker coroutine");
        WWW req = new WWW("https://www.google.com/");
        yield return req;
        if (req.error != null)
        {
            isNetConnected = false;
            //Debug.LogError("No Internet");
        }
        else if (req.isDone)
        {
            if (!isNetConnected)
            {
                isNetConnected = true;
                ReloadAds();
                //Debug.LogError("Internet connected");
            }
        }
    }
    void ReloadAds()
    {
        //if (PlayerPrefs.GetInt("RemoveAds", 0) == 0)
        //{
        //    InvokeRepeating("RequestBanner", 1, 35);
        //    Invoke("RequestInterstitial", 2.5f);
        //}
        // Advertisement.Initialize(gameId, isTestMode);

        // InvokeRepeating("RequestBanner", 1, 50);
        Invoke("LoadInterstitialAd", 5f);
        //Invoke("RequestInterstitial", 5f);
        Invoke("RequestRewardAd", 5);

    }

    public void RequestBanner(AdPosition adpos)
    {
        if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_banner == 0)
        {
            return;
        }

        if (BannerIdIndex >= BannerAdsIds.Count)
        {
            return;
        }
        //Debug.LogError("Banner load");
        string adUnitId = BannerAdsIds[BannerIdIndex];

        if (_bannerView != null)
        {
            DestroyBannerView();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(adUnitId, AdSize.Banner, adpos);


        //if (_bannerView == null)
        //{
        //    CreateBannerView();
        //}

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
       
        _bannerView.LoadAd(adRequest);
    }

    public void RequestAdaptiveBanner(AdPosition adpos)
    {
        if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_banner == 0)
        {
            return;
        }

        if (BannerIdIndex >= BannerAdsIds.Count)
        {
            return;
        }
        //Debug.LogError("Banner load");
        string adUnitId = BannerAdsIds[BannerIdIndex];

        if (_bannerView != null)
        {
            DestroyBannerView();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);


        //if (_bannerView == null)
        //{
        //    CreateBannerView();
        //}

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        adRequest.Extras.Add("collapsible", "bottom");
        _bannerView.LoadAd(adRequest);
    }



    //private void ListenToAdEvents()
    //{
    //    // Raised when an ad is loaded into the banner view.
    //    _bannerView.OnBannerAdLoaded += () =>
    //    {
    //        Debug.Log("Banner view loaded an ad with response : "
    //            + _bannerView.GetResponseInfo());
    //    };
    //    // Raised when an ad fails to load into the banner view.
    //    _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
    //    {
    //        Debug.LogError("Banner view failed to load an ad with error : "
    //            + error);
    //    };
    //    // Raised when the ad is estimated to have earned money.
    //    _bannerView.OnAdPaid += (AdValue adValue) =>
    //    {
    //        Debug.Log(String.Format("Banner view paid {0} {1}.",
    //            adValue.Value,
    //            adValue.CurrencyCode));
    //    };
    //    // Raised when an impression is recorded for an ad.
    //    _bannerView.OnAdImpressionRecorded += () =>
    //    {
    //        Debug.Log("Banner view recorded an impression.");
    //    };
    //    // Raised when a click is recorded for an ad.
    //    _bannerView.OnAdClicked += () =>
    //    {
    //        Debug.Log("Banner view was clicked.");
    //    };
    //    // Raised when an ad opened full screen content.
    //    _bannerView.OnAdFullScreenContentOpened += () =>
    //    {
    //        Debug.Log("Banner view full screen content opened.");
    //    };
    //    // Raised when the ad closed full screen content.
    //    _bannerView.OnAdFullScreenContentClosed += () =>
    //    {
    //        Debug.Log("Banner view full screen content closed.");
    //    };
    //}

    public void DestroyBannerView()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
    public void DestrotyBannerAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    public void HideBannerAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Hide();
           
        }
    }

    public void UnHideBannerAd()
    {
        if (_bannerView != null)
            _bannerView.Show();
    }

    private AppOpenAd ad;

    private bool isShowingAd = false;





    private bool IsAdAvailable
    {
        get
        {
            return ad != null;
        }
    }

    public void ShowAppOpenAd()
    {
        if (IsAppOpenAdAvailable)
        {
            Debug.Log("Showing app open ad.");
            appOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }
    private void OnAppStateChanged(AppState state)
    {
        Debug.Log("App State changed to : " + state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            if (IsAdAvailable)
            {
                ShowAppOpenAd();
            }
        }
    }
    public void RequestAppOpenAds()
    {
        if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_appopen == 0 || AppOpenAdsIds.Count <= 0)
        {
            return;
        }

        if (IsAdAvailable && !isShowingAd)
        {
            ShowAppOpenAd();
            return;
        }


        if (AppOpenIdIndex >= AppOpenAdsIds.Count)
        {
            AppOpenIdIndex = 0;
        }

        LoadAppOpenAd();

    }

    private AppOpenAd appOpenAd;
    public bool isFirstOpenAdd = true;
    public void LoadAppOpenAd()
    {
        if (AppOpenIdIndex >= AppOpenAdsIds.Count)
        {
            AppOpenIdIndex = 0;
        }
        // Clean up the old ad before loading a new one.
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        Debug.LogError("Loading the app open ad." + AppOpenAdsIds[AppOpenIdIndex]);

        // Create our request used to load the ad.
        var adRequest = new AdRequest();
        string adUnitId = AppOpenAdsIds[AppOpenIdIndex];
        AppOpenAd.Load(adUnitId, adRequest,
          (AppOpenAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("app open ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.LogError("App open ad loaded with response : "
                        + ad.GetResponseInfo());

              appOpenAd = ad;
              RegisterEventHandlers(ad);

              if (isFirstOpenAdd)
              {
                  ShowAppOpenAd();
                  isFirstOpenAdd = false;
              }
          });




        //   var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        //   AppOpenAd.Load(adUnitId, ScreenOrientation.Portrait, adRequest,
        //(AppOpenAd ad, LoadAdError error) =>
        //{
        //   // if error is not null, the load request failed.
        //   if (error != null || ad == null)
        //      {
        //               AppOpenIdIndex++;
        //               Debug.LogError("app open ad failed to load an ad " +
        //                              "with error : " + error);
        //               return;
        //       }

        //           Debug.Log("App open ad loaded with response : "
        //                     + ad.GetResponseInfo());
        //        _expireTime = DateTime.Now + TimeSpan.FromHours(4);
        //        appOpenAd = ad;
        //    AppOpenRegisterEventHandlers(ad);


        //    if (isFirstOpenAdd)
        //    {
        //        ShowAppOpenAd();
        //        isFirstOpenAdd = false;
        //    }
        //       });
    }

    DateTime _expireTime;
    public bool IsAppOpenAdAvailable
    {
        get
        {
            return appOpenAd != null
                   && appOpenAd.CanShowAd();

        }

    }


    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");
            LoadAppOpenAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
            LoadAppOpenAd();
        };
    }
    //private void AppOpenRegisterEventHandlers(AppOpenAd ad)
    //{
    //    // Raised when the ad is estimated to have earned money.
    //    ad.OnAdPaid += (AdValue adValue) =>
    //    {
    //        Debug.Log(String.Format("App open ad paid {0} {1}.",
    //            adValue.Value,
    //            adValue.CurrencyCode));
    //    };
    //    // Raised when an impression is recorded for an ad.
    //    ad.OnAdImpressionRecorded += () =>
    //    {
    //        Debug.Log("App open ad recorded an impression.");
    //    };
    //    // Raised when a click is recorded for an ad.
    //    ad.OnAdClicked += () =>
    //    {
    //        Debug.Log("App open ad was clicked.");
    //    };
    //    // Raised when an ad opened full screen content.
    //    ad.OnAdFullScreenContentOpened += () =>
    //    {
    //        Debug.Log("App open ad full screen content opened.");
    //    };
    //    // Raised when the ad closed full screen content.
    //    ad.OnAdFullScreenContentClosed += () =>
    //    {
    //       // LoadAppOpenAd();
    //        Debug.Log("App open ad full screen content closed.");
    //    };
    //    // Raised when the ad failed to open full screen content.
    //    ad.OnAdFullScreenContentFailed += (AdError error) =>
    //    {
    //        Debug.LogError("App open ad failed to open full screen content " +
    //                       "with error : " + error);
    //        //LoadAppOpenAd();
    //    };
    //}



    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        AppOpenIdIndex++;
        //RequestAppOpenAds();
    }

    private void HandleAdFailedToPresentFullScreenContent(object sender, AdError args)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", args.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        AppOpenIdIndex++;

    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Displayed app open ad");
        isShowingAd = true;
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args)
    {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                args.AdValue.CurrencyCode, args.AdValue.Value);
    }

    public void LoadInterstitialAd()
    {

        if (interstitial != null && interstitial.CanShowAd())
        {
            return;
        }

        if (IntrestitialIdIndex >= IntrestitialAdsIds.Count)
        {
            IntrestitialIdIndex = 0;
            // return;
        }

        string adUnitId = IntrestitialAdsIds[IntrestitialIdIndex];

        var adRequest = new AdRequest();
             

        // send the request to load the ad.
        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    IntrestitialIdIndex++;

                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitial = ad;
                IntrestitialRegisterEventHandlers(ad);
            });
    }

    private void IntrestitialRegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            SuperStarAd.Instance.tryCount = 0;
            SuperStarAd.Instance.adLoader.SetActive(false);
            Debug.Log("Interstitial ad full screen content closed.");
            IntrestitialIdIndex++;
            SuperStarAd.Instance.LoadInterstitialAd(0f);
            SuperStarAd.Instance.isIntrestitiallShowing = false;
            if (SuperStarAd.Instance._callbackIntrestital == null)
            {
                return;
            }
            SuperStarAd.Instance._callbackIntrestital.Invoke(true);
            SuperStarAd.Instance._callbackIntrestital = null;
            //LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            IntrestitialIdIndex++;
            SuperStarAd.Instance.tryCount = 0;
            SuperStarAd.Instance.adLoader.SetActive(false);
            SuperStarAd.Instance.isIntrestitiallShowing = false;
            if (SuperStarAd.Instance.tryCount <= 0)
            {

                SuperStarAd.Instance.LoadInterstitialAd(0f);
                if (SuperStarAd.Instance._callbackIntrestital == null)
                {
                    return;
                }
                SuperStarAd.Instance._callbackIntrestital.Invoke(false);
                SuperStarAd.Instance._callbackIntrestital = null;
            }

        };
    }




    public void ShowInterstrial()
    {
        interstitial.Show();
    }

    public void RequestRewardAd()
    {

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            return;
        }
        if (RewardIdIndex >= RewardAdsIds.Count)
        {
            RewardIdIndex = 0;
            return;
        }
        //Debug.LogError("reward load");
        string adUnitId = RewardAdsIds[RewardIdIndex];

        //this.rewardedAd = new RewardedAd(adUnitId);

        //// Called when an ad request has successfully loaded.
        //this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        //// Called when an ad request failed to load.
        //this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        //// Called when an ad is shown.
        //this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        //// Called when an ad request failed to show.
        //this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        //// Called when the user should be rewarded for interacting with the ad.
        //this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        //// Called when the ad is closed.
        //this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        //// Create an empty ad request.
        //AdRequest request = new AdRequest.Builder().Build();
        //// Load the rewarded ad with the request.
        //this.rewardedAd.LoadAd(request);
        var adRequest = new AdRequest();
        RewardedAd.Load(adUnitId, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              rewardedAd = ad;
              RVRegisterEventHandlers(ad);
          });
    }

    private void RVRegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            if (SuperStarAd.Instance._callback == null)
            {
                return;
            }
            SuperStarAd.Instance._callback.Invoke(true);
            SuperStarAd.Instance._callback = null;
            SuperStarAd.Instance.isRewardShowing = false;
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            SuperStarAd.Instance.isRewardShowing = false;
            RewardIdIndex++;

            if (SuperStarAd.Instance._callback == null)
            {
                return;
            }
            SuperStarAd.Instance._callback.Invoke(false);
            SuperStarAd.Instance._callback = null;

            SuperStarAd.Instance.isRewardGiven = false;
            // SuperStarAd.Instance.isRewardShowing = false;
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #region events of reward video
    //public void HandleRewardedAdLoaded(object sender, EventArgs args)
    //{
    //    MonoBehaviour.print("HandleRewardedAdLoaded event received");
    //}

    //public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    //{

    //    MonoBehaviour.print(
    //        "HandleRewardedAdFailedToLoad event received with message: "
    //                         + args.LoadAdError);
    //}

    //public void HandleRewardedAdOpening(object sender, EventArgs args)
    //{
    //    MonoBehaviour.print("HandleRewardedAdOpening event received");
    //}

    //public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    //{
    //    SuperStarAd.Instance.isRewardShowing = false;
    //    if (SuperStarAd.Instance.isRewardGiven)
    //    {
    //        if (SuperStarAd.Instance._callback == null)
    //        {
    //            return;
    //        }
    //        SuperStarAd.Instance._callback.Invoke(false);
    //        SuperStarAd.Instance._callback = null;
    //    }
    //    SuperStarAd.Instance.isRewardGiven = false;
    //    MonoBehaviour.print(
    //        "HandleRewardedAdFailedToShow event received with message: "
    //                         + args.Message);
    //}

    //public void HandleRewardedAdClosed(object sender, EventArgs args)
    //{
    //    if (SuperStarAd.Instance.isRewardGiven)
    //    {
    //        if (SuperStarAd.Instance._callback == null)
    //        {
    //            return;
    //        }
    //        SuperStarAd.Instance._callback.Invoke(true);
    //        SuperStarAd.Instance._callback = null;
    //    }
    //    else
    //    {
    //        if (SuperStarAd.Instance._callback == null)
    //        {
    //            return;
    //        }
    //        SuperStarAd.Instance._callback.Invoke(false);
    //        SuperStarAd.Instance._callback = null;
    //    }
    //    SuperStarAd.Instance.isRewardShowing = false;
    //    MonoBehaviour.print("HandleRewardedAdClosed event received");
    //}

    //public void HandleUserEarnedReward(object sender, Reward args)
    //{

    //    SuperStarAd.Instance.isRewardGiven = true;
    //    SuperStarAd.Instance.isRewardShowing = false;
    //    //string type = args.Type;
    //    //double amount = args.Amount;
    //    //MonoBehaviour.print("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);
    //    //SuperStarAd.Instance.GiveRewardToUser();
    //    //switch (_videoFor)
    //    //{
    //    //    case VideoFor.PowerUps:
    //    //        //Debug.LogError("Give reward powerups here");
    //    //        //UiManager.Instance.WatchAdForPowerUps();
    //    //        return;
    //    //    case VideoFor.Money:
    //    //        //Debug.LogError("Give reward Money here");
    //    //        return;

    //    //    case VideoFor.Coin:
    //    //       // FindObjectOfType<DailyRewardScript>().RewardMarket150CoinSuccess();
    //    //        Debug.LogError("Give reward Coin here");
    //    //        return;
    //    //    case VideoFor.Diamond:

    //    //        //Debug.LogError("Give reward Diamond here");
    //    //        return;
    //    //    case VideoFor.DubbleCoin:
    //    //        //UiManager.Instance.isDubbleRewardClick = true;
    //    //        //  FindObjectOfType<DailyRewardPopupScript>().GetReward();
    //    //        //Debug.LogError("Give reward Coin here");
    //    //        return;

    //    //    default:
    //    //        return;
    //    //}
    //}
    #endregion



    public void ShowRewardVideo()
    {
        rewardedAd.Show((reward) =>
        {

        });
    }


    //public void ShowRewardVideo()
    //{
    //  //  _videoFor = v;
    //    //Debug.Log("Show RewardAd");
    //    if (rewardedAd != null)
    //    {
    //        if (rewardedAd.IsLoaded())
    //        {
    //            rewardedAd.Show();
    //            Invoke("RequestRewardAd", 1);
    //        }
    //        //else
    //        //{
    //        //  //  ShowRewardedAdUnity();
    //        //    Invoke("RequestRewardAd", 1);
    //        //}
    //    }
    //    else
    //    {
    //        //  ShowRewardedAdUnity();
    //        Invoke("RequestRewardAd", 1);
    //    }

    //}
    //public void ShowRewardedAdUnity()
    //{
    //    const string RewardedPlacementId = "rewardedVideo";

    //    //if (!Advertisement.IsReady(RewardedPlacementId))
    //    //{
    //    //    //Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
    //    //    return;
    //    //}

    //    //var options = new ShowOptions { resultCallback = HandleShowResult };
    //    //Advertisement.Show(RewardedPlacementId, options);
    //}

    //private void HandleShowResult(ShowResult result)
    //{
    //    switch (result)
    //    {
    //        case ShowResult.Finished:
    //            //Debug.Log("The ad was successfully shown.");
    //            switch (_videoFor)
    //            {
    //                case VideoFor.Gems75:
    //                    UILogic.Instance.OnSuccessWatchVideoForGemsMain();
    //                    //Debug.LogError("Give reward 75 gems here");
    //                    return;
    //                case VideoFor.GameOverContinue:
    //                    UILogic.Instance.SuccessContinueGameOver();
    //                    //Debug.LogError("Give reward GameOverContinue here");
    //                    return;
    //                default:
    //                    return;
    //            }
    //            break;
    //        case ShowResult.Skipped:
    //            //Debug.Log("The ad was skipped before reaching the end.");
    //            break;
    //        case ShowResult.Failed:
    //            //Debug.LogError("The ad failed to be shown.");
    //            break;
    //    }
    //}

    //--------------coin Reward------------------------
    //void Update()
    //{
    //    currentTime = DateTime.UtcNow - startTime;
    //    Debug.Log("current time" + currentTime);

    //}
    //private void StartTime()
    //{
    //    startTime = DateTime.UtcNow;
    //    Debug.Log("Start time" + startTime);
    //}
    //private string GetTime(TimeSpan time)
    //{
    //    TimeSpan countdown = hour - time;
    //    return countdown.Hours.ToString() + ":" + countdown.Minutes.ToString()
    //        + ":" + countdown.Seconds.ToString();
    //}




    // private GameObject icon;
    // private bool nativeAdLoaded;
    // private NativeAd nativeAd;
    // private void RequestNativeAd()
    // {
    //     string adUnitId = AppNativeAdsIds[AppNativeIdIndex];
    //     AdLoader adLoader = new AdLoader.Builder(adUnitId)
    //         .ForNativeAd()
    //         .Build();
    //     adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
    //     adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;
    //     adLoader.LoadAd(new AdRequest());
    // }
    // void Update()
    // {


    //     if (this.nativeAdLoaded)
    //     {
    //         this.nativeAdLoaded = false;
    //         // Get Texture2D for icon asset of native ad.
    //         Texture2D iconTexture = this.nativeAd.GetIconTexture();

    //         icon = GameObject.CreatePrimitive(PrimitiveType.Quad);
    //         icon.transform.position = new Vector3(1, 1, 1);
    //         icon.transform.localScale = new Vector3(1, 1, 1);
    //         icon.GetComponent<Renderer>().material.mainTexture = iconTexture;

    //         // Register GameObject that will display icon asset of native ad.
    //         if (!this.nativeAd.RegisterIconImageGameObject(icon))
    //         {
    //             // Handle failure to register ad asset.
    //         }
    //     }
    // }
    // private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
    // {
    //     Debug.Log("Native ad loaded.");

    //     this.nativeAd = args.nativeAd;
    //     this.nativeAdLoaded = true;
    // }

    // private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    // {
    //     Debug.Log("Native ad failed to load: ");
    // }
}

public enum VideoFor
{
    PowerUps,
    Money,
    Coin,
    Diamond,
    DubbleCoin
}