using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperStarSdk;

public class ItemSkins2 : MonoBehaviour
{
    public int CurrentIndex1;
    public int CurrentIndex2;

    public RawImage Character1;
    public RawImage Character2;

    public TextMeshProUGUI NumberCharacter1;
    public TextMeshProUGUI NumberCharacter2;

    //public TextMeshProUGUI NameCharacter1;
    //public TextMeshProUGUI NameCharacter2;

   
    public GameObject ItemObject1;
    public GameObject ItemObject2;

  //  public GameObject StarObject1;
  //  public GameObject StarObject2;
    // Start is called before the first frame update

    public void LoadSkinToPrefab(int index1)
    {
        ItemObject2.SetActive(false);

      

        CurrentIndex1 = index1;
       

        

        // Your code here...
        Texture2D texture = Resources.Load<Texture2D>("DefaultSkinsSprites/" + index1);

        if (texture != null)
        {
            Character1.texture = texture;
        }
        else
        {
            Debug.LogError("Texture not found in Resources folder.");
        }
        NumberCharacter1.text = "" + index1;

    }
    public void LoadSkinToPrefab(int index1,int index2)
    {

       

        ItemObject2.SetActive(true);
        CurrentIndex1 = index1;
        CurrentIndex2 = index2;

        

      

        // Your code here...
        Texture2D texture = Resources.Load<Texture2D>("DefaultSkinsSprites/" + index1);

        if (texture != null)
        {
            Character1.texture = texture;
        }
        else
        {
            Debug.LogError("Texture not found in Resources folder.");
        }
        NumberCharacter1.text = "" + index1;



      



        Texture2D texture1 = Resources.Load<Texture2D>("DefaultSkinsSprites/" + (index2));

        if (texture != null)
        {
            Character2.texture = texture1;
        }
        else
        {
            Debug.LogError("Texture not found in Resources folder.");
        }
        NumberCharacter2.text = "" + (index2);
    }


    public void SelectIndex1()
    {
        

        if (PlayerPrefs.GetInt("Item_" + CurrentIndex1, 0) == 0)
        {
           // SceneManager.instance.waitingPanel.gameObject.SetActive(false);
            SuperStarAd.Instance.ShowInterstitialTimer((result) =>
            {

                SkinManager2.Instance.SelectSkin(Resources.Load<Texture2D>("DefaultSkins/" + (CurrentIndex1)), Resources.Load<Texture2D>("DefaultSkinsSprites/" + (CurrentIndex1)));
              //  SceneManager.instance.ShowSkin(Resources.Load<Texture2D>("DefaultSkins/" + (CurrentIndex1)), CurrentIndex1);


            });
        }
        else
        {
            SuperStarAd.Instance.ShowForceInterstitial((result) =>
            {

            //    SceneManager.instance.waitingPanel.gameObject.SetActive(false);
                if (result)
                {
                    PlayerPrefs.SetInt("Item_" + CurrentIndex1, 0);
                    SkinManager2.Instance.SelectSkin(Resources.Load<Texture2D>("DefaultSkins/" + (CurrentIndex1)), Resources.Load<Texture2D>("DefaultSkinsSprites/" + (CurrentIndex1)));
                    //SceneManager.instance.ShowSkin(Resources.Load<Texture2D>("DefaultSkins/" + (CurrentIndex1)), CurrentIndex1);
                }
                else
                {
                    ToastManager.Instance.ShowToast("AD is not available");
                }


            });
        }
    }

    public void SelectIndex2()
    {


        if (PlayerPrefs.GetInt("Item_" +(CurrentIndex2), 0) == 0)
        {

            SuperStarAd.Instance.ShowInterstitialTimer((result) =>
            {

                //  SceneManager.instance.waitingPanel.gameObject.SetActive(false);
                SkinManager2.Instance.SelectSkin(Resources.Load<Texture2D>("DefaultSkins/" + CurrentIndex2), Resources.Load<Texture2D>("DefaultSkinsSprites/" + CurrentIndex2));
                //SceneManager.instance.ShowSkin(Resources.Load<Texture2D>("DefaultSkins/" + CurrentIndex2), CurrentIndex2);
            });
        }
        else
        {
            SuperStarAd.Instance.ShowForceInterstitial((result) =>
            {
              //  SceneManager.instance.waitingPanel.gameObject.SetActive(false);
                if (result)
                {
                    PlayerPrefs.SetInt("Item_" + (CurrentIndex2), 0);
                    SkinManager2.Instance.SelectSkin(Resources.Load<Texture2D>("DefaultSkins/" + CurrentIndex2), Resources.Load<Texture2D>("DefaultSkinsSprites/" + CurrentIndex2));
                   // SceneManager.instance.ShowSkin(Resources.Load<Texture2D>("DefaultSkins/" + CurrentIndex2), CurrentIndex2);
                }
                else
                {
                    ToastManager.Instance.ShowToast("AD is not available");
                }


            });
        }
    }
}