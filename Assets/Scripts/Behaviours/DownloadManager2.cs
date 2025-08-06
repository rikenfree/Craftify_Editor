using System;
using System.Collections;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Networking;

public class DownloadManager2 : MonoBehaviour
{
    public static DownloadManager2 instance;

    public static readonly string GAME_FOLDER = "games";

    public static readonly string MOJANG_FOLDER = "games/com.mojang";

    public static readonly string WORLD_FOLDER = "minecraftWorlds";

    public MapData2 currentMapData;

    public string currentUrl;

    private byte[] currentWorldData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }
    private void Start()
    {


    }

    Coroutine DownloadMapCoroutine;
    public void DownloadMap(MapData2 mapData)
    {

        Debug.Log("Download Stareted");
        currentMapData = mapData;
        currentUrl = mapData.mapFileUrl;
        Debug.LogError("currenturl" + currentUrl);
        GuiManager2.instance.CloseDetailsPanel();
        GuiManager2.instance.InitDownloadProgressPanel(mapData.Name);

        if (DownloadMapCoroutine != null)
        {
            StopCoroutine(DownloadMapCoroutine);
        }
        DownloadMapCoroutine = StartCoroutine(StartLoading());
    }

    public void StopDownload()
    {
        if (DownloadMapCoroutine != null)
        {
            StopCoroutine(DownloadMapCoroutine);
        }
    }

    [Button]
    public void DownloadData()
    {
        StartCoroutine(DownloadAllMaps11());
    }

    public IEnumerator DownloadAllMaps11()
    {
        //MapsManager.instance.mapsData.Count
        for (int i = 0; i < 300; i++)
        {
            DownloadMap(APIManager2.Instance.mapsData[i]);

            currentMapData = APIManager2.Instance.mapsData[i];
            currentUrl = APIManager2.Instance.mapsData[i].mapFileUrl;
            //  Debug.LogError("currenturl" + currentUrl);
            //  GuiManager.instance.CloseDetailsPanel();
            //  GuiManager.instance.InitDownloadProgressPanel(currentMapData.Name);

            WWW www = new WWW(currentUrl);

            while (!www.isDone)
            {
                GuiManager2.instance.UpdateDownloadProgress(www.progress);
                yield return null;
            }
            yield return www;
            if (www.error == null && www.url == currentUrl)
            {
                Debug.Log("successfull");
                byte[] bytes = www.bytes;
                SaveWorldData(bytes);
            }
            else
            {
                Debug.Log("error download" + i);
            }


            WWW www1 = new WWW(currentMapData.imageUrl);
            while (!www1.isDone)
            {
                //GuiManager.instance.UpdateDownloadProgress(www.progress);
                yield return null;
            }
            yield return www1;
            if (www1.error == null && www1.url == currentMapData.imageUrl)
            {
                Debug.Log("successfull");
                byte[] bytes1 = www1.bytes;
                SaveImage(currentMapData.Id, bytes1);
                // SaveWorldData(bytes);
            }
            else
            {
                Debug.Log("error download" + i);
            }


            yield return new WaitForSeconds(1);
        }

        Debug.LogError("All Sucessfull Download");
    }



    private IEnumerator StartLoading()
    {
        Debug.LogError("StartLoading 111 " + currentUrl);

        UnityWebRequest www = UnityWebRequest.Get(currentUrl);
        www.SendWebRequest();

        while (!www.isDone)
        {
            GuiManager2.instance.UpdateDownloadProgress(www.downloadProgress);
            yield return null;
        }

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Download successful");
            byte[] bytes = www.downloadHandler.data;
            SaveWorldData(bytes);
        }
        else
        {
            Debug.LogError("❌ Download failed: " + www.error);
            if (errorPopup != null)
                errorPopup.SetActive(true);
        }
    }

    public GameObject errorPopup;

    //    private void SaveWorldData(byte[] data)
    //    {
    //        Debug.LogError("SaveWorldData");
    //        currentWorldData = data;
    //        string DirPath = "";
    //#if UNITY_EDITOR
    //        DirPath =Application.persistentDataPath+"/";
    //#elif UNITY_ANDROID
    //        DirPath = "/storage/emulated/0/Download/";

    //#endif

    //        if (Directory.Exists(DirPath + "MapForMCPE"))
    //        {
    //            Debug.Log("Directory Exist");

    //        }
    //        else { 
    //            Debug.Log("Directory Not  Exist");
    //            Directory.CreateDirectory(DirPath + "MapForMCPE");
    //        }


    //        string text2 = DirPath + "MapForMCPE" + "/" + WORLD_FOLDER;
    //        Debug.LogError("SaveWorldData");

    //        SaveWorldData(text2);
    //       /* try
    //        {

    //            string text = new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory", Array.Empty<object>()).Call<string>("getPath", Array.Empty<object>());
    //            GuiManager.instance.AddDebugString("RootPath: " + text);
    //            Debug.Log("rootpath" + text);
    //            string text2 = text + "/" + WORLD_FOLDER;
    //            Debug.LogError("text2" + text2);


    //            if (!Directory.Exists(text2))
    //            {
    //                GuiManager.instance.AddDebugString("World folder not exist. Create.");
    //                Directory.CreateDirectory(text2);
    //            }
    //            else
    //            {
    //                // Directory.CreateDirectory(text2+"/riken");
    //                Debug.LogError("Folder Exist");
    //                //errorPopup.SetActive(true);
    //                GuiManager.instance.AddDebugString("World folder exist. Skip creation.");
    //            }

    //        }
    //        catch (Exception)
    //        {
    //            // Debug.LogError("error error erroe error");
    //            // errorPopup.SetActive(true);
    //#if UNITY_EDITOR
    //            GuiManager.instance.AddDebugString("RootPath: " + Application.persistentDataPath);
    //           // string text2 = Application.persistentDataPath + "/" + WORLD_FOLDER;
    //            string text2 = Application.persistentDataPath + "/" + WORLD_FOLDER;
    //            Debug.LogError("text2" + text2);


    //            if (!Directory.Exists(text2))
    //            {
    //                GuiManager.instance.AddDebugString("World folder not exist. Create.");
    //                Directory.CreateDirectory(text2);
    //            }
    //            else
    //            {
    //                GuiManager.instance.AddDebugString("World folder exist. Skip creation.");
    //            }
    //            SaveWorldData(text2);
    //#endif
    //        }*/
    //    }

    private void SaveWorldData(byte[] data)
    {
        Debug.LogError("📦 SaveWorldData");

        string folderPath = Path.Combine(Application.persistentDataPath, "minecraftWorlds");
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("📁 Folder does not exist. Creating: " + folderPath);
            Directory.CreateDirectory(folderPath);
        }
        else
        {
            Debug.Log("📁 Folder already exists: " + folderPath);
        }

        string fileName = currentMapData.Id + ".mcworld";
        string filePath = Path.Combine(folderPath, fileName);

        File.WriteAllBytes(filePath, data);

        Debug.Log("✅ File saved successfully at: " + filePath);

        // ✅ Visually complete the progress bar
        GuiManager2.instance.FinishDownloadProgress();

        // ✅ Launch Minecraft
        AndroidContentOpenerWrapper.OpenContent(filePath);

    }


    public void OnClickErrorOkButton()
    {
        errorPopup.SetActive(false);
        SoundManager2.instance.PlayButtonSound();
        currentMapData = null;
        GuiManager2.instance.CloseDetailsPanel();
        //IronSourceAdManager.Instance.ShowInterstitial();
    }


    public void SaveImage(string mapid, byte[] data)
    {


        string text = Application.persistentDataPath + "/" + WORLD_FOLDER + "/" + mapid + ".png";
        File.WriteAllBytes(text, data);
    }

    private void SaveWorldData(string worldFolder)
    {
        Debug.LogError("here" + currentWorldData);
        //  string text = worldFolder + "/" + currentMapData.Id + ".zip";

        if (Directory.Exists(worldFolder))
        {
            Debug.LogError("Directory Exist : " + worldFolder);
        }
        else
        {
            Directory.CreateDirectory(worldFolder);
        }
            string text = worldFolder + "/" + currentMapData.Id + ".mcworld";
        Debug.LogError("error => "+ text);
        File.WriteAllBytes(text, currentWorldData);


        //      Debug.Log("here error");
        //int[] progress = new int[512];
       /* string text3 = Util.RandomAlphaNumeric(6);
        string text2 = worldFolder + "/MAP_" + text3 + currentMapData.Id;
        Debug.LogError("error");

        if (Directory.Exists(text2))
        {
            Debug.LogError("Directory Exist : " + text2);
        }
        else
        {
            Directory.CreateDirectory(text2);

            if (Directory.Exists(text2))
            {
                Debug.LogError("Directory Exist : " + text2);
            }
            Debug.LogError("create Directory" + text2);

        }
        lzip.decompress_File(text, worldFolder, null, currentWorldData);*/
        Debug.LogError("error");
        // File.Delete(text);
        // string sourceDirName = Directory.GetDirectories(text2)[0];
        //  Debug.Log("sourceDirName" + sourceDirName);
        //Directory.Move(sourceDirName, worldFolder + "/MAP_" + currentMapData.Id + "_" + text3);
        //Directory.Delete(text2);
        GuiManager2.instance.CompleteDownloadProgress();

    }
}
