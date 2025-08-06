using Main.View;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ElytraMain : MonoBehaviour
{
    public Texture2D skin;
    public Texture2D elytratexture;
    public GameObject[] elytra;


    private void OnEnable()
    {
        if (CapeController.Instance.currentcap.CurrentResolution == CapeResolution.Elytra6432)
        {
            for (int i = 0; i < elytra.Length; i++)
            {
                elytra[i].SetActive(true);
            }
            StartCoroutine(LoadElytra());
        }
        else {
            for (int i = 0; i < elytra.Length; i++)
            {
                elytra[i].SetActive(false);
            }
        }
    }


    public IEnumerator LoadElytra() {

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
        if (skin!=null)
        {
             elytratexture = new Texture2D(64, 32, TextureFormat.ARGB32, false);

            Cape1.OverrideTextureWithSubtexture(elytratexture, skin, new Vector2(22, 10));
            elytratexture.filterMode = FilterMode.Point;
            if (CapeController.Instance.currentcap.CurrentResolution == Main.View.CapeResolution.Elytra6432)
        {
            for (int i = 0; i < elytra.Length; i++)
            {
                elytra[i].SetActive(true);
                elytra[i].GetComponent<Renderer>().material.mainTexture = elytratexture;
            }
        }
        else
        {
            for (int i = 0; i < elytra.Length; i++)
            {
                elytra[i].SetActive(false);
                //   elytra[i].GetComponent<Renderer>().material.mainTexture = CapeController.Instance.currentcap.skin;
            }
        }
        }
    }

    [Button]
    public void SetSkinOnclick(Texture2D texture)
    {
        if (texture == null)
        {
            for (int i = 0; i < elytra.Length; i++)
            {
                elytra[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < elytra.Length; i++)
            {
                elytra[i].SetActive(true);
            }
        }

        texture.filterMode = FilterMode.Point;
        for (int i = 0; i < elytra.Length; i++)
        {
        elytra[i].GetComponent<Renderer>().material.mainTexture = texture;
        }

    }
}
