//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Advertisements;
//using AudienceNetwork;

//public class AdsManager : MonoBehaviour
//{
//    private AdView adView;
//    private AdPosition currentAdViewPosition;
//    private ScreenOrientation currentScreenOrientation;

//    private RewardedVideoAd rewardedVideoAd;
//    private bool isVideoLoaded;

//    private InterstitialAd interstitialAd;
//    private bool isInterstrialLoaded;

//    private bool isInternetConnect;
//    public static AdsManager instance;


//    private static float lastInterstitial;
//    public int ReloadTime = 40;

//    public GameObject bannerImage;


//    [Header("Android")]
//    public string[] bannerId;
//    public string[] interstrialId, videoId,nativeBannerId;

//    [Header("iOS")]
//    public string[] bannerIdiOS;
//    public string[] interstrialIdiOS, videoIdiOS,nativeBannerIdiOS;

//    public bool isTestMode;

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(this);
//        }
//    }
//    private void Start()
//    {
//        if (Application.internetReachability != NetworkReachability.NotReachable)
//        {
//            //Advertisement.Initiali"123456");
//            isInternetConnect = true;
//            Invoke("LoadAllAds", 1);// ();
//        }
//        else
//        {
//            isInternetConnect = false;
//            InvokeRepeating("CheckNet", 2, 5);
//        }

//        lastInterstitial = -1000f;
//    }

//    private void CheckNet()
//    {
//        if (Application.internetReachability != NetworkReachability.NotReachable)
//        {
//            if (!isInternetConnect)
//            {
//                isInternetConnect = true;
//                Invoke("LoadAllAds", 1);
//            }

//        }

//    }
//    // Start is called before the first frame update
//    void LoadAllAds()
//    {
//      //  Advertisement.Initialize("3361513", false);
//        Invoke("LoadBanner",0);
//        Invoke("LoadInterstrial",2f);

//        //LoadVideo();
//    }

//    void LoadBanner()
//    {
//        if (isInternetConnect)
//        {
//            Debug.LogError("Load banner");
//            if (adView!=null)
//            {
//                adView.Dispose();
//            }
//            string currentId = "YOUR_PLACEMENT_ID";
//            if (isTestMode)
//            {
//                currentId = "YOUR_PLACEMENT_ID";
//            }
//            else
//            {
//#if UNITY_ANDROID
//                currentId = bannerId[Random.Range(0, bannerId.Length)];
//#elif UNITY_IOS
//       currentId = bannerIdiOS[Random.Range(0, bannerIdiOS.Length)];
//#endif
//            }


//            //currentId = "YOUR_PLACEMENT_ID";
//            adView = new AdView(currentId, AdSize.BANNER_HEIGHT_50);

//            adView.Register(gameObject);
//            currentAdViewPosition = AdPosition.TOP;
//            // Set delegates to get notified on changes or when the user interacts with the ad.
//            adView.AdViewDidLoad = delegate ()
//            {
//                currentScreenOrientation = Screen.orientation;

//                string isAdValid = adView.IsValid() ? "valid" : "invalid";
//            };
//            adView.AdViewDidFailWithError = delegate (string error)
//            {
//                Debug.LogError("Banner Fial" + error);
//            };
//            adView.AdViewWillLogImpression = delegate ()
//            {
//            };
//            adView.AdViewDidClick = delegate ()
//            {
//            };

//            // Initiate a request to load an ad.
//            adView.LoadAd();
//            ChangePosition();
//            bannerImage.SetActive(true);
//        }  
//    }

//    public void ChangePosition()
//    {
//        adView.Show(AdPosition.TOP);
//       // currentAdViewPosition = AdPosition.TOP;
//        Debug.LogError(currentAdViewPosition);
       
//    }

//    void LoadInterstrial()
//    {
//        if (isInternetConnect)
//        {
//            Debug.LogError("Load interstrial");
//            string currentId = "YOUR_PLACEMENT_ID";
//            if (isTestMode)
//            {
//                currentId = "YOUR_PLACEMENT_ID";
//            }
//            else
//            {
//#if UNITY_ANDROID
//                currentId = interstrialId[Random.Range(0, interstrialId.Length)];
//#elif UNITY_IOS
//        currentId = interstrialIdiOS[Random.Range(0, interstrialIdiOS.Length)];
//#endif
//            }
//            //currentId = "YOUR_PLACEMENT_ID";
//            // Create the interstitial unit with a placement ID (generate your own on the Facebook app settings).
//            // Use different ID for each ad placement in your app.
//            interstitialAd = new InterstitialAd(currentId);

//            interstitialAd.Register(gameObject);

//            // Set delegates to get notified on changes or when the user interacts with the ad.
//            interstitialAd.InterstitialAdDidLoad = delegate ()
//            {
//                Debug.Log("Interstitial ad loaded.");
//                isInterstrialLoaded = true;
//                string isAdValid = interstitialAd.IsValid() ? "valid" : "invalid";
//            };
//            interstitialAd.InterstitialAdDidFailWithError = delegate (string error)
//            {
//                Time.timeScale = 1;
//                Debug.Log("Interstitial ad failed to load with error: " + error);
//                Invoke("LoadInterstrial", 5);
//            };
//            interstitialAd.InterstitialAdWillLogImpression = delegate ()
//            {
//                Debug.Log("Interstitial ad logged impression.");
//            };
//            interstitialAd.InterstitialAdDidClick = delegate ()
//            {
//                Debug.Log("Interstitial ad clicked.");
//            };
//            interstitialAd.InterstitialAdDidClose = delegate ()
//            {

//                Time.timeScale = 1;
//                Debug.Log("Interstitial ad did close.");
//                if (interstitialAd != null)
//                {
//                    interstitialAd.Dispose();
//                }
//            };

//#if UNITY_ANDROID
//            interstitialAd.interstitialAdActivityDestroyed = delegate ()
//            {
//                //if (!didClose)
//                //{
//                //    Debug.Log("Interstitial activity destroyed without being closed first.");
//                //    Debug.Log("Game should resume.");
//                //}
//            };
//#endif

//            // Initiate the request to load the ad.
//            interstitialAd.LoadAd();
//        }
//    }

//    void LoadVideo()
//    {
//        if (isInternetConnect)
//        {
//            Debug.LogError("Load video");
//            string currentId = "YOUR_PLACEMENT_ID";
//            if (isTestMode)
//            {
//                currentId = "YOUR_PLACEMENT_ID";
//            }
//            else
//            {
//#if UNITY_ANDROID
//                currentId = videoId[Random.Range(0, 4)];
//#elif UNITY_IOS
//       currentId = videoIdiOS[Random.Range(0, videoIdiOS.Length)];
//#endif
//            }
//            rewardedVideoAd = new RewardedVideoAd(currentId);

//            RewardData rewardData = new RewardData
//            {
//                UserId = "USER_ID",
//                Currency = "REWARD_ID"
//            };
//#pragma warning disable 0219
//            RewardedVideoAd s2sRewardedVideoAd = new RewardedVideoAd(currentId, rewardData);
//#pragma warning restore 0219

//            rewardedVideoAd.Register(gameObject);

//            // Set delegates to get notified on changes or when the user interacts with the ad.
//            rewardedVideoAd.RewardedVideoAdDidLoad = delegate ()
//            {
//                Debug.Log("RewardedVideo ad loaded.");
//                isVideoLoaded = true;
//                string isAdValid = rewardedVideoAd.IsValid() ? "valid" : "invalid";
//            };
//            rewardedVideoAd.RewardedVideoAdDidFailWithError = delegate (string error)
//            {
//                Debug.Log("RewardedVideo ad failed to load with error: " + error);
//                Invoke("LoadVideo", 10);
//            };
//            rewardedVideoAd.RewardedVideoAdWillLogImpression = delegate ()
//            {
//                Debug.Log("RewardedVideo ad logged impression.");
//            };
//            rewardedVideoAd.RewardedVideoAdDidClick = delegate ()
//            {
//                Debug.Log("RewardedVideo ad clicked.");
//            };

//            // For S2S validation you need to register the following two callback
//            // Refer to documentation here:
//            // https://developers.facebook.com/docs/audience-network/android/rewarded-video#server-side-reward-validation
//            // https://developers.facebook.com/docs/audience-network/ios/rewarded-video#server-side-reward-validation
//            rewardedVideoAd.RewardedVideoAdDidSucceed = delegate ()
//            {
//                Debug.Log("Rewarded video ad validated by server");
//            };

//            rewardedVideoAd.RewardedVideoAdDidFail = delegate ()
//            {
//                Debug.Log("Rewarded video ad not validated, or no response from server");
//            };

//            rewardedVideoAd.RewardedVideoAdDidClose = delegate ()
//            {
//                Debug.Log("Rewarded video ad did close.");
              
//            };

//#if UNITY_ANDROID
//            rewardedVideoAd.RewardedVideoAdActivityDestroyed = delegate ()
//            {
//            //if (!didClose)
//            //{
//            //    Debug.Log("Rewarded video activity destroyed without being closed first.");
//            //    Debug.Log("Game should resume. User should not get a reward.");
//            //}
//        };
//#endif

//            // Initiate the request to load the ad.
//            rewardedVideoAd.LoadAd();
//        }
//    }

//    public bool IsInterstrialLoaded()
//    {
//        return isInterstrialLoaded;
//    }

//    public void ShowInterstrial()
//    {
//        if (isInterstrialLoaded)
//        {
//            interstitialAd.Show();
//            isInterstrialLoaded = false;
//            Invoke("LoadInterstrial", 10);
//        }
//        else
//        {
//            //if (Advertisement.IsReady("video"))
//            //{
//            //    Advertisement.Show("video");
//            //}
//            LoadInterstrial();
//        }
//    }

//    public bool ShowInterstitialIfReady()
//        {
//            if (!isInterstrialLoaded)
//            {
              
//                return false;
//            }
//            float time = Time.time;
//            if (time - lastInterstitial > ReloadTime)
//            {
//                ShowInterstrial();
//                lastInterstitial = time;
//                return true;
//            }
//           // f();
//            return false;
//        }

//    public void ShowVideo()
//    {
//        if (isVideoLoaded)
//        {
//            rewardedVideoAd.Show();
//            isVideoLoaded = false;
//            Invoke("LoadVideo", 15);
//        }
//        else
//        {
//            //if (Advertisement.IsReady("rewardedVideo"))
//            //{
//            //    ShowOptions options = new ShowOptions();
//            //    options.resultCallback = AdCallbackhanler;
//            //    Advertisement.Show("rewardedVideo", options);
//            //}
//            LoadVideo();
//        }
        
//    }

//    //void AdCallbackhanler(ShowResult result)
//    //{
//    //    switch (result)
//    //    {
//    //        case ShowResult.Finished:
              
//    //            break;
//    //        case ShowResult.Skipped:
//    //            Debug.Log("Ad Skipped");
//    //            break;
//    //        case ShowResult.Failed:
//    //            Debug.Log("Ad failed");
//    //            break;
//    //    }

//    //}
//}