using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperStarSdk;
public class SkinManager2 : MonoBehaviour
{
    public static SkinManager2 Instance;


    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }

    public Texture2D currentskin;


    public void SelectSkin(Texture2D tex,Texture2D uiskin) {
        currentskin = tex;
        GuiManager2.instance.ShowSkinDownloadPanel(uiskin);
    }

    public void DownloadCurrentSkin()
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((_) =>
            {
                SaveSkin();
            }, 3);
        }
        else
        {
            SaveSkin();
        }
    }

  
    private void SaveSkin()
    {
        NativeGallery.SaveImageToGallery(currentskin, "3DSkins", "Skin", null);
        ToastManager.Instance.ShowToast("Skin Saved Successfully!.");
    }


}
