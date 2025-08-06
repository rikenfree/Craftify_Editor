using Main.View;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CapeMain2217 : MonoBehaviour
{
    public Texture2D skin;
    public Texture2D capetexture;
    public GameObject[] cape;

    private void OnEnable()
    {
        if (CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C2217 || CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C6432)
        {
            for (int i = 0; i < cape.Length; i++)
            {
                cape[i].SetActive(true);
            }
            StartCoroutine(LoadElytra());
        }
        else
        {
            for (int i = 0; i < cape.Length; i++)
            {
                cape[i].SetActive(false);
            }
        }
        //StartCoroutine(LoadElytra());
    }

    public IEnumerator LoadElytra()
    {

        string path = Path.Combine(Application.persistentDataPath, "autosave.png");


        if (File.Exists(path))
        {
            Debug.Log("File gload  Exist");
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + path))
            {
                // Send the request and yield until it's done.
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to load texture: " + www.error);
                }
                else
                {
                    // Get the loaded texture.
                    skin = DownloadHandlerTexture.GetContent(www);


                }
            }
        }
        else
        {

            Debug.LogError("File Not Exist");
        }
        if (skin != null)
        {
            capetexture = new Texture2D(64, 32, TextureFormat.ARGB32, false);

            Cape1.OverrideTextureWithSubtexture(capetexture, skin, new Vector2(0, 15));
            capetexture.filterMode = FilterMode.Point;
            
                for (int i = 0; i < cape.Length; i++)
                {
                    cape[i].SetActive(true);
                    cape[i].GetComponent<Renderer>().material.mainTexture = capetexture;
                }
           
        }
    }

    [Button]
    public void SetSkinOnclick(Texture2D texture)
    {
        if (texture == null)
        {
            for (int i = 0; i < cape.Length; i++)
            {
                cape[i].SetActive(false);
            }
        }
        else 
        {
            for (int i = 0; i < cape.Length; i++)
            {
                if (cape[i] == null)
                {
                    Debug.LogError($"Cape object at index {i} is null!");
                    continue;
                }
                var renderer = cape[i].GetComponent<Renderer>();
                if (renderer == null)
                {
                    Debug.LogError($"Renderer not found on cape object at index {i}!");
                    continue;
                }
                if (texture == null)
                {
                    Debug.LogError("Texture is null!");
                    continue;
                }
                renderer.material.mainTexture = texture;
                Debug.Log($"Assigned texture to cape[{i}]");
            }
        }
    }
}
