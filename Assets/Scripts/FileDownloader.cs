using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownloader : MonoBehaviour
{
    // URL of the file you want to download
    public string fileURL = "http://example.com/yourfile.zip";

    // Path to save the downloaded file
    private string filePath;

    public void StartDownload()
    {
#if UNITY_EDITOR || UNITY_IOS
        //filePath = Application.persistentDataPath + "/SkinPackerTool/";
#elif UNITY_ANDROID
        filePath = "/storage/emulated/0/";
#endif

        //filePath = Path.Combine(Application.persistentDataPath, "SkinEditorMCPE.mcpack");
        //StartCoroutine(DownloadFile(fileURL, filePath));
    }

    IEnumerator DownloadFile(string url, string path)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error downloading file: " + request.error);
        }
        else
        {
            // Save the downloaded file
            File.WriteAllBytes(path, request.downloadHandler.data);
            Debug.Log("File downloaded and saved to " + path);
            yield return new WaitForSeconds(2f);
            AndroidContentOpenerWrapper.OpenContent(path);
        }
    }
}
