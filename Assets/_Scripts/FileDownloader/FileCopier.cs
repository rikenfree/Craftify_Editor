using Sirenix.OdinInspector;
using System.IO;
using UnityEngine;

public class FileCopier : MonoBehaviour
{
    // Start is called before the first frame update
     public   string pathToSourceFolders = "/path/to/your/source/folders"; // Specify the path to the parent folder which contains all your 50 folders
    public string AdditionalPath;
    public string searchPattern = "*.*"; // Specify the file types to copy. Use "*.*" to copy all files.
    public string targetFolder = "/path/to/your/target/folder"; // Specify the path to the folder you want to copy files to.
    public int currentindex = 0;
    

    [Button]
    void CopyAndRename()
    {

        // Ensure the target folder exists
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        string[] directories = Directory.GetDirectories(pathToSourceFolders);
        foreach (string directory in directories)
        {
            string[] files = Directory.GetFiles(directory+ AdditionalPath, searchPattern);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetFolder, currentindex+".png");
                File.Copy(file, destFile, true);
                currentindex++;
            }
        }
    }


    [Button]
    void CopyAndRename2AllinOneFolder()
    {

        // Ensure the target folder exists
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

       
            string[] files = Directory.GetFiles(pathToSourceFolders, searchPattern);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetFolder, currentindex + ".png");
                File.Copy(file, destFile, true);
                currentindex++;
            }
        
    }
}
