using Sirenix.OdinInspector;
using System;
using UnityEngine;
using TMPro;
using Main.View;
using SuperStarSdk;
using Main.Controller;
using I2.Loc;

public class CapeEltraView : MonoBehaviour
{
    public MasterCanvas1 MC;
    public CapeEltraData[] CapeItem;
    public CapeEltraData CapeSelectedItem;
    public int currentPageIndex=0;
    public int MaxItem = 70;
    public int MaxPage = 0;

    public Texture2D texture2DSelected;
    public TextMeshProUGUI PageTxt;

    public GameObject[] CapeCollectionView;
    public GameObject[] CapeSelectedView;

    public GameObject succesfullPooup;
    private void Start()
    {
        if (MaxItem % 6 == 0)
        {
            MaxPage = Mathf.FloorToInt(MaxItem / 6);
        }
        else 
        { 
            MaxPage = Mathf.FloorToInt(MaxItem / 6);
        }
    }
    private void OnEnable()
    {
        LoadCapeElytraData(currentPageIndex);
    }

    [Button]
    public void LoadCapeElytraData(int capePageIndex) 
    {
        PageTxt.text = "Page " + (capePageIndex+1);
        string s = LocalizationManager.GetTranslation("Page");
        PageTxt.text = s + " " +(capePageIndex + 1);

        for (int i = 0; i < CapeItem.Length; i++)
        {
            Texture2D cape = Resources.Load<Texture2D>("cape/"+((capePageIndex*6) + i));

           // CapeItem[i].Cape.SetSkinOnclick(cape);
            CapeItem[i].Elytra.SetSkinOnclick(cape);
        }
    
    }

    public void NextPage() 
    {
            currentPageIndex++;
        if (currentPageIndex > MaxPage)
        {
            currentPageIndex = MaxPage;
        }
       
            LoadCapeElytraData(currentPageIndex);
        SoundController1.Instance.PlayClickSound();
        SuperStarAd.Instance.ShowInterstitialTimer(null);
    }

    public void PreviousPage()
    {
        currentPageIndex--;
        if (currentPageIndex < 0)
        {
            currentPageIndex=0;
        }
       
        LoadCapeElytraData(currentPageIndex);
        SoundController1.Instance.PlayClickSound();
        SuperStarAd.Instance.ShowInterstitialTimer(null);
    }

    public void OpenSelectedCapeBigView() 
    {
        for (int i = 0; i < CapeSelectedView.Length; i++)
        {
        CapeSelectedView[i].SetActive(true);
        }
        for (int i = 0; i < CapeCollectionView.Length; i++)
        {
            CapeCollectionView[i].SetActive(false);
        }
        CapeSelectedItem.Elytra.SetSkinOnclick(texture2DSelected);
        SuperStarAd.Instance.ShowInterstitialTimer(null);
    }

    public void BackSelectedCapeBigView()
    {
        for (int i = 0; i < CapeSelectedView.Length; i++)
        {
            CapeSelectedView[i].SetActive(false);
        }
        for (int i = 0; i < CapeCollectionView.Length; i++)
        {
            CapeCollectionView[i].SetActive(true);
        }
        SuperStarAd.Instance.ShowInterstitialTimer(null);
        MC.OnClickBackFromCollectionView();
    }


    public void SelectCape(Texture2D texture) {

        texture2DSelected = texture;
       
    }

    public void EditCape() 
    {
        UndoRedoController1.Instance.InitStacks();
        //  CapeController.Instance.currentcap
        if (texture2DSelected!=null)
        {

        CapeController.Instance.ChangePickCapeTexture(texture2DSelected);
        }
            MC.OnClickBackFromCollectionView();
        SuperStarAd.Instance.ShowForceInterstitial(null);

    }
    public void EditElytra() {

        UndoRedoController1.Instance.InitStacks();
        if (texture2DSelected != null)
        {
            CapeController.Instance.ChangePickElytraTexture(texture2DSelected);
        }
        MC.OnClickBackFromCollectionView();
        SuperStarAd.Instance.ShowForceInterstitial(null);
    }

    public int LastSavedIndex
    {
        get { return PlayerPrefs.GetInt("LastSavedIndex", 0); }

        set { PlayerPrefs.SetInt("LastSavedIndex", value); }
    }

    public void SaveGallery()
    {
        LastSavedIndex++;
        SuperStarAd.Instance.ShowForceInterstitial((result)=> {

            if (result)
            {
                if (texture2DSelected != null)
                {
                    NativeGallery.SaveImageToGallery(texture2DSelected, "3DCapeEditor2217", "Skin"+ LastSavedIndex, null);
                }
                else
                {
                    Debug.LogError("No Texture for save");
                }
                 succesfullPooup.SetActive(true);

            }
            else {
                ToastManager.Instance.ShowToast("Ad is not Loaded");
            }
        });

    }
}


[Serializable]
public class CapeEltraData 
{
    public LoadTextureOnModel Elytra;
    


}
