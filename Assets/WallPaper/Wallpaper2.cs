using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using Newtonsoft.Json;
using System.IO;
using SuperStarSdk;
using TMPro;
using SuperStar.Helpers;

public class Wallpaper2 : MonoBehaviour
{
    public static Wallpaper2 Instance;
    // Path to the images on disk
    public string regularImagePath = "D:\\McpeWallPaperHd\\MyWallPaper";
    public string image4KPath = "D:\\McpeWallPaper4K\\MyWallPaper4k";

    // URL to your PHP upload script
    private string apiUrl = "https://velocitytechnosoft.com/API/wallpaperApi/wallpapermanagement.php";
    //private string apiUrl = "https://goldygamestudio.com/wallpaperApi/wallpapermanagement.php";

    //private string apiUrl = "http://yourserver.com/wallpaper_script.php";

    public List<ImageData> wallpapers = new List<ImageData>();

    public int imageCount;
    public GameObject loader;
    public Image progressImage;
    public GameObject progressView;
    public bool isShowVideo = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        //StartCoroutine(UploadOrUpdateImage(regularImagePath, image4KPath, "My Wallpaper", 0));
        // Set ID > 0 to update
        // Get first page
        StartCoroutine(GetAllImages(1));
        //StartCoroutine(GetImageById(1)); // Get image with ID 1
    }

    public IEnumerator DelaytoCall()
    {
        yield return new WaitForSeconds(.5f);
        
    }

    IEnumerator UploadOrUpdateImage(string regularImagePath, string image4KPath, string title, int id)
    {
        Debug.Log("Move to next " + regularImagePath);
        WWWForm form = new WWWForm();
        byte[] regularImageData = File.ReadAllBytes(regularImagePath);
        form.AddBinaryData("regularImage", regularImageData, System.IO.Path.GetFileName(regularImagePath));
        Debug.Log("Move to next 1 " + regularImagePath);
        byte[] image4KData = File.ReadAllBytes(image4KPath);
        form.AddBinaryData("image4K", image4KData, System.IO.Path.GetFileName(image4KPath));
        Debug.Log("Move to next 2" + regularImagePath);
        form.AddField("title", title);
        if (id > 0) form.AddField("id", id.ToString());
        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Upload/Update Response: " + www.downloadHandler.text);
                wallpapers = JsonConvert.DeserializeObject<List<ImageData>>(www.downloadHandler.text);
                StartCoroutine(ImagesLoadAndSet());
            }
        }
    }

    public IEnumerator GetAllImages(int page)
    {
       
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl + "?page=" + page))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("All Images Response: " + www.downloadHandler.text);
                imageCount = 0;
                wallpapers = JsonConvert.DeserializeObject<List<ImageData>>(www.downloadHandler.text);
                Debug.Log("wallpapers : " + wallpapers.Count);
                StartCoroutine(ImagesLoadAndSet());
            }
        }
    }

    IEnumerator GetImageById(int id)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl + "?id=" + id))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Image By ID Response: " + www.downloadHandler.text);
            }
        }
    }

    string selectedImgName;

    public void OnClickWallPaper(Sprite sprite, string img)
    {
        WallPaperManeger2.Instance.wallPaperSc.SetActive(false);
        WallPaperManeger2.Instance.showImage.sprite = sprite;
        selectedImgName = img;
        Debug.Log("Sprite Name : " + WallPaperManeger2.Instance.showImage.sprite.name);
        WallPaperManeger2.Instance.saveWallPaperSc.SetActive(true);
    }


    public IEnumerator ImagesLoadAndSet()
    {

        for (int i = 0; i < wallpapers.Count; i++)
        {
            //wallpapers[i].image_path = regularImagePath + (i+1) + ".png";
            //wallpapers[i].image_4k_path = image4KPath;
            StartCoroutine(ImageDownloadAndSet(wallpapers[i].image_path, (i + 1) + ".png", wallpapers[i].title));
        }

        while (wallpapers.Count != imageCount)
        {
            yield return new WaitForSeconds(.3f);
        }
        WallPaperManeger2.Instance._hasItems = true;
    }

    public IEnumerator ImageDownloadAndSet(string URL, string name, string imgName)
    {
        Debug.Log("name "+ URL);
        WWW www = new WWW(URL);
        yield return www;

        AssetCache2.Instance.EnqueueOneResAndWait(URL, URL, (success) =>
        {
            if (success)
            {
                Debug.Log("Success : " + 0);
                GameObject imageObj = Instantiate(WallPaperManeger2.Instance.wallPaperPrefab, WallPaperManeger2.Instance.content);
                //imageObj.GetComponent<ButtonData>().ImgName = imgName;
                Debug.Log("Images Count : " + wallpapers.Count);
                AssetCache2.Instance.LoadSpriteIntoImage(imageObj.GetComponent<Image>(), URL, changeAspectRatio: true);
                imageObj.GetComponent<Button>().onClick.AddListener(delegate { OnClickWallPaper(imageObj.GetComponent<Image>().sprite, imgName); });
                Debug.Log("Success : " + 1);
                imageCount++;
            }
        });
    }

    public void LoadImageToScrollingTime()
    {
        Debug.Log("Check Bool : " + WallPaperManeger2.Instance._hasItems);

        if (WallPaperManeger2.Instance._scrollRect.normalizedPosition.y < 0.025f && WallPaperManeger2.Instance._hasItems)
        {
           
            WallPaperManeger2.Instance.pageNo++;
            // Check for additional items and add them to your scrollview
            Debug.Log("Page NO : " + WallPaperManeger2.Instance.pageNo);
            StartCoroutine(GetAllImages(WallPaperManeger2.Instance.pageNo));
            // If there are no more items to be loaded, set a class variable
            WallPaperManeger2.Instance._hasItems = false;
            Debug.Log("Check Bool : " + WallPaperManeger2.Instance._hasItems);
        }
    }

    private string imagetype = "4k";
    public void SaveImageToGallery(string path)
    { 
        if(path == imagetype)
        {
            Debug.Log("4k");
            SuperStarAd.Instance.ShowRewardVideo((o) => { StartCoroutine(DownloadWallPaperImageWithURL(path)); });
        }
        else
        {
            Debug.Log("regular");
            SuperStarAd.Instance.ShowInterstitialTimer((result) => { StartCoroutine(DownloadWallPaperImageWithURL(path)); });
        }
    }


    public IEnumerator DownloadWallPaperImageWithURL(string path)
    {
        var imgUrl = "https://velocitytechnosoft.com/API/wallpaperApi/uploads/image" + path + "/" + selectedImgName + ".png";
        Debug.Log("Download Image Url : " + imgUrl);
        loader.SetActive(true);
        StartCoroutine(DownloadProgress(imgUrl));
        Debug.Log(imgUrl);
        WWW www = new WWW(imgUrl);
        yield return www;
        //File.WriteAllBytes(Application.dataPath + "/Resources/Logo/" + name, www.bytes);
        Texture2D texture = new Texture2D(1080, 1920, TextureFormat.RGB24, false);
        www.LoadImageIntoTexture(texture);
        NativeGallery.SaveImageToGallery(texture, "McpeWallPapers" , selectedImgName, null);
    }


    public IEnumerator DownloadProgress(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SendWebRequest();

        while (!www.isDone)
        {
            Debug.Log("Progress : " + (int)(www.downloadProgress * 100f) + "%");
            progressView.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (int)(www.downloadProgress * 100f) + "%";
            progressImage.fillAmount = www.downloadProgress;
            yield return null;
        }

        progressImage.fillAmount = 1.0f;

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            loader.SetActive(false);
            Debug.Log("Download Success!");

        }
        www.Dispose();  
    }
}

[System.Serializable]
public class ImageData2
{
    public int id;
    public string title;
    public string image_path;
    public string image_4k_path;
    public string created_at;
}
