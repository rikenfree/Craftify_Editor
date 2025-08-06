//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using GoogleMobileAds.Api;

//namespace AvocadoShark
//{
//    public class Native_Large_Static_Ad_Script : MonoBehaviour
//    {
//        private NativeAd nativeAd;
//        private bool nativeAdLoaded;
//        public GameObject Ad_Title;
//        public GameObject Ad_Advertiser;
//        public GameObject Ad_Store;
//        public GameObject Ad_Price;
//        public GameObject Ad_Description;
//        public GameObject Ad_Calltoactiontext;
//        public GameObject Ad_Icon;
//        public GameObject Ad_Image;
//        public GameObject Ad_Choices;
//        //  AdUnitSettings Units;
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
//            adUnit = AdmobManager.Instance.AppNativeAdsIds[UnityEngine.Random.Range(0, AdmobManager.Instance.AppNativeAdsIds.Count)];

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
//                Texture2D adtextureimage = this.nativeAd.GetImageTextures()[0];
//                Ad_Advertiser.GetComponent<Text>().text = this.nativeAd.GetAdvertiserText();
//                Ad_Store.GetComponent<Text>().text = this.nativeAd.GetStore();
//                Ad_Price.GetComponent<Text>().text = this.nativeAd.GetPrice();
//                Ad_Description.GetComponent<Text>().text = this.nativeAd.GetBodyText();
//                Ad_Icon.GetComponent<RawImage>().texture = this.nativeAd.GetIconTexture();
//                Ad_Choices.GetComponent<RawImage>().texture = this.nativeAd.GetAdChoicesLogoTexture();
//                Ad_Title.GetComponent<Text>().text = this.nativeAd.GetHeadlineText();
//                Ad_Calltoactiontext.GetComponent<Text>().text = this.nativeAd.GetCallToActionText();
//                Ad_Image.GetComponent<Image>().sprite = Sprite.Create(adtextureimage, new Rect(0, 0, adtextureimage.width, adtextureimage.height), new Vector2(0.5f, 0.5f), 100);
//                List<GameObject> images = new List<GameObject>();
//                images.Add(Ad_Image);

//                this.nativeAd.RegisterIconImageGameObject(Ad_Icon);
//                this.nativeAd.RegisterHeadlineTextGameObject(Ad_Title);
//                this.nativeAd.RegisterAdvertiserTextGameObject(Ad_Advertiser);
//                this.nativeAd.RegisterStoreGameObject(Ad_Store);
//                this.nativeAd.RegisterPriceGameObject(Ad_Price);
//                this.nativeAd.RegisterBodyTextGameObject(Ad_Description);
//                this.nativeAd.RegisterAdChoicesLogoGameObject(Ad_Choices);
//                this.nativeAd.RegisterCallToActionGameObject(Ad_Calltoactiontext);
//                this.nativeAd.RegisterImageGameObjects(images);

//                Invoke("Refreshad", 60);
//            }
//        }
//        void Refreshad()
//        {
//            RequestNativeAd();
//        }
//    }
//}
