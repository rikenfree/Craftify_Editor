using SuperStarSdk;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;
//public enum CrossPromoType {

//    PORTRAIT=0,
//    LANDSCAPE=1
//}

public class SSCrosspromo : MonoBehaviour
{
    [Header("App Open Intrestitial")]
    public bool isAppOpen = false;
    public GameObject Closebtn;


    private const string PlayStoreUrl = "https://play.google.com/store/apps/details?id={0}";
    private const string AppStoreUrl = "https://itunes.apple.com/app/apple-store/id{0}";
    [Header("Cross Box Banner")]
    [SerializeField]
    private VideoPlayer _videoPlayer;
    [SerializeField]
    private GameObject _videoPlayerGameObject;
    [SerializeField]
    private TextMeshProUGUI _gameName;
    [SerializeField]
    private Image _gameIcon;
    [SerializeField]
    private TextMeshProUGUI _gameDescriptiontxt;
    [SerializeField]
    private TextMeshProUGUI _buttontxt;
    [SerializeField]
    private Button _containerButton;
    [SerializeField]
    private RawImage _videoContent;
    [SerializeField]
    private RenderTexture _renderTexture;
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private Button _playButton;
    [Header("0 portrait , 1 LandScape")]
    public string CrosspromoType;
    //[SerializeField]
    //private Material _material;
    public GameObject SSParent;
    private Vector3 _previousLocalScale;
    public SSCrossPromoAsset _currentCrossPromoAsset;
    private static Vector3 ZeroScaleVector = new Vector3(0, 0, 0);

    private bool DidLoad;

    int CurrentVideoIndex = -1;

    private void OnEnable()
    {
        if (!isAppOpen)
        {
            SSParent.SetActive(false);
        }
        //  _container.SetActive(false);
        //  _videoContent.gameObject.SetActive(false);
        SuperStarSdkManager.OnDataArrive += CrossPromoDataAarrived;
        Debug.LogError("on enable called");
        //yield return new WaitForSeconds(5f);
        LoadCrossPromo();
    }

    private void Start()
    {
        _videoPlayer = _videoPlayerGameObject.GetComponent<VideoPlayer>();
        _videoContent.texture = _renderTexture;
        _videoPlayer.targetTexture = _renderTexture;
        _videoPlayer.errorReceived += OnErrorReceived;
        _videoPlayer.prepareCompleted += OnVideoPrepared;
        _videoPlayer.loopPointReached += EndReached;
        _videoPlayer.sendFrameReadyEvents = true;
        SuperStarAd.Instance.adLoader.SetActive(false);
    }
    public void LoadCrossPromo()
    {

        //yield return new WaitForSeconds(5f);
        if (SuperStarSdkManager.Instance.isCrossPromoDataArrived)
        {
            //find cross promo data for this cross promo

            int cross = GiveMeCrossPromoVideoIndex();
            Debug.Log("cross => " + cross);
            if (cross == -1 || SuperStarAd.Instance.NoAds == 1)
            {
                Debug.Log("No video Prepared");
                if (!isAppOpen)
                {
                    SSParent.SetActive(false);

                }
            }
            else
            {

                _currentCrossPromoAsset = SuperStarSdkManager.Instance.crossPromoAssetsRoot.data[cross];
                SetUpCrosspromo();
            }
            if (isAppOpen && (cross == -1 || SuperStarAd.Instance.NoAds == 1))
            {
                Destroy(transform.parent.gameObject);
            }
        }

        if (isAppOpen)
        {
            Closebtn.GetComponent<Button>().onClick.RemoveAllListeners();
            Closebtn.GetComponent<Button>().onClick.AddListener(() => { SuperStarAd.Instance.OnCloseSSIntrestitial(); });
            Closebtn.GetComponent<Button>().onClick.AddListener(() => { Destroy(transform.parent.gameObject); });
            Closebtn.SetActive(false);
            //SuperStarAd.Instance.adLoader.SetActive(false);
        }

    }

    public void OnInfoButtonClicked()
    {


    }

    public List<int> data = new List<int>();
    public int GiveMeCrossPromoVideoIndex()
    {

        Debug.Log("GiveMeCrossPromoVideoIndex");

        for (int i = 0; i < SuperStarSdkManager.Instance.crossPromoAssetsRoot.data.Count; i++)
        {
            for (int z = 0; z < SuperStarSdkManager.Instance.crossPromoAssetsRoot.data[i].appvideourl.Count; z++)
            {
                if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.data[i].appvideourl[z].isDownloaded)
                {
                    if (!data.Contains(i))
                    {
                        data.Add(i);
                    }
                }
            }
            //else if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.data[i].isVideoDownloaded && SuperStarSdkManager.Instance.crossPromoAssetsRoot.data[i].appvideotype == CrosspromoType)
            //{
            //    data.Add(i);
            //}
        }
        Debug.Log("GiveMeCrossPromoVideoIndex ======= " + data.Count);
        if (data.Count > 1)
        {
            int x = data[UnityEngine.Random.Range(0, data.Count)];


            CurrentVideoIndex = x;
            return x;

        }
        else if (data.Count == 1)
        {
            CurrentVideoIndex = 0;
            return 0;
        }
        else
        {
            return -1;
        }
    }

    public int GiveMeCrossPromoVideoURLIndex()
    {
        List<int> dummyIndex = new List<int>();

        for (int z = 0; z < _currentCrossPromoAsset.appvideourl.Count; z++)
        {
            if (_currentCrossPromoAsset.appvideourl[z].isDownloaded)
            {
                dummyIndex.Add(z);
            }
        }

        return dummyIndex[UnityEngine.Random.Range(0, dummyIndex.Count)];
        return -1;
        //else if (SuperStarSdkManager.Instance.crossPromoAssetsRoot.data[i].isVideoDownloaded && SuperStarSdkManager.Instance.crossPromoAssetsRoot.data[i].appvideotype == CrosspromoType)
        //{
        //    data.Add(i);
        //}


    }

    private void OnDisable()
    {
        SuperStarSdkManager.OnDataArrive -= CrossPromoDataAarrived;
    }


    public void CrossPromoDataAarrived()
    {

        int cross = GiveMeCrossPromoVideoIndex();

        if (cross == -1)
        {
            Debug.Log("No video Prepared");
            if (!isAppOpen)
            {
                SSParent.SetActive(false);
            }


        }
        else
        {

            _currentCrossPromoAsset = SuperStarSdkManager.Instance.crossPromoAssetsRoot.data[cross];
            SetUpCrosspromo();
        }
        if (isAppOpen && (cross == -1 || SuperStarAd.Instance.NoAds == 1))
        {
            Destroy(transform.parent.gameObject);
        }
        //Debug.LogError("Data arrived Load Video");
        //_currentCrossPromoAsset = SuperStarSdkManager.Instance.crossPromoAssetsRoot.crossPromoAssets[0];
        //SetUpCrosspromo();
        // _container.SetActive(true);
    }

    private void SetUpCrosspromo()
    {
        _containerButton.onClick.RemoveAllListeners();
        _playButton.onClick.RemoveAllListeners();
        Debug.LogError("SetUpCrosspromo");
        _containerButton.onClick.AddListener(OnClickEvent);
        _playButton.onClick.AddListener(OnClickEvent);
        //SuperStarAd.Instance.adLoader.SetActive(false);

        _renderTexture.Create();

        LoadVideo();
        LoadIcon();
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        if (!isAppOpen)
        {
            LoadCrossPromo();
        }
    }
    private void OnErrorReceived(VideoPlayer source, string message)
    {
        StartCoroutine(PlayNextVideo());
        Debug.LogError("Error recieved ");
    }

    private void CleanTexture()
    {
        _renderTexture.Release();
        _renderTexture.DiscardContents();
    }
    private void OnVideoPrepared(VideoPlayer source)
    {

        Debug.LogError("Video Prepared");
        CleanTexture();
        // source.frameReady += OnFrameReady;
        source.Play();
        _videoContent.gameObject.SetActive(true);
        //  Show();
    }

    //private void Show()
    //{
    //    Debug.Log("Video Show");
    //    if (_previousLocalScale != null)
    //    {
    //        _container.transform.localScale = _previousLocalScale;
    //    }
    //}

    //private void OnFrameReady(VideoPlayer source, long frameIdx)
    //{
    //    if (source.frame >= 0)
    //    {
    //        source.frameReady -= OnFrameReady;
    //    }
    //}

    public void OnClickEvent()
    {
        if (_currentCrossPromoAsset != null)
        {

#if UNITY_ANDROID
            Application.OpenURL(_currentCrossPromoAsset.Aappstoreid);
            SSEventManager.Instance.SSOnPressCrossPromoBox(_currentCrossPromoAsset.appname, "Android");
#elif  UNITY_IOS
            Application.OpenURL(_currentCrossPromoAsset.Iappstoreid);
            SSEventManager.Instance.SSOnPressCrossPromoBox(_currentCrossPromoAsset.appname, "IOS");

#endif
        }
    }

    void LoadVideo()
    {
        int crosspromoindex = GiveMeCrossPromoVideoURLIndex();
        Debug.Log("Load video" + Application.persistentDataPath + "/" + _currentCrossPromoAsset.appname + _currentCrossPromoAsset.appvideourl[crosspromoindex].name + ".mp4");
        _videoPlayer.url = Application.persistentDataPath + "/" + _currentCrossPromoAsset.appname + _currentCrossPromoAsset.appvideourl[crosspromoindex].name + ".mp4";
        _gameName.text = _currentCrossPromoAsset.appname;
        _container.SetActive(true);
        SSParent.SetActive(true);
        _videoPlayer.Prepare();
        if (isAppOpen)
        {
            StartCoroutine(closeCoroutine());
        }
    }

    void LoadIcon()
    {
        StartCoroutine(DownloadImageCoroutine(_currentCrossPromoAsset.appiconurl.url));
    }

    private IEnumerator DownloadImageCoroutine(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                // Convert Texture2D to Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Set the sprite to the Image
                _gameIcon.sprite = sprite;
            }
            else
            {
                Debug.LogError($"Failed to download image: {request.error}");
            }
        }
    }

    IEnumerator closeCoroutine()
    {

        yield return new WaitForSeconds(3f);
        Closebtn.SetActive(true);
        //SuperStarAd.Instance.adLoader.SetActive(false);
    }

    private IEnumerator PlayNextVideo()
    {
        while (!DidLoad)
        {
            yield return null;
        }
        //if (!SupersonicWisdom.HasCrossPromo(Format))
        //{
        //    Hide();
        //    yield break;
        //}
        // StartCoroutine(SuperStarSdk.StartInitialDownload(Format));

        while (!isActiveAndEnabled)
        {
            yield return null;
        }
        if (isActiveAndEnabled)
        {
            yield return StartCoroutine(LoadNextVideo());
        }
        if (isActiveAndEnabled)
        {
            Play();
        }
    }
    IEnumerator LoadNextVideo()
    {
        //if (SuperStarSdk.HasCrossPromo(Format))
        //{
        //    if (_videoPlayer != null)
        //    {
        //        if (_videoPlayer.isPlaying)
        //        {
        //            yield break;
        //        }
        //    }

        //    SuperStarSdk.SwCrossPromoAsset currentAsset;
        //    do
        //    {
        //        currentAsset = SuperStarSdk.GetNextCrossPromoAsset(Format);
        //        if (!isActiveAndEnabled)
        //        {
        //            yield break;
        //        }
        //        if (currentAsset == null)
        //        {
        yield return null;
        //        }
        //    } while (currentAsset == null);
        //    _currentCrossPromoAsset = currentAsset;
        //    _videoPlayer.url = currentAsset.cachePath;
        //    _gameName.text = currentAsset.name;
        //}
    }

    private void Play()
    {
        if (_videoPlayer != null)
        {
            if (String.IsNullOrEmpty(_videoPlayer.url) || _videoPlayer.isPlaying)
            {
                return;
            }

            if (!_videoPlayer.isPrepared)
            {
                _videoPlayer.Prepare();
            }
        }

    }

    private void OnDestroy()
    {

        _containerButton.onClick.RemoveAllListeners();
        _containerButton.onClick.RemoveAllListeners();
        _playButton.onClick.RemoveAllListeners();
    }
}
