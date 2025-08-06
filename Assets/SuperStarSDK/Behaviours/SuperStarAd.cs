using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.unity3d.mediation;
namespace SuperStarSdk
{
    public class SuperStarAd : MonoBehaviour
    {
        public static SuperStarAd Instance;

        public delegate void AfterInterstitialFunction();

        public bool isTestAdmobAds = false;
        List<string> TestAISBID = new List<string>() { "ca-app-pub-3940256099942544/6300978111" };
        List<string> TestAISInID = new List<string>() { "ca-app-pub-3940256099942544/1033173712" };
        List<string> TestAISReID = new List<string>() { "ca-app-pub-3940256099942544/5224354917" };
      

        List<string> TestIISBID = new List<string>() { "ca-app-pub-3940256099942544/2934735716" };
        List<string> TestIISInID = new List<string>() { "ca-app-pub-3940256099942544/4411468910" };
        List<string> TestIISReID = new List<string>() { "ca-app-pub-3940256099942544/1712485313" };


        public List<string> BannerAdsIds = new List<string>();
        public List<string> IntrestitialAdsIds = new List<string>();
        public List<string> RewardAdsIds = new List<string>();

        public int BannerIdIndex = 0;
        public int IntrestitialIdIndex = 0;
        public int RewardIdIndex = 0;
        public int AppOpenIdIndex = 0;
        public int AppNativeIdIndex = 0;


        private static float lastInterstitial;

        private int triggeredAdRetryCounter;

        private static bool triggeredAdReady;


        public bool testMode = false;

        public LevelPlayBannerPosition baanerPosition;
        public AdPosition baanerPositionadmob;

        public GameObject bannerImage;
        public GameObject adLoader;

        public GameObject SSIntrestitialPrefab;

        public int ReloadTime = 40;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

        }

        public int NoAds
        {
            get
            {
                return PlayerPrefs.GetInt("NoAds", 0);
            }
            set
            {
                PlayerPrefs.SetInt("NoAds", value);
            }
        }

        public static string uniqueUserId = "SSSdk";


        private void OnEnable() 
        {
            IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoAdClickedEvent;
        }
        public void Setup()
        {

            if (isTestAdmobAds)
            {
#if UNITY_ANDROID
               
                BannerAdsIds = TestAISBID;
                IntrestitialAdsIds = TestAISInID;
                RewardAdsIds = TestAISReID;

#else
          
            BannerAdsIds = TestIISBID;
            IntrestitialAdsIds = TestIISInID;
            RewardAdsIds = TestIISReID;
          
#endif
            }
            else
            {
                BannerAdsIds = SuperStarSdkManager.Instance.crossPromoAssetsRoot.ISBID;
                IntrestitialAdsIds = SuperStarSdkManager.Instance.crossPromoAssetsRoot.ISInID;
                RewardAdsIds = SuperStarSdkManager.Instance.crossPromoAssetsRoot.ISReID;
            }

#if UNITY_ANDROID
            string appKey = SuperStarSdkManager.Instance.crossPromoAssetsRoot.ISAppKey;
#elif UNITY_IPHONE
        string appKey = SuperStarSdkManager.Instance.crossPromoAssetsRoot.ISAppKey;
#else
        string appKey = "unexpected_platform";
#endif

           

            string id = IronSource.Agent.getAdvertiserId();
            Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);

            Debug.Log("unity-script: IronSource.Agent.validateIntegration");
            IronSource.Agent.validateIntegration();

            Debug.Log("unity-script: unity version" + IronSource.unityVersion());

            // SDK init
            Debug.Log("unity-script: LevelPlay Init");
            LevelPlay.Init(appKey, uniqueUserId, new[] { LevelPlayAdFormat.REWARDED });

            LevelPlay.OnInitSuccess += OnInitializationCompleted;
            LevelPlay.OnInitFailed += (error => Debug.Log("Initialization error: " + error));

            bannerImage.SetActive(false);

            if (NoAds == 0)
            {
                Debug.Log("no ads is inactive");
                ShowBannerAd();
                LoadInterstitialAd(0);
            }
            else
            {
                Debug.Log("no ads is activated");

                // ShowBannerAd();
                //LoadInterstitialAd(0);
            }
            lastInterstitial = -1000f;

        }
        private LevelPlayBannerAd bannerAd;
        void OnInitializationCompleted(LevelPlayConfiguration configuration)
        {
            Debug.Log("Initialization completed");
            LoadBanner();
            LoadInterstitialIS();
        }

        void LoadBanner()
        {
            // Create object
            bannerAd = new LevelPlayBannerAd(BannerAdsIds[BannerIdIndex],null,baanerPosition);

            bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
            bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
            bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
            bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
            bannerAd.OnAdClicked += BannerOnAdClickedEvent;
            bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
            bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
            bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;

            // Ad load
            bannerAd.LoadAd();
        }

        public void DestroyBanner(){

            bannerAd.DestroyAd();
        }



        #region RewardedAd callback handlers
        void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
        {
            Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
        }

        void RewardedVideoAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            isRewardGiven = true;
            isRewardShowing = false;
            Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
        }

        void RewardedVideoAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            isRewardGiven = true;
            isRewardShowing = false;
            Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + placement.getRewardAmount() + " name = " + placement.getRewardName());
            //GiveRewardToUser();
        }

        void RewardedVideoAdClosedEvent(IronSourceAdInfo adInfo)
        {

            if (isRewardGiven)
            {
                if (_callback == null)
                {
                    return;
                }
                _callback.Invoke(true);
                _callback = null;
            }
            else
            {
                if (_callback == null)
                {
                    return;
                }
                _callback.Invoke(false);
                _callback = null;
            }
            isRewardShowing = false;
            //Time.timeScale = 1;
            // GiveRewardToUser();
            Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
        }
        private void RewardedVideoOnAdUnavailable()
        {
            Debug.Log("unity-script: RewardedVideoOnAdUnavailable");
            //throw new NotImplementedException();
        }

        private void RewardedVideoOnAdAvailable(IronSourceAdInfo obj)
        {
            Debug.Log("unity-script: RewardedVideoOnAdAvailable");
            //  throw new NotImplementedException();
        }
        void RewardedVideoAdStartedEvent()
        {
            Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
        }

        void RewardedVideoAdEndedEvent()
        {
            isRewardShowing = false;
            Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
        }

        void RewardedVideoAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
        {
            isRewardShowing = false;
            if (isRewardGiven)
            {
                if (_callback == null)
                {
                    return;
                }
                _callback.Invoke(false);
                _callback = null;
            }
            // Time.timeScale = 1;
            Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        }

        void RewardedVideoAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + placement.getRewardName());
        }
        #endregion

       
        void InterstitialAdReadyEvent(IronSourceAdInfo adInfo)
        {
            isLoadingISIntrestital = false;
            Debug.Log("unity-script: I got InterstitialAdReadyEvent");
        }

        void InterstitialAdLoadFailedEvent(IronSourceError error)
        {
            //  Time.timeScale = 1;
            isLoadingISIntrestital = false;
            LoadInterstitialAd(0f);
            isIntrestitiallShowing = false;


            Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
        }

        void InterstitialAdShowSucceededEvent(IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
            adLoader.SetActive(false);
            //if (GameManager.Instance.IdForShowIntrestitialAd == "TryAgain")
            //{
            //    GameManager.Instance.IdForShowIntrestitialAd = "";
            //    GameManager.Instance.CloseAdAfterCallForTryAgain();
            //}
        }

        void InterstitialAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
        {
            tryCount = 0;
            adLoader.SetActive(false);
            isIntrestitiallShowing = false;
            if (tryCount <= 0)
            {

                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
            }
            //Time.timeScale = 1;
            Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());

        }

        void InterstitialAdClickedEvent(IronSourceAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialAdClickedEvent");
        }

        void InterstitialAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            // Time.timeScale = 0;
            Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
        }

        void InterstitialAdClosedEvent(IronSourceAdInfo adInfo)
        {
            //  Time.timeScale = 1;
            tryCount = 0;
            adLoader.SetActive(false);
            LoadInterstitialAd(0f);
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;

            Debug.Log("heheheheheh");

        }
        
        #region Banner callback handlers
        void BannerOnAdLoadedEvent(LevelPlayAdInfo obj)
        {
            //Debug.Log("unity-script: I got BannerAdLoadedEvent");
            bannerImage.SetActive(true);
            Debug.Log("RefreshBanner");

        }

        void BannerOnAdLoadFailedEvent(LevelPlayAdError error)
        {
            Debug.Log("unity-script: I got BannerOnAdLoadFailedEvent With Error " + error);
        }

        void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdClickedEvent With AdInfo " + adInfo);
        }

        void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdDisplayedEvent With AdInfo " + adInfo);
        }

        void BannerOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError adInfoError)
        {
            Debug.Log("unity-script: I got BannerOnAdDisplayFailedEvent With AdInfoError " + adInfoError);
        }

        void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdCollapsedEvent With AdInfo " + adInfo);
        }

        void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdLeftApplicationEvent With AdInfo " + adInfo);
        }

        void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got BannerOnAdExpandedEvent With AdInfo " + adInfo);
        }
        #endregion


        public void ShowBannerAd(int bannertype=0)
        {
            if (NoAds != 1)
            {

                if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_IS_banner_ads == 1)
                {
                    Debug.Log("Banner ad load");
                    LoadBanner();
                    bannerImage.SetActive(true);
                }
                else if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_banner == 1)
                {
                    if (bannertype==1){
                    AdmobManager.Instance.RequestAdaptiveBanner(baanerPositionadmob);
                    bannerImage.SetActive(true);
                    }else {
                        AdmobManager.Instance.RequestBanner(baanerPositionadmob);
                        bannerImage.SetActive(true);
                    }
                }
            }
        }

        public void HideBannerAd()
        {
            Debug.Log("Hide Banner Ad");
            bannerImage.SetActive(false);
            bannerAd.DestroyAd();
            AdmobManager.Instance.DestrotyBannerAd();
            //ShowBannerAd();
        }

        private LevelPlayInterstitialAd interstitialAd;
        public void LoadInterstitialIS()
        {
            // Create interstitial Ad
            interstitialAd = new LevelPlayInterstitialAd(IntrestitialAdsIds[IntrestitialIdIndex]);

            // Register to events
            interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
            interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
            interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
            interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
            interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
            interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
            interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;

            Debug.Log("unity-script: LoadInterstitialButtonClicked");
            interstitialAd.LoadAd();
        }
        void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
        {
            isLoadingISIntrestital = false;
            Debug.Log("unity-script: I got InterstitialOnAdLoadedEvent With AdInfo " + adInfo);
        }

        void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
        {
            isLoadingISIntrestital = false;
            LoadInterstitialAd(0f);
            isIntrestitiallShowing = false;
            Debug.Log("unity-script: I got InterstitialOnAdLoadFailedEvent With Error " + error);
        }

        void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            adLoader.SetActive(false);
            Debug.Log("unity-script: I got InterstitialOnAdDisplayedEvent With AdInfo " + adInfo);
        }

        void InterstitialOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError infoError)
        {
            tryCount = 0;
            adLoader.SetActive(false);
            isIntrestitiallShowing = false;
            if (tryCount <= 0)
            {

                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
            }
            Debug.Log("unity-script: I got InterstitialOnAdDisplayFailedEvent With InfoError " + infoError);
        }

        void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialOnAdClickedEvent With AdInfo " + adInfo);
        }

        void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
        {
            tryCount = 0;
            adLoader.SetActive(false);
            LoadInterstitialAd(0f);
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
            Debug.Log("unity-script: I got InterstitialOnAdClosedEvent With AdInfo " + adInfo);
        }

        void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
        {
            Debug.Log("unity-script: I got InterstitialOnAdInfoChangedEvent With AdInfo " + adInfo);
        }


        public bool isLoadingISIntrestital;
        public void LoadInterstitialAd()
        {
            if (interstitialAd !=null && !interstitialAd.IsAdReady() && !isLoadingISIntrestital)
            {
                isLoadingISIntrestital = true;
                LoadInterstitialIS();
            }
            Debug.Log("Load Interstitial AD");
            if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial == 1)
            {
                AdmobManager.Instance.LoadInterstitialAd();
            }

        }

        public void LoadInterstitialAd(float delay)
        {
            Invoke(nameof(LoadInterstitialAd), delay);
        }


        public void ShowInterstitialIS()
        {
            float time = Time.time;
            if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_IS_interstitial_ads == 1 && interstitialAd.IsAdReady())
            {

                interstitialAd.ShowAd();
                lastInterstitial = time;

            }
            //else if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial == 1 && AdmobManager.Instance.interstitial != null && AdmobManager.Instance.interstitial.IsLoaded())
            //{
            //    IronSource.Agent.loadInterstitial();
            //    AdmobManager.Instance.ShowInterstrial();
            //    lastInterstitial = time;
            //}

            else
            {
                LoadInterstitialAd(0f);
            }
        }

        public int InterstitialCounter = 0;


        public bool ISIntrestitialReadyToShow(bool ForceShow = false)
        {
            if (!interstitialAd.IsAdReady())
            {
              LoadInterstitialAd(0);
            }
            float time = Time.time;
            bool isloadedTime = false;
            if (time - lastInterstitial >= ReloadTime)
            {
                //ShowInterstitial(f);
                //lastInterstitial = time;
                isloadedTime = true;
            }
            if (ForceShow)
            {
                isloadedTime = true;
            }
            return SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_IS_interstitial_ads == 1 && interstitialAd.IsAdReady() && isloadedTime;
        }


        public void ExampleShowIntrestitialWithCallback()
        {
            ShowInterstitialTimer(TestIntrestitialWithCallback);
        }

        public void ExampleShowIntrestitialWithLoaderCallback()
        {
            ShowForceInterstitialWithLoader(TestIntrestitialWithCallback, 3);
        }

        public void TestIntrestitialWithCallback(bool isCompleted)
        {

            if (isCompleted)
            {
                //Give reward here
                Debug.Log("Intrestitial  completed  Do other thing");

            }
            else
            {
                Debug.Log("Intrestitial  has issue");

                // do next step as reward not available
            }
        }

        public bool IsAdmobIntrestitialAdAvailable(bool ForceShow = false)
        {
            float time = Time.time;
            bool isloadedTime = false;
            if (time - lastInterstitial >= ReloadTime)
            {
                //ShowInterstitial(f);
                //lastInterstitial = time;
                isloadedTime = true;
            }
            if (ForceShow)
            {
                isloadedTime = true;
            }
            return SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial == 1 && AdmobManager.Instance.interstitial != null && AdmobManager.Instance.interstitial.CanShowAd() && isloadedTime;
        }

        public Action<bool> _callbackIntrestital;
        [HideInInspector]
        public bool isIntrestitiallShowing = false;
        public void ShowInterstitialTimer(Action<bool> onComplete)
        {
            tryCount = 0;
            Debug.Log("isIntrestitiallShowing => " + isIntrestitiallShowing);
            if (isIntrestitiallShowing)
            {
                return;
            }

            if (NoAds == 1)
            {
                isIntrestitiallShowing = false;
                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
                return;
            }
            isIntrestitiallShowing = true;
            _callbackIntrestital = onComplete;

            if (testMode)
            {
                isIntrestitiallShowing = false;
                adLoader.SetActive(false);
                if (_callbackIntrestital == null)
                {
                    return;
                }

                _callbackIntrestital.Invoke(true);
                _callbackIntrestital = null;
                return;
            }

#if UNITY_EDITOR
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
            return;
#endif
            //Regular call

            adLoader.SetActive(true);
            Debug.Log("ShowInterstitialTimer");
            if (IsAdmobIntrestitialAdAvailable() && ISIntrestitialReadyToShow())
            {
                float admobintrestitalprobablity = UnityEngine.Random.Range(0f, 1f);
                if (admobintrestitalprobablity <= SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial_Probablity)
                {
                    AdmobManager.Instance.ShowInterstrial();
                    lastInterstitial = Time.time;
                }
                else
                {
                    ShowInterstitialIS();
                      lastInterstitial = Time.time;
                }
            }
            else if (IsAdmobIntrestitialAdAvailable())
            {
                AdmobManager.Instance.ShowInterstrial();
                lastInterstitial = Time.time;
                if (!ISIntrestitialReadyToShow())
                {
                    LoadInterstitialAd(0);
                }
            }
            else if (ISIntrestitialReadyToShow())
            {
                Debug.Log("Ready to show");
                ShowInterstitialIS();
                lastInterstitial = Time.time;
            }
           
            else
            {
                ShowSSIntrestitial();
            }
        }

        public void ShowSSIntrestitial()
        {
            if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_ssinterstitial == 1)
            {
                Debug.Log("SSIntrestitialPrefab Ready to show");
                Instantiate(SSIntrestitialPrefab);
            }
            else
            {

                Debug.Log("can't show");
                adLoader.SetActive(false);
                isIntrestitiallShowing = false;
                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
            }


            LoadInterstitialAd(0f);

        }


        public void OnCloseSSIntrestitial()
        {
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
        }

        public void ShowForceInterstitial(Action<bool> onComplete)
        {
            tryCount = 0;
            if (isIntrestitiallShowing)
            {
                return;
            }
            if (NoAds == 1)
            {
                isIntrestitiallShowing = false;
                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
                return;
            }
            isIntrestitiallShowing = true;
            _callbackIntrestital = onComplete;


            if (testMode)
            {
                isIntrestitiallShowing = false;
                adLoader.SetActive(false);
                if (_callbackIntrestital == null)
                {
                    return;
                }

                _callbackIntrestital.Invoke(true);
                _callbackIntrestital = null;
                return;
            }
#if UNITY_EDITOR
            isIntrestitiallShowing = false;
            if (_callbackIntrestital == null)
            {
                return;
            }
            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
            return;
#endif

            //Regular call

            adLoader.SetActive(true);
            if (IsAdmobIntrestitialAdAvailable() && ISIntrestitialReadyToShow())
            {

                float admobintrestitalprobablity = UnityEngine.Random.Range(0f, 1f);
                if (admobintrestitalprobablity <= SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial_Probablity)
                {
                    AdmobManager.Instance.ShowInterstrial();
                    lastInterstitial = Time.time;
                    if (!ISIntrestitialReadyToShow())
                    {
                        LoadInterstitialAd(0);
                    }
                }
                else
                {
                  ShowInterstitialIS();
                    lastInterstitial = Time.time;
                }

            }
            else if (IsAdmobIntrestitialAdAvailable(true))
            {
                AdmobManager.Instance.ShowInterstrial();
                lastInterstitial = Time.time;
                if (!ISIntrestitialReadyToShow())
                {
                    LoadInterstitialAd(0);
                }
            }
            else if (ISIntrestitialReadyToShow(true))
            {
                ShowInterstitialIS();
                lastInterstitial = Time.time;
            }
           
            else
            {
                if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_ssinterstitial == 1)
                {
                    Debug.Log("SSIntrestitialPrefab Ready to show");
                    Instantiate(SSIntrestitialPrefab);
                }
                else
                {
                    adLoader.SetActive(false);
                    isIntrestitiallShowing = false;
                    if (_callbackIntrestital == null)
                    {
                        return;
                    }
                    _callbackIntrestital.Invoke(false);
                    _callbackIntrestital = null;
                }
                LoadInterstitialAd(0f);
            }
        }


        public int tryCount;
        public void ShowForceInterstitialWithLoader(Action<bool> onComplete, int _tryCount)
        {
            tryCount = _tryCount;
            // adLoader.SetActive(true);
            //alreay showing
            if (isIntrestitiallShowing)
            {
                return;
            }

            // No Ads Purchased
            if (NoAds == 1)
            {
                isIntrestitiallShowing = false;
                if (_callbackIntrestital == null)
                {
                    return;
                }
                _callbackIntrestital.Invoke(false);
                _callbackIntrestital = null;
                return;
            }

            //Regular call

            adLoader.SetActive(true);

            isIntrestitiallShowing = true;
            _callbackIntrestital = onComplete;

            if (testMode)
            {
                isIntrestitiallShowing = false;
                adLoader.SetActive(false);
                if (_callbackIntrestital == null)
                {
                    return;
                }

                _callbackIntrestital.Invoke(true);
                _callbackIntrestital = null;
                return;
            }

#if UNITY_EDITOR
            isIntrestitiallShowing = false;
            adLoader.SetActive(false);
            if (_callbackIntrestital == null)
            {
                return;
            }

            _callbackIntrestital.Invoke(true);
            _callbackIntrestital = null;
            return;
#endif
            if (IsAdmobIntrestitialAdAvailable() && ISIntrestitialReadyToShow())
            {
                Debug.LogError("Both loaded");
                float admobintrestitalprobablity = UnityEngine.Random.Range(0f, 1f);
                if (admobintrestitalprobablity <= SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_intrestitial_Probablity)
                {
                    AdmobManager.Instance.ShowInterstrial();
                    lastInterstitial = Time.time;
                    if (!ISIntrestitialReadyToShow())
                    {
                       LoadInterstitialAd(0);
                    }
                }
                else
                {
                    ShowInterstitialIS();
                    lastInterstitial = Time.time;
                }

            }
            else if (IsAdmobIntrestitialAdAvailable(true))
            {
                AdmobManager.Instance.ShowInterstrial();
                lastInterstitial = Time.time;
                if (!ISIntrestitialReadyToShow())
                {
                    LoadInterstitialAd(0);
                }
            }
            else if (ISIntrestitialReadyToShow(true))
            {
                ShowInterstitialIS();
                lastInterstitial = Time.time;
            }
            else
            {

                if (tryCount > 0)
                {
                    // tryCount--;
                    isIntrestitiallShowing = false;
                    LoadInterstitialAd(0);
                    StartCoroutine(IEShowForceInterstitialWithLoader(_callbackIntrestital));
                }
                else
                {
                    adLoader.SetActive(false);
                    isIntrestitiallShowing = false;
                    if (_callbackIntrestital == null)
                    {
                        return;
                    }
                    _callbackIntrestital.Invoke(false);
                    _callbackIntrestital = null;
                }
                LoadInterstitialAd(0f);
            }
        }

        public IEnumerator IEShowForceInterstitialWithLoader(Action<bool> onComplete)
        {
            tryCount--;
            if (tryCount > 0)
            {
                yield return new WaitForSeconds(1f);
                ShowForceInterstitialWithLoader(onComplete, tryCount);
            }
            else if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_ssinterstitial == 1)
            {
                Debug.Log("SSIntrestitialPrefab Ready to show");
                Instantiate(SSIntrestitialPrefab);
            }
            else
            {
                tryCount = 0;
                adLoader.SetActive(false);
                isIntrestitiallShowing = false;
                if (_callbackIntrestital != null)
                {
                    _callbackIntrestital.Invoke(false);
                    _callbackIntrestital = null;
                }
                LoadInterstitialAd(0f);
            }
        }

        public void ExampleShowReward()
        {
            ShowRewardVideo(ExampleShowRewardAssign);
        }

        public void ExampleShowRewardAssign(bool isrewarded)
        {
            if (isrewarded)
            {
                //Give reward here
                Debug.Log("Reward Given");
            }
            else
            {
                Debug.Log("Reward Eroor Given");
                // do next step as reward not available
            }
        }
        public Action<bool> _callback;
        [HideInInspector]
        public bool isRewardShowing = false;
        string VideoFor = "";
        [HideInInspector]
        public bool isRewardGiven = false;

        public bool IsAdmobRewardAdAvailable()
        {

            return SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_reward == 1 && AdmobManager.Instance.rewardedAd != null && AdmobManager.Instance.rewardedAd.CanShowAd();

            //return AdmobManager.Instance.interstitial != null && AdmobManager.Instance.interstitial.CanShowAd();
        }

        public bool IsISRewardAdAvailable()
        {

            return SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_IS_reward_ads == 1 && IronSource.Agent.isRewardedVideoAvailable();

            //return AdmobManager.Instance.interstitial != null && AdmobManager.Instance.interstitial.CanShowAd();
        }
        public void ShowRewardVideo(Action<bool> onComplete)
        {
            Debug.Log("Show Reward video Ad");

            if (isRewardShowing)
            {
                return;
            }



            isRewardGiven = false;
            isRewardShowing = true;
            _callback = onComplete;

            if (testMode)
            {
                isRewardShowing = false;
                if (_callback == null)
                {
                    return;
                }
                _callback.Invoke(true);
                _callback = null;
                return;
            }
            //GiveRewardToUser();
#if UNITY_EDITOR
            isRewardShowing = false;
            if (_callback == null)
            {
                return;
            }
            _callback.Invoke(true);
            _callback = null;
            return;
#endif

            if (IsISRewardAdAvailable() && IsAdmobRewardAdAvailable())
            {

                float admobintrestitalprobablity = UnityEngine.Random.Range(0f, 1f);
                if (admobintrestitalprobablity <= SuperStarSdkManager.Instance.crossPromoAssetsRoot.display_Admob_reward_Probablity)
                {
                    AdmobManager.Instance.ShowRewardVideo();
                }
                else
                {
                    IronSource.Agent.showRewardedVideo();

                }
            }
            else if (IsISRewardAdAvailable())
            {
                //SoundManager.Instance.StopAllSoundRunningVideoAd(true);
                IronSource.Agent.showRewardedVideo();
                AdmobManager.Instance.RequestRewardAd();
            }
            else if (IsAdmobRewardAdAvailable())
            {
                AdmobManager.Instance.ShowRewardVideo();
                IronSource.Agent.loadRewardedVideo();
            }
            else
            {
                AdmobManager.Instance.RequestRewardAd();
                IronSource.Agent.loadRewardedVideo();
                Debug.LogError("Problem in showing video");
                isRewardShowing = false;
                if (_callback == null)
                {
                    return;
                }
                _callback.Invoke(false);
                _callback = null;
                return;
                // NotificationHandler.Instance.ShowNotification("Reward Ad is not available!");
            }
        }

        public void GiveRewardToUser()
        {
            Debug.LogError("watch video comepleted:" + VideoFor);
            switch (VideoFor)
            {

            }
        }




    }




}
