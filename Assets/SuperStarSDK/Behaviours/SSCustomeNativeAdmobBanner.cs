// using GoogleMobileAds.Api;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SSCustomeNativeAdmobBanner : MonoBehaviour
// {

//     private bool adLoaded;
//     private Texture2D mainImageTexture;
//     private string headline;
//     private CustomNativeAd customNativeAd;

//     private void LoadCustomNativeAd()
//     {
//         AdLoader adLoader = new AdLoader.Builder("/6499/example/native")
//                 .ForCustomNativeAd("10063170", HandleCustomNativeAdClicked)
//                 .Build();
//         adLoader.OnCustomNativeAdLoaded += HandleCustomNativeAdLoaded;
//         adLoader.LoadAd(new AdRequest());
//     }

//     private void HandleCustomNativeAdClicked(CustomNativeAd customNativeAd, string assetName)
//     {
//         Debug.Log("Custom Native ad asset with name " + assetName + " was clicked.");
//     }

//     void HandleCustomNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//     {
//         string message = args.LoadAdError.GetMessage();
//         Debug.Log("Ad Loader fail event received with message: " + message);
//     }
//     void HandleCustomNativeAdLoaded(object sender, CustomNativeAdEventArgs args)
//     {
//         this.customNativeAd = args.nativeAd;
//     }
// }
