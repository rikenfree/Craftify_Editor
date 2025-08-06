using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif
using UnityEngine;
using UnityEngine.Networking;

public class APIManager2 : MonoBehaviour
{
    [Header("Download")]
    public static APIManager2 Instance;
    public string LastJSON;
    public bool mapDataLoaded = false;

    public string DefaultMAPConfig
    {
        get
        {
            return PlayerPrefs.GetString("DefaultMAPConfig", LastJSON);
        }
        set
        {
            PlayerPrefs.SetString("DefaultMAPConfig", value);
        }
    }
    public List<MapData2> mapsData;


    public int startindex;
    public int lastindex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }
    public string ServerURL;
    public string ServerUploadURL;
    public string password;

    [Header("Upload")]

    public Root mdata;
    public List<MapData2> SendAlldata;
    public List<AddonData> UpdateAllData;

    public string imagePathUrlSuffix;
    public string zipPathUrlSuffix;

    private void Start()
    {
        StartCoroutine(IEGetMapData());
    }

    [Button]
    public void GetAllMapData()
    {
#if UNITY_EDITOR
        EditorCoroutineUtility.StartCoroutine(IEGetMapData(), this);
#endif
     //   StartCoroutine(IEGetMapData());
    }

    [Button]
    public void GetAllMapDataSize()
    {
#if UNITY_EDITOR
        EditorCoroutineUtility.StartCoroutine(GetSize(), this);
#endif
        //   StartCoroutine(IEGetMapData());
    }


   [Button]
    public void UploadAllDataToServer() {
#if UNITY_EDITOR
        EditorCoroutineUtility.StartCoroutine(IEUploadDataToDataBase(), this);
#endif
    }


    public IEnumerator IEUploadDataToDataBase()
    {
        //yield return new WaitForSeconds(2);
       // UpdateAllData.Clear();


        for (int i = 0; i < mapsData.Count; i++)
        {
            AddonData m = new AddonData(mapsData[i].mapid, mapsData[i].name, mapsData[i].mapFileUrl,mapsData[i].mapFileSize);
            //m.mapid =;
            //m.name =;
            //m.imageUrl = ;
            //m.mapFileUrl = ;
            UpdateAllData.Add(m);
        }

        string json =  JsonConvert.SerializeObject(UpdateAllData);

        Debug.Log("complete data : " + json);


        WWWForm form = new WWWForm();
        form.AddField("data", json);

        yield return new WaitForSeconds(2);
        UnityWebRequest www = UnityWebRequest.Post(ServerUploadURL, form);//APImainURL
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log("USer Craetion failed : " + www.downloadHandler.text);

        }
        else
        {
            string data = www.downloadHandler.text;
            Debug.Log("data " + data);

        }
        yield return new WaitForSeconds(2);


        //MapsManager.instance.mapsData.Count
        //for (int i = startindex; i < lastindex; i++)
        //{
        //    WWWForm form = new WWWForm();
        //    form.AddField("mapid", MapsManager.instance.mapsData[i].mapid);
        //    //form.AddField("name", MapsManager.instance.mapsData[i].name);
        //    //form.AddField("imageUrl", imagePathUrlSuffix+ MapsManager.instance.mapsData[i].mapid +".jpg");
        //    //form.AddField("mapFileUrl", zipPathUrlSuffix + MapsManager.instance.mapsData[i].mapid + ".zip");



        //    Debug.Log("mapid : " + MapsManager.instance.mapsData[i].mapid);
        //    Debug.Log("name : " + MapsManager.instance.mapsData[i].name);
        //    yield return new WaitForEndOfFrame();

           
        //}

        Debug.LogError("All Data Sucessfully added");
  
    }


    public IEnumerator IEGetMapData()
    {
        Debug.Log("its on way");
        WWWForm form = new WWWForm();
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(ServerURL, form);//APImainURL
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log("USer Craetion failed : " + www.downloadHandler.text);
        }
        else
        {
          
            string data = www.downloadHandler.text;
            Debug.Log("data " + data);
            data = Regex.Unescape(data);
            LastJSON = data;
            mapDataLoaded = true;
            if (!string.IsNullOrEmpty(data))
            {
                Debug.Log("data1 " + data);
                mdata = JsonConvert.DeserializeObject<Root>(data);
            }

            Debug.Log("All Data arrives sucessfully");
            mapsData = mdata.mapdata;
            DefaultMAPConfig = data;
            //MapsManager.instance.mapsData = mdata.mapdata;
            //MapsManager.instance.AllMapDataArrived = true;
            //MapsManager.instance.RefreshMapViews();
        }


    }


        private IEnumerator GetSize()
        {
        for (int i = 0; i < mapsData.Count; i++)
        {

             MapData2 Data = mapsData[i];
            if (string.IsNullOrWhiteSpace(Data.mapFileSize))
            {
            Debug.LogError("GetSize" + Data.mapFileUrl);

            using (UnityWebRequest uwr = UnityWebRequest.Get(Data.mapFileUrl))
            {
                uwr.method = "HEAD";
                yield return uwr.Send();

                //Debug.LogError("1  " + uwr.GetResponseHeader("Content-Length"));

                int bytes = int.Parse(uwr.GetResponseHeader("Content-Length")) / 1024;
                Data.mapFileSize = bytes.ToString();
            }
            
            }
            yield return new WaitForEndOfFrame();


        }


            

        }
}
[System.Serializable]
public class Mapdata
{
    public string id;
    public string mapid;
    public string name;
    public string imageUrl;
    public string mapFileUrl;
}
[System.Serializable]
public class Root
{
    public List<MapData2> mapdata;
}