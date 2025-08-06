using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AddonFileData2 : MonoBehaviour
{
    public List<AddonData> myAddonData = new List<AddonData>();
    // Start is called before the first frame update
    public string FolderPath;

    [Button]
    public void checkFolderFiles()
    {
        DirectoryInfo dir = new DirectoryInfo(FolderPath);
        DirectoryInfo[] info = dir.GetDirectories();

        //foreach (DirectoryInfo f in info)
        //{
        //    print("Found: " + f.Name);
        //    AddonData a = new AddonData();
        //    a.id = f.Name;

        //    DirectoryInfo dir1 = new DirectoryInfo(FolderPath+"/"+ f.Name);
        //    DirectoryInfo[] info1 = dir1.GetDirectories();
        //    a.name = info1[0].Name;



        //}
    }
}

[System.Serializable]
public class AddonData
{
    public string id;
    public string name;
    public string url;
    public string size;

    public AddonData(string id1, string name1, string Url,string Size)
    {
        this.id = id1;
        this.name = name1;
        this.url = Url;
        this.size = Size;
        
    }
}
