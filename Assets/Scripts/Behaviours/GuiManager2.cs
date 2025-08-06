
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperStarSdk;
using System;
using System.Collections;

public class GuiManager2 : MonoBehaviour
{


    public static GuiManager2 instance;

    [SerializeField] internal Sprite LanguageSelect;
    [SerializeField] internal Sprite LanguageDeselect;

    public GameObject homeButton;

	public GameObject tagsButton;

	public GameObject SkinsButton;

	public GameObject favouriteButton;

	public GameObject searchButton;

	public GameObject menuButton;

	public GameObject detailsPanel;

	public GameObject skinDownloadPanel;

	public Text mapNameText;

	public Image mapImage;
	public RawImage skinImage;

	public MapData2 currentMapData;

	public GameObject searchInputPanel;

	public InputField searchText;

	public Image favouriteImage;

	public Text favouriteText;

	public Sprite favouriteSprite;

	public Sprite unfavouriteSprite;

	public GameObject homePanel;

	public GameObject homeBottom;

	public GameObject tagsPanel;
	public GameObject SkinsPanel;

	public GameObject tagsTagsPanel;

	public GameObject tagsTagsGrid;

	public GameObject tagsTagsBottom;

	public GameObject tagsMapsPanel;

	public GameObject tagsMapsGrid;

	public GameObject tagsMapsBottom;

	public Text tagsMapsBottomText;

	public TagView2 tagViewPrefab;

	public MapView2 mapViewPrefab;

	public GameObject favouritePanel;

	public GameObject favouriteMapsGrid;

	public GameObject favouriteBottom;

	public GameObject searchResultPanel;

	public GameObject searchResultMapsGrid;

	public GameObject searchResultBottom;

	public Text searchResultBottomText;

	public GameObject downloadProgressPanel;

	public Text downloadProgressTitle;

	public RectTransform downloadProgressBar;

	public Text downloadProgressText;

	public GameObject playPanel;

	public GameObject ratePanel;

	public GameObject menuPanel;

	public GameObject selectLanguagePanel;

	public Text pageText;

	public Text debugText;

	public GameObject profileScreen;
	public GameObject LoginScreen;
	public Image playerProfileImage;
	public Sprite degultProfile;
	public Text playerName;

	public GameObject errorPopup;

	public GameObject BottomPanel;

    private I2.Loc.Localize favouriteLocalize;

    public int PlayTime
	{
		get
		{
			return (PlayerPrefs.GetInt("PlayTime", 0));
		}
		set
		{
			PlayerPrefs.SetInt("PlayTime", value);
		}
	}

    public int Rate
    {
        get
        {
            return (PlayerPrefs.GetInt("Rate", 0));
        }
        set
        {
            PlayerPrefs.SetInt("Rate", value);
        }
    }

	public int Login
	{
		get
		{
			return PlayerPrefs.GetInt("Login", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Login", value);
		}
	}
	public string PlayerName
	{
		get
		{
			return (PlayerPrefs.GetString("PlayerName", "player"));
		}
		set
		{
			PlayerPrefs.SetString("PlayerName", value);
		}
	}
	public int LoginAsGuest   //0 = guest ,1 = fb
	{
		get
		{
			return PlayerPrefs.GetInt("LoginAsGuest", 0);
		}
		set
		{
			PlayerPrefs.SetInt("LoginAsGuest", value);
		}
	}
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(base.gameObject);
		}

		favouriteLocalize = favouriteText.GetComponent<I2.Loc.Localize>();
    }

    private void Start()
	{
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ShowHomeTab();
        PlayTime += 1;
	}
   

    public void HomeButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		ShowHomeTab();
		BottomPanel.SetActive(true);
        SuperStarAd.Instance.ShowInterstitialTimer(null);
	//	AdMobAdManager.Instance.ShowInterstitialIfAndItsTime();
	}

	public void TagsButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		ShowTagsTab();
		BottomPanel.SetActive(true);
        SuperStarAd.Instance.ShowInterstitialTimer(null);
        //AdMobAdManager.Instance.ShowInterstitialIfAndItsTime();
	}
	public void SkinsButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		ShowSkinsTab();
		BottomPanel.SetActive(false);
		SuperStarAd.Instance.ShowInterstitialTimer(null);
		
		//AdMobAdManager.Instance.ShowInterstitialIfAndItsTime();
	}

	public void FavouriteButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		ShowFavouriteTab();
		BottomPanel.SetActive(true);
        SuperStarAd.Instance.ShowInterstitialTimer(null);
      //  AdMobAdManager.Instance.ShowInterstitialIfAndItsTime();
	}

	public void SearchButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		ShowSearchInputPanel();
		SuperStarAd.Instance.ShowInterstitialTimer(null);

        //AdMobAdManager.Instance.ShowInterstitialIfAndItsTime();
	}

	public void MenuButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		ShowMenuPanel();
        SuperStarAd.Instance.ShowInterstitialTimer(null);
        //AdMobAdManager.Instance.ShowInterstitialIfAndItsTime();
	}

	private void HighLightButton(GameObject button)
	{
		ChangeButtonColor("#958181", button);
	}

	private void UnhighLightButton(GameObject button)
	{
		ChangeButtonColor("#FFFFFF00", button);
	}

	private void ChangeButtonColor(string hexColor, GameObject button)
	{
		ColorUtility.TryParseHtmlString(hexColor, out Color color);
		button.GetComponent<Image>().color = color;
	}

	public void ShowDetailsPanel(Sprite mapSprite, MapData2 mapData)
	{
		currentMapData = mapData;
		detailsPanel.SetActive(value: true);
		mapNameText.text = mapData.Name;
		mapImage.sprite = mapSprite;
		RefreshFavouriteButton();
	}

	public void ShowSkinDownloadPanel(Texture2D skin)
	{
		skinImage.texture = skin;
		skinDownloadPanel.SetActive(value: true);
		///RefreshFavouriteButton();
	}
	public void CloseSkinDownloadPanel()
	{
		skinDownloadPanel.SetActive(value: false);
	}

	public void CloseDetailsPanelButtonClick()
	{
		SoundManager2.instance.PlayButtonSound();
		currentMapData = null;
		CloseDetailsPanel();
	}

	public void CloseDetailsPanel()
	{
		detailsPanel.SetActive(value: false);
	}

	public void ClearSearchTextClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		ClearSearchText();
	}

	public void ClearSearchText()
	{
		searchText.text = "";
	}

	public void ShowSearchInputPanel()
	{
		searchInputPanel.SetActive(value: true);
	}

	public void CloseSearchInputPanelClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		CloseSearchInputPanel();
	}

	public void CloseSearchInputPanel()
	{
		ClearSearchText();
		searchInputPanel.SetActive(value: false);
	}

	public void SearchMapsClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		SearchMaps();
	}

	public void SearchMaps()
	{
		string text = searchText.text.Trim().ToLower();
		if (!(text == ""))
		{
			CloseSearchInputPanel();
			ShowSearchResultPanel(text);
		}
	}

	public void RefreshFavouriteButton()
	{
        if (StorageManager2.instance.HasFavouriteMapId(currentMapData.Id))
        {
            favouriteImage.sprite = unfavouriteSprite;
            favouriteLocalize.SetTerm("Unfavourite");
        }
        else
        {
            favouriteImage.sprite = favouriteSprite;
            favouriteLocalize.SetTerm("Favourite");
        }
    }

	public void AddFavouriteButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		SuperStarAd.Instance.ShowInterstitialTimer(null);
		if (StorageManager2.instance.HasFavouriteMapId(currentMapData.Id))
		{
			StorageManager2.instance.RemoveFavouriteMapId(currentMapData.Id);
		}
		else
		{
			StorageManager2.instance.AddFavouriteMapId(currentMapData.Id);
		}
		RefreshFavouriteButton();
		RefreshFavouriteMaps();
	}

    //public void DownloadButtonClicked()
    //{
    //	SoundManager2.instance.PlayButtonSound();

    //       InternetCheckingManager.Instance._connectionTester.TestInternet(hasInternet =>
    //       {
    //           if (!hasInternet)
    //           {
    //               MapsManager2.instance.ShowNoInternetToast();
    //           }
    //           else
    //           {
    //			//SuperStarAd.Instance.adCount= 0;
    //			SuperStarAd.Instance.ShowForceInterstitialWithLoader((result) => 
    //			{
    //				if (result)
    //				{

    //					DownloadManager2.instance.DownloadMap(currentMapData);
    //				}
    //				else
    //				{
    //					ToastManager.Instance.ShowTost("AD is not available");
    //				}
    //			}, 3);
    //           }

    //       });


    //}


    public void DownloadButtonClicked()
    {
        SoundManager2.instance.PlayButtonSound();
        //InternetCheckingManager.Instance._connectionTester.TestInternet(hasInternet =>
        //{
        if (!InternetCheckingManager.Instance.isinternetavailable)
        {
            MapsManager2.instance.ShowNoInternetToast();
        }
        else
        {
            //SuperStarAd.Instance.adCount= 0;
            if (SuperStarAd.Instance.NoAds == 0)
            {
                SuperStarAd.Instance.ShowForceInterstitialWithLoader((_) =>
                {
                    // Proceed to download no matter the ad result
                    DownloadManager2.instance.DownloadMap(currentMapData);
                }, 3);
            }
            else
            {
                DownloadManager2.instance.DownloadMap(currentMapData);
            }
        }
    }

    public void ShowHomeTab()
	{
		homePanel.SetActive(value: true);
		homeBottom.SetActive(value: true);
		HighLightButton(homeButton);
		tagsPanel.SetActive(value: false);
		tagsTagsBottom.SetActive(value: false);
		tagsMapsBottom.SetActive(value: false);
		UnhighLightButton(tagsButton);
		favouritePanel.SetActive(value: false);
		favouriteBottom.SetActive(value: false);
		UnhighLightButton(favouriteButton);
		searchResultPanel.SetActive(value: false);
		searchResultBottom.SetActive(value: false);
		UnhighLightButton(searchButton);
		SkinsPanel.SetActive(value: false);
		UnhighLightButton(SkinsButton);
	}

	public void ShowTagsTab()
	{
		homePanel.SetActive(value: false);
		homeBottom.SetActive(value: false);
		UnhighLightButton(homeButton);
		tagsPanel.SetActive(value: true);
		ShowTagsTags();
		HighLightButton(tagsButton);
		favouritePanel.SetActive(value: false);
		favouriteBottom.SetActive(value: false);
		UnhighLightButton(favouriteButton);
		searchResultPanel.SetActive(value: false);
		searchResultBottom.SetActive(value: false);
		UnhighLightButton(searchButton);

		SkinsPanel.SetActive(value: false);
		UnhighLightButton(SkinsButton);
	}

	public void ShowSkinsTab()
	{
		homePanel.SetActive(value: false);
		homeBottom.SetActive(value: false);
		UnhighLightButton(homeButton);
		tagsPanel.SetActive(value: false);
		UnhighLightButton(tagsButton);
		//ShowTagsTags();
		SkinsPanel.SetActive(value: true);
		HighLightButton(SkinsButton);
		favouritePanel.SetActive(value: false);
		favouriteBottom.SetActive(value: false);
		UnhighLightButton(favouriteButton);
		searchResultPanel.SetActive(value: false);
		searchResultBottom.SetActive(value: false);
		UnhighLightButton(searchButton);
	}

	public void ShowTagsTags()
	{
		tagsTagsPanel.SetActive(value: true);
		tagsTagsBottom.SetActive(value: true);
		tagsMapsPanel.SetActive(value: false);
		tagsMapsBottom.SetActive(value: false);
	}

	public void CloseTagsMapsClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		CloseTagsMaps();
	}

	public void CloseTagsMaps()
	{
		ShowTagsTags();
	}

	public void ShowTagsMapsByTag(string tag)
	{
		tagsTagsPanel.SetActive(value: false);
		tagsTagsBottom.SetActive(value: false);
		tagsMapsPanel.SetActive(value: true);
		tagsMapsBottom.SetActive(value: true);
		tagsMapsBottomText.text = "TAG: " + tag;
		List<MapData2> list = MapsManager2.instance.MapsDataByTag(tag);
		for (int i = 0; i < tagsMapsGrid.transform.childCount; i++)
		{
			Destroy(tagsMapsGrid.transform.GetChild(i).gameObject);
		}
		foreach (MapData2 item in list)
		{
			Instantiate(mapViewPrefab, tagsMapsGrid.transform).SetData(item);
		}
	}

	public void PopulateTags(List<TagData2> tagsData)
	{
		foreach (TagData2 tagsDatum in tagsData)
		{
			Instantiate(tagViewPrefab, tagsTagsGrid.transform).SetName(tagsDatum.Name);
		}
	}

	public void UpdatePage(int page)
	{
		pageText.text = page.ToString();
	}

	public void UpdatePage(string str)
	{
		pageText.text = str;
	}

	public void ShowFavouriteTab()
	{
		homePanel.SetActive(value: false);
		homeBottom.SetActive(value: false);
		UnhighLightButton(homeButton);
		tagsPanel.SetActive(value: false);
		tagsTagsBottom.SetActive(value: false);
		tagsMapsBottom.SetActive(value: false);
		UnhighLightButton(tagsButton);
		favouritePanel.SetActive(value: true);
		favouriteBottom.SetActive(value: true);
		HighLightButton(favouriteButton);
		RefreshFavouriteMaps();
		searchResultPanel.SetActive(value: false);
		searchResultBottom.SetActive(value: false);
		UnhighLightButton(searchButton);
		SkinsPanel.SetActive(value: false);
		UnhighLightButton(SkinsButton);
	}

	public void RefreshFavouriteMaps()
	{
		if (favouritePanel.activeSelf && favouriteMapsGrid.activeSelf)
		{
			string[] ids = StorageManager2.instance.LoadFavouriteMapIds();
			List<MapData2> list = MapsManager2.instance.MapsDataByIds(ids);
			for (int i = 0; i < favouriteMapsGrid.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(favouriteMapsGrid.transform.GetChild(i).gameObject);
			}
			foreach (MapData2 item in list)
			{
                UnityEngine.Object.Instantiate(mapViewPrefab, favouriteMapsGrid.transform).SetData(item);
            }
		}
	}

	public void ShowSearchResultPanel(string keyword)
	{
		homePanel.SetActive(value: false);
		homeBottom.SetActive(value: false);
		UnhighLightButton(homeButton);
		tagsPanel.SetActive(value: false);
		tagsTagsBottom.SetActive(value: false);
		tagsMapsBottom.SetActive(value: false);
		UnhighLightButton(tagsButton);
		favouritePanel.SetActive(value: false);
		favouriteBottom.SetActive(value: false);
		UnhighLightButton(favouriteButton);
		searchResultPanel.SetActive(value: true);
		searchResultBottom.SetActive(value: true);
		HighLightButton(searchButton);
		RefreshSearchResultMaps(keyword);
		SkinsPanel.SetActive(value: false);
		UnhighLightButton(SkinsButton);
	}

	public void RefreshSearchResultMaps(string keyword)
	{
		if (searchResultPanel.activeSelf && searchResultMapsGrid.activeSelf)
		{
			List<MapData2> list = MapsManager2.instance.MapsDataByKeyword(keyword);
			for (int i = 0; i < searchResultMapsGrid.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(searchResultMapsGrid.transform.GetChild(i).gameObject);
			}
			foreach (MapData2 item in list)
			{
				UnityEngine.Object.Instantiate(mapViewPrefab, searchResultMapsGrid.transform).SetData(item);
			}
			searchResultBottomText.text = "Maps Search: " + keyword;
		}
	}

	public void InitDownloadProgressPanel(string title)
	{
        tempCount = 0;
        downloadProgressBar.GetComponent<Image>().fillAmount = 0;
        downloadProgressTitle.text = title;
		downloadProgressText.text = "0 %";
		//downloadProgressBar.sizeDelta = new Vector2(0f, downloadProgressBar.sizeDelta.y);
		downloadProgressPanel.SetActive(true);
	}

    float tempCount = 0;
    public void UpdateDownloadProgress(float percent)
    {
        Debug.Log("📦 Download Progress: " + (int)(percent * 100) + "%");

        StopCoroutine("AnimateProgressBar");
        StartCoroutine(AnimateProgressBar(percent));
    }

    private IEnumerator AnimateProgressBar(float target)
    {
		Image bar = downloadProgressBar.GetComponent<Image>();
        float start = bar.fillAmount;
        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            bar.fillAmount = Mathf.Lerp(start, target, t);
            yield return null;
        }

        bar.fillAmount = target;
        downloadProgressText.text = Mathf.RoundToInt(target * 100f) + "%";
    }

    public void FinishDownloadProgress()
    {
        StopAllCoroutines(); // stop any ongoing animation
        downloadProgressBar.GetComponent<Image>().fillAmount = 1f;
        downloadProgressText.text = "100%";

        // Optionally hide the whole panel if you use one
        if (downloadProgressPanel != null)
            downloadProgressPanel.SetActive(false);
    }


    public void CancelLoadingMapButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		DownloadManager2.instance.StopDownload();
		downloadProgressPanel.SetActive(value: false);
	}

	public void CompleteDownloadProgress()
	{
        downloadProgressBar.GetComponent<Image>().fillAmount = 1;

        downloadProgressText.text = "Download Complete";
		downloadProgressPanel.SetActive(false);
		playPanel.SetActive(true);
        if (Rate!=1)
        {

        ratePanel.SetActive(true);
        }
	}

	public void CancelPlayMinecraftButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		playPanel.SetActive(value: false);
	}

    //	public void PlayMinecraftButtonClicked()
    //	{
    //		SoundManager2.instance.PlayButtonSound();
    //		//  AndroidContentOpenerWrapper.OpenContent(DownloadManager.instance.lastOpenFileLink);
    //		//RunApk("com.mojang.minecraftpe");
    //		string DirPath = "";
    //#if UNITY_EDITOR
    //        DirPath = Application.persistentDataPath + "/MapForMCPE/";
    //#elif UNITY_ANDROID
    //        DirPath = "/storage/emulated/0/Download/MapForMCPE/";
    //#endif
    //        string path = DirPath + DownloadManager2.WORLD_FOLDER + "/" + currentMapData.Id + ".mcworld";
    //		Debug.Log(path);

    //		AndroidContentOpenerWrapper.OpenContent(path);
    //		//Singleton<AndroidNativeUtility>.instance.StartApplication("com.mojang.minecraftpe");
    //	}

    public void PlayMinecraftButtonClicked()
    {
        SoundManager2.instance.PlayButtonSound();

        string DirPath = "";
#if UNITY_EDITOR
        DirPath = Application.persistentDataPath + "/MapForMCPE/";
#elif UNITY_ANDROID
    DirPath = "/storage/emulated/0/Download/MapForMCPE/";
#endif

        string path = DirPath + DownloadManager2.WORLD_FOLDER + "/" + currentMapData.Id + ".mcworld";
        Debug.Log("Trying to open: " + path);

        try
        {
            if (System.IO.File.Exists(path))
            {
                AndroidContentOpenerWrapper.OpenContent(path);
            }
            else
            {
                ToastManager.Instance.ShowToast("⚠ File not found!");
                Debug.LogWarning("File not found: " + path);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("❌ Failed to open file: " + ex.Message);
            ToastManager.Instance.ShowToast("Permission denied or file error.");
        }
    }

    public void RunApk(string package)
    {
        string bundleId = package; // your target bundle id
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        //if the app is installed, no errors. Else, doesn't get past next line
        AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);

        ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
    }
    public void ButtonMenuShareClicked()
    {
        // Debug.Log("path" + Application.dataPath + "/Texture2D/minecraft-egypt-adventure-map-1024x684_orig.png");
        new NativeShare().AddFile(Application.dataPath + "/Texture2D/minecraft-egypt-adventure-map-1024x684_orig.png")
        .SetSubject("Map FOr MinecraftMCPE").SetText("Hey!, here is amzing maps For Minecraft Did You Check it? https://play.google.com/store/apps/details?id=com.MapsforMinecraft.MCPE")
        .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        .Share();

    }

	public void ButtonMenuSelectLanguageClicked()
	{
		selectLanguagePanel.SetActive(true);
        menuPanel.SetActive(value: false);
    }

    public void ButtonMenuProfileClicked()
	{
		profileScreen.SetActive(true);
	}
	public void OpenRatingPanel()
	{
		ratePanel.SetActive(value: true);
	}

	public void CancelRateButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		ratePanel.SetActive(value: false);
	}

	public void RateButtonClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		ratePanel.SetActive(value: false);
		SuperStarSdkManager.Instance.Rate();
	}

	public void ShowMenuPanel()
	{
		menuPanel.SetActive(value: true);
	}

	public void ButtonMenuRateClicked()
	{
        SoundManager2.instance.PlayButtonSound();
        ratePanel.SetActive(value: false);
        SuperStarSdkManager.Instance.Rate();
    }

	public void ButtonMenuMoreClicked()
	{
	}

	public void ButtonMenuFollowClicked()
	{
	}

	public void ButtonMenuPrivacyClicked()
	{
        Application.OpenURL(SuperStarSdkManager.Instance.crossPromoAssetsRoot.PrivacyPolicy);
    }

	public void ButtonMenuCloseClicked()
	{
		SoundManager2.instance.PlayButtonSound();
		menuPanel.SetActive(value: false);
	}

    public void ButtonSelectLanguageCloseClicked()
    {
        SoundManager2.instance.PlayButtonSound();
        menuPanel.SetActive(value: true);
		selectLanguagePanel.SetActive(value: false);
    }

    public void AddDebugString(string s)
	{
		debugText.text = debugText.text + "\n" + s;
	}
	int randomNumber;
	public void OnClickPlayAsGuestButton()
    {
		GetRandomNumber();
		PlayerName = "Guest" + randomNumber.ToString();
		playerName.text = PlayerName;
		playerProfileImage.sprite = degultProfile;
		LoginScreen.SetActive(false);
		Login = 1;
		LoginAsGuest = 0;

	}
	public void GetRandomNumber()
    {
        randomNumber = UnityEngine.Random.Range(100000, 999999);


    }
    public void OnClickLogout()
	{
		//if (LoginAsGuest == 0)
		//{
			
		//}
		//else if (LoginAsGuest == 1)
		//{
		//	FaceBookManager.Instance.FacebookLogout();
		//}
		profileScreen.SetActive(false);
		//LoginScreen.SetActive(true);
		PlayerName = "Player";
		playerProfileImage.sprite = degultProfile;
		Login = 0;
	}

    public void OnClickIAPButton()
    {
        InAppPurchase.Instance.LimitedNoAds();
    }
}
