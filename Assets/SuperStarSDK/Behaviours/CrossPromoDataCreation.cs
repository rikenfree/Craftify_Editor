using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CrossPromoDataCreation : MonoBehaviour
{
    
    public CrossPromoGameData[] gameData;

    public List<SSCrossPromoAsset> data= new List<SSCrossPromoAsset>();

    [Button]
    public void CreateAndroidJSONData() 
    {
        string pattern = @"[\s\n\r]+";

        // Use Regex.Replace to remove spaces and newlines
       

        data.Clear();
        for (int i = 0; i < gameData.Length; i++)
        {
            if (gameData[i].android)
            {
                SSCrossPromoAsset ss = new SSCrossPromoAsset();
                ss.appname = gameData[i].gameName;
                ss.Aappstoreid = gameData[i].aappstoreid;
                ss.Iappstoreid = gameData[i].iappstoreid;

                //app icon data
                URLClass uRLClass = new URLClass();
                string gamenamenospace = Regex.Replace(gameData[i].gameName, pattern, "");
                uRLClass.url = gameData[i].appIconURL;
                uRLClass.name = gamenamenospace+"icon";
                uRLClass.isDownloaded = false;
                ss.appiconurl = uRLClass;

                //app video data
                ss.appvideourl = new List<URLClass>();
                for (int k = 0; k < gameData[i].appVideoURL.Length; k++)
                {
                    URLClass uRLClass1 = new URLClass();
                    string gamenamenospace1 = Regex.Replace(gameData[i].gameName, pattern, "");
                    uRLClass1.url = gameData[i].appVideoURL[k];
                    uRLClass1.name = gamenamenospace1 + k;
                    uRLClass1.isDownloaded = false;
                    ss.appvideourl.Add(uRLClass1);
                }

                data.Add(ss);
            }
        }

    }

    [Button]
    public void CreateIosJSONData()
    {

        string pattern = @"[\s\n\r]+";

        // Use Regex.Replace to remove spaces and newlines


        data.Clear();
        for (int i = 0; i < gameData.Length; i++)
        {
            if (gameData[i].android)
            {
                SSCrossPromoAsset ss = new SSCrossPromoAsset();
                ss.appname = gameData[i].gameName;
                ss.Aappstoreid = gameData[i].aappstoreid;
                ss.Iappstoreid = gameData[i].iappstoreid;

                //app icon data
                URLClass uRLClass = new URLClass();
                string gamenamenospace = Regex.Replace(gameData[i].gameName, pattern, "");
                uRLClass.url = gameData[i].appIconURL;
                uRLClass.name = gamenamenospace + "icon";
                uRLClass.isDownloaded = false;
                ss.appiconurl = uRLClass;

                //app video data
                ss.appvideourl = new List<URLClass>();
                for (int k = 0; k < gameData[i].appVideoURL.Length; k++)
                {
                    URLClass uRLClass1 = new URLClass();
                    string gamenamenospace1 = Regex.Replace(gameData[i].gameName, pattern, "");
                    uRLClass1.url = gameData[i].appVideoURL[k];
                    uRLClass1.name = gamenamenospace1 + k;
                    uRLClass1.isDownloaded = false;
                    ss.appvideourl.Add(uRLClass1);
                }

                data.Add(ss);
            }
        }

    }

    [Button]
    public void DebugJSON() 
    {
      //  Debug.unityLogger.logEnabled = true;
        Debug.Log(" \"data\":" + JsonConvert.SerializeObject(data) );
    
    }

}
[System.Serializable]
public class CrossPromoGameData 
{
    public string about;
    public string gameName;
    public string aappstoreid;
    public string iappstoreid;
    public string appIconURL;
    public string[] appVideoURL;
    public bool android;
    public bool iOS;

}

//public enum CrossPromoPlatfom 
//{ 
//    IOS,
//    ANDROID
//}