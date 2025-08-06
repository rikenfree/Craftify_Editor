using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using SuperStarSdk;
using Main.Controller;

public class WallPaperManeger : MonoBehaviour
{
    public List<string> wallPapersUrl = new List<string>();
    public Transform content;
    public GameObject wallPaperPrefab;
    public GameObject saveWallPaperSc;
    public GameObject wallPaperSc;
    public Image showImage;
    public string fileName;
    public string savePath;
    [SerializeField]
    public bool _hasItems = true;
    public ScrollRect _scrollRect;
    public int pageNo = 1;

    public static WallPaperManeger Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        
        
    }

    private void Update()
    {
       
    }

    public IEnumerator ImagesLoadAndSet()
    {
        savePath = Application.persistentDataPath + "/McpeWallPaper/";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        for (int i = 0; i < wallPapersUrl.Count; i++)
        {
           //StartCoroutine(ImageDownloadAndSet(wallPapersUrl[i], (i + 1) + ".png"));
        }
        yield return new WaitForSeconds(.5f);
    }

    public void LoadImageToScrollingTime()
    {
        Debug.Log("Check Bool : " + _hasItems);
        if (_scrollRect.normalizedPosition.y < 0.025f && _hasItems)
        {
            pageNo++;
            // Check for additional items and add them to your scrollview
            //StartCoroutine(Wallpaper.Instance.GetAllImages(pageNo));
            // If there are no more items to be loaded, set a class variable
            _hasItems = false;
         Debug.Log("Check Bool : " + _hasItems);
        }
    }

    /*public IEnumerator ImageDownloadAndSet(string URL, string name)
    {
        Debug.Log(URL);
        WWW www = new WWW(URL);
        yield return www;
        Debug.Log("Save Data 1" + savePath);

        AssetCache.Instance.EnqueueOneResAndWait(URL, URL, (success) =>
        {
            if (success)
            {
                Debug.Log("Success : " + 0);
                GameObject imageObj = Instantiate(wallPaperPrefab, content);
                AssetCache.Instance.LoadSpriteIntoImage(imageObj.GetComponent<Image>(), URL, changeAspectRatio: true);
                imageObj.GetComponent<Button>().onClick.AddListener(delegate { OnClickWallPaper(imageObj.GetComponent<Image>().sprite); });
                Debug.Log("Success : " + 1);
            }
        });
    }*/

    public void OnClickWallPaper(Sprite sprite)
    {
        wallPaperSc.SetActive(false);
        showImage.sprite = sprite;
        Debug.Log("Sprite Name : " + sprite.name);
        saveWallPaperSc.SetActive(true);
    }

    public void OnClickCanCelBtn()
    {
        wallPaperSc.SetActive(true);
        saveWallPaperSc.SetActive(false);
        SoundController.instance.PlayClickSound();
    }

    public void OnClickWallPaper()
    {
        wallPaperSc.SetActive(true);
        SoundController.instance.PlayClickSound();
    }

    public void OnClickBackBtn()
    {
        wallPaperSc.SetActive(false);
        SoundController.instance.PlayClickSound();
    }

    public void SaveImageToGallery()
    {
            int i = ImgIndex++;
            byte[] texureBytes = showImage.sprite.texture.EncodeToJPG();
           //File.WriteAllBytes(Application.persistentDataPath + "/" + i + ".jpg", texureBytes);
           Debug.LogError("Path: " + Application.persistentDataPath);

            NativeGallery.SaveImageToGallery(texureBytes, "Wallpaper", "McpeWallpaper", null);

            //Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            //ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            //ss.Apply();
            //Debug.Log("Image:  " + ss);
        //SuperStarAd.Instance.ShowRewardVideo((o) =>
        //{
        //});
    }

    
    int ImgIndex
    {
        get
        {
            return PlayerPrefs.GetInt("ImgIndex", 0);
        }
        set
        {
            PlayerPrefs.SetInt("ImgIndex", value);
        }
    }
}
