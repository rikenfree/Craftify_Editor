using Main.View;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CapeController : MonoBehaviour
{
    public static CapeController Instance;

    public List<Texture2D> capeTextures = new List<Texture2D>();

    public Cape1 currentcap;

    public GameObject onlineCapCatalogue;
    public int pageNum = 1;
    public int minPage, maxPage;

    public GameObject maincharacter;
    public int currentCapInt 
    {
        get
        {
            return PlayerPrefs.GetInt("currentCapInt", 0);
        }
        set
        {
            PlayerPrefs.SetInt("currentCapInt", value);
        }
    }
       
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //SetLastSelectedCapeSkin();
    }

    void SetLastSelectedCapeSkin()
    {
        //if (File.Exists("C:/Users/Game 3/AppData/LocalLow/Minecraft Mania/3D Skin Editor For MCPE/autosave.png"))
        //{
        //    File.Delete("C:/Users/Game 3/AppData/LocalLow/Minecraft Mania/3D Skin Editor For MCPE/autosave.png");
        //}
        currentcap.LoadDefaultCapeSkin();
        currentcap.TempLoadCape();
    }

    public void ChangeCapTexture(int imageIndex)
    {
        //if (File.Exists("C:/Users/Game 3/AppData/LocalLow/Minecraft Mania/3D Skin Editor For MCPE/autosave.png"))
        //{
        //    File.Delete("C:/Users/Game 3/AppData/LocalLow/Minecraft Mania/3D Skin Editor For MCPE/autosave.png");
        //}
        Debug.LogError("currentCapInt : " + imageIndex);
        currentCapInt = ((pageNum - 1) * 8 + imageIndex);
        currentcap.LoadCapeSkinFromGallery(capeTextures[currentCapInt]);
        currentcap.TempLoadCape();
        onlineCapCatalogue.SetActive(false);
    }

    public void ChangePickCapeTexture(Texture2D pickedTexture2D)
    {
        //if (File.Exists("C:/Users/Game 3/AppData/LocalLow/Minecraft Mania/3D Skin Editor For MCPE/autosave.png"))
        //{
        //    File.Delete("C:/Users/Game 3/AppData/LocalLow/Minecraft Mania/3D Skin Editor For MCPE/autosave.png");
        //}

        if (pickedTexture2D.width == 22 && pickedTexture2D.height == 17)
        {
            currentcap.LastMode = 0;
            currentcap.RefreshModels();
            currentcap.capeTexture2217 = pickedTexture2D;
            currentcap.LoadCapeSkinFromGallery(pickedTexture2D);
        }
        else if (pickedTexture2D.width == 64 && pickedTexture2D.height == 32)
        {
            currentcap.LastMode = 1;
            currentcap.RefreshModels();
            currentcap.capeTexture6432 = pickedTexture2D;

            currentcap.LoadCapeSkinFromGallery6432(pickedTexture2D);

        }
        else if (pickedTexture2D.width == 512 && pickedTexture2D.height == 256)
        {
            currentcap.LastMode = 2;

            Debug.LogError("Not supported yet");
        }
        currentcap.TempLoadCape();
    }

    public void ChangePickElytraTexture(Texture2D pickedTexture2D)
    {
        //if (File.Exists("C:/Users/Game 3/AppData/LocalLow/Minecraft Mania/3D Skin Editor For MCPE/autosave.png"))
        //{
        //    File.Delete("C:/Users/Game 3/AppData/LocalLow/Minecraft Mania/3D Skin Editor For MCPE/autosave.png");
        //}
        currentcap.LastMode = 3;
        currentcap.elytraTexture6432 = pickedTexture2D;
        currentcap.RefreshModels();
        if (pickedTexture2D.width == 64 && pickedTexture2D.height == 32)
        {
            currentcap.CurrentResolution = CapeResolution.Elytra6432;
            currentcap.LoadElytraWithTexture(pickedTexture2D);

        }
       
      //  currentcap.TempLoadCape();
    }
}