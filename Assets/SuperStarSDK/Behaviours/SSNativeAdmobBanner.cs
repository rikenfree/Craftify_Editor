// using GoogleMobileAds.Api;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class SSNativeAdmobBanner : MonoBehaviour
// {

// 	private NativeAd adNative;
// 	private bool nativeLoaded = false;
// 	[SerializeField] GameObject adNativePanel;
// 	[SerializeField] RawImage adIcon;
// 	//[SerializeField] RawImage adBannerImage;
// 	[SerializeField] RawImage adChoices;
// 	[SerializeField] Text adHeadline;
// 	[SerializeField] Text adCallToAction;
// 	[SerializeField] Text adAdvertiser;


// 	void Awake()
// 	{
// 		adNativePanel.SetActive(false); //hide ad panel

// 	}
//     private void Start()
//     {
// 		AdmobManager.Instance.SetupAds += Setup;
        
//     }
//     void Setup()
// 	{
// 		//idNative = "ca-app-pub-3940256099942544/2247696110";
// 		RequestNativeAd();
// 	}

// 	void Update()
// 	{
// 		if (nativeLoaded)
// 		{
// 			nativeLoaded = false;

// 			Texture2D iconTexture = this.adNative.GetIconTexture();
// 			Texture2D iconAdChoices = this.adNative.GetAdChoicesLogoTexture();
// 			string headline = this.adNative.GetHeadlineText();
// 			string cta = this.adNative.GetCallToActionText();
// 			string advertiser = this.adNative.GetAdvertiserText();
// 			adIcon.texture = iconTexture;
// 			adChoices.texture = iconAdChoices;
// 			adHeadline.text = headline;
// 			adAdvertiser.text = advertiser;
// 			adCallToAction.text = cta;

// 			//register gameobjects
// 			adNative.RegisterIconImageGameObject(adIcon.gameObject);
// 			adNative.RegisterAdChoicesLogoGameObject(adChoices.gameObject);
// 			adNative.RegisterHeadlineTextGameObject(adHeadline.gameObject);
// 			adNative.RegisterCallToActionGameObject(adCallToAction.gameObject);
// 			adNative.RegisterAdvertiserTextGameObject(adAdvertiser.gameObject);

// 			adNativePanel.SetActive(true); //show ad panel
// 		}
// 	}



// 	private void RequestNativeAd()
// 	{

// 		string adUnitId = AdmobManager.Instance.AppNativeAdsIds[Random.Range(0, AdmobManager.Instance.AppNativeAdsIds.Count)];
// 		AdLoader adLoader = new AdLoader.Builder(adUnitId)
// 			.ForNativeAd()
// 			.Build();
// 		adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
// 		adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;
// 		adLoader.LoadAd(new AdRequest());

// 		//AdLoader adLoader = new AdLoader.Builder(idNative).ForNativeAd().Build();
// 		//adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
// 		//adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;
// 		//adLoader.LoadAd(AdRequestBuild());
// 	}

// 	private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
// 	{
// 		Debug.Log("Native ad failed to load: " + args.LoadAdError);
// 	}
// 	//events
// 	private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
// 	{
// 		this.adNative = args.nativeAd;
// 		nativeLoaded = true;
// 	}
// 	AdRequest AdRequestBuild()
// 	{
// 		return new AdRequest();
// 	}

// 	//[SerializeField] GameObject panelMenu;



// 	public void ShowMenu()
// 	{
// 		adNativePanel.SetActive(true);
// 	}

// 	public void HideMenu()
// 	{
// 		adNativePanel.SetActive(false);
// 		RequestNativeAd();
// 	}
// }
