using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileHandler2 : MonoBehaviour
{

    public static FileHandler2 Instance;


    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }
    public List<AddonData> myAddonData = new List<AddonData>();
    // Start is called before the first frame update
    public string FolderPath;
    public string ExportFolderPath;


    [Button]
    public void ClearList()
    {
        myAddonData.Clear();



    }


        [Button]
    public void checkFolderFiles()
    {
      //  StartCoroutine(Compressfiles());
    }

   // public IEnumerator Compressfiles() {
        //Debug.Log("checkFolderFiles");

        //DirectoryInfo dir = new DirectoryInfo(FolderPath);
        //DirectoryInfo[] info = dir.GetDirectories();

        //foreach (DirectoryInfo f in info)
        //{
        //    print("Found: " + f.FullName);
        //    AddonData a = new AddonData();
        //    a.id = f.Name;

        //    DirectoryInfo dir1 = new DirectoryInfo(FolderPath + "/" + f.Name);

        //    DirectoryInfo[] info1 = dir1.GetDirectories();
        //    a.name = info1[0].Name;

        //    int[] dirProgress = new int[1];
        //    Debug.LogError("FullName" + dir1.FullName + "/" + info1[0].Name);
           
        //    lzip.compressDir(dir1.FullName + "/" + info1[0].Name, 9,ExportFolderPath + "/" + f.Name + ".mcworld", false, dirProgress);

        //    myAddonData.Add(a);

        //    yield return new WaitForSeconds(2);
        //}
    //}

}
