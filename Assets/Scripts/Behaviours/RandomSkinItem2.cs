
using SuperStarSdk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSkinItem2 : MonoBehaviour
{
    public int CurrentIndex;


  // public bool hasBeenSeen = false;
    public RawImage Character;

    //void OnBecameVisible()
    //{
    //    Debug.Log("OnBecameVisible");
    //    if (!hasBeenSeen)
    //    {
    //        // Call the method that you want to execute only once
    //        // the object becomes visible in the scroll view.
    //        LoadSkinToPrefab();

    //        // Set the boolean variable to true so that the method is
    //        // not called again when the object becomes visible again.
    //        hasBeenSeen = true;
    //    }
    //}.

    private void Start()
    {
        LoadSkinToPrefab();
    }


    public void GiveMeNewUniqueIndex() 
    {
        CurrentIndex = RandomSkinManager2.Instance.GiveMeuniqueIndex();
        LoadSkinToPrefab();

    }

    void LoadSkinToPrefab()
    {
        // Your code here...
        Texture2D texture = Resources.Load<Texture2D>("DefaultSkinsSprites/"+ CurrentIndex);

        if (texture != null)
        {
            Character.texture = texture;
        }
        else
        {
            Debug.LogError("Texture not found in Resources folder.");
        }
    }

    public void SelectIndex1()
    {
            SuperStarAd.Instance.ShowInterstitialTimer((result) =>
            {
                if (result)
                {
                    PlayerPrefs.SetInt("Item_" + CurrentIndex, 0);
                //    SceneManager.instance.currentskin = Resources.Load<Texture2D>("DefaultSkins/" + (CurrentIndex));
                //    SceneManager.instance.ShowSkin(Resources.Load<Texture2D>("DefaultSkins/" + (CurrentIndex)), CurrentIndex);
                    GiveMeNewUniqueIndex();
                }
                else
                {
                    ToastManager.Instance.ShowToast("AD is not available");
                }


            });
        
    }
}
