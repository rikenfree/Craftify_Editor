//using GoogleMobileAds.Api;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//namespace AvocadoShark
//{
//    public class Infeed_Ad_Script : MonoBehaviour
//    {
//        private NativeAd nativeAd;
//        private bool nativeAdLoaded;
//        public GameObject Ad_Title;
//        public GameObject Ad_Calltoactiontext;
//        public GameObject Ad_Icon;
//        public GameObject Ad_Choices;
//        // AdUnitSettings Units;
//        private void Start()
//        {
//            AdmobManager.Instance.SetupAds += Setup;

//        }
//        void Setup()
//        {
//            //idNative = "ca-app-pub-3940256099942544/2247696110";
//            RequestNativeAd();
//        }

//        private void RequestNativeAd()
//        {
//            string adUnit = null;
//            Debug.Log("requestedthenate");
            
//                adUnit = AdmobManager.Instance.AppNativeAdsIds[UnityEngine.Random.Range(0, AdmobManager.Instance.AppNativeAdsIds.Count)];
          

//            AdLoader adLoader = new AdLoader.Builder(adUnit)
//          .ForNativeAd()
//          .Build();
//            adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
//            adLoader.OnAdFailedToLoad += this.HandleAdFailedToLoad;
//            adLoader.LoadAd(new AdRequest.Builder().Build());
//        }
//        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
//        {
//            Debug.Log("nateadfailed");
//        }
//        private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
//        {
//            Debug.Log("Native ad loaded.");
//            this.nativeAd = args.nativeAd;
//            this.nativeAdLoaded = true;
//        }
//        void Update()
//        {
//            if (this.nativeAdLoaded)
//            {
//                this.nativeAdLoaded = false;
//                Ad_Icon.GetComponent<RawImage>().texture = this.nativeAd.GetIconTexture();
//                Ad_Choices.GetComponent<RawImage>().texture = this.nativeAd.GetAdChoicesLogoTexture();
//                Ad_Title.GetComponent<Text>().text = this.nativeAd.GetHeadlineText();
//                Ad_Calltoactiontext.GetComponent<Text>().text = this.nativeAd.GetCallToActionText();

//                this.nativeAd.RegisterIconImageGameObject(Ad_Icon);
//                this.nativeAd.RegisterHeadlineTextGameObject(Ad_Title);
//                this.nativeAd.RegisterAdChoicesLogoGameObject(Ad_Choices);
//                this.nativeAd.RegisterCallToActionGameObject(Ad_Calltoactiontext);

//                Invoke("Refreshad", 60);
//            }
//        }
//        void Refreshad()
//        {
//            RequestNativeAd();
//        }
//    }
//}
