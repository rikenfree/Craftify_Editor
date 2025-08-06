
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using Main;
using System.Text.RegularExpressions;

public class CharacterMain : SceneElement1
{
    public delegate void Clicked(Texture2D skin, int id);
    public GameObject player64x32;
    public GameObject player64x64;
    public GameObject playerSlim;
    public Texture2D skin;
    public Texture2D skin2;
    public bool isSkinLocked;
    int characterIndex;
    public bool isSkinLoaded;
    public int currentskinIndex;
    // public Body body;

    public float defaultRotation;

    private string skinName;

    private Clicked clicked;
    public GameObject LockedObject;

    private int id;

    public Animator mainAnimator;

    private void Start()
    {
        mainAnimator = this.GetComponent<Animator>();
        SetSkinOnclick();
    }

    private void OnEnable()
    {
        if (player64x32.activeSelf)
        {
            SetAnimationClip("Anim1");
        }
        else if (playerSlim.activeSelf)
        {
            SetAnimationClip("Anim2");
        }
        else if(player64x64.activeSelf)
        {
            SetAnimationClip("Anim3");
        }
    }

    public void RegisterClickedEvent(Clicked clicked, Texture2D skin, int id)
    {
        this.clicked = clicked;
        this.skin = skin;
        this.id = id;
    }

    Coroutine LoadSkinResourceCoroutine;
    public void LoadSkinResource(string skinName, int id)
    {
        currentskinIndex = id;
        //if (SkinPackManager.Instance.skinpackdata.skins[id].skintype == "1" && PlayerPrefs.GetInt("SkinUnlockedPack" + skinName, 0)==0)
        //{
        //    isSkinLocked = true;
        //    LockedObject.SetActive(true);
        //}
        //else
        //{
        //    isSkinLocked = false;
        //    LockedObject.SetActive(false);
        //}
        //if (LoadSkinResourceCoroutine != null)
        //{
        //    StopCoroutine(LoadSkinResourceCoroutine);
        //}
        //LoadSkinResourceCoroutine = StartCoroutine(IELoadSkinResource(skinName));
        IELoadSkinResourceNew(skinName);
    }

    public void LoadSkinOnine(string name)
    {
        string url = "https://minotar.net/skin/" + name + ".png";
        StartCoroutine(Load(url, random: false));
    }

    public void LoadSkinGallery()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (!string.IsNullOrEmpty(path))
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 64, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                LoadSkinDirect(texture);
            }
        }, "Select a PNG image", "image/png");
    }


    public void SaveGallery()
    {
        NativeGallery.SaveImageToGallery(skin, "3DSkinEditor", "CharacterSkin", null);
        if (Main.Controller.RatingController1.instance.Rate == 0)
        {
            Main.Controller.RatingController1.instance.rateCanvas.gameObject.SetActive(true);
        }
        Main.Controller.RatingController1.instance.succesfullPooup.SetActive(true);
    }

    public void LoadRandomSkinOnine()
    {
        string str = base.scene.controller.randomSkin.next();
        str = Regex.Replace(str, @"\s+$", "");//for remove end space.......
        string url = "https://minotar.net/skin/" + str + ".png";
        StartCoroutine(Load(url, random: true));
    }

    private IEnumerator Load(string url, bool random)
    {
        Debug.Log("Random URL:" + url);
        WWW www = new WWW(url);
        Texture2D tmpTexture = Resources.Load<Texture2D>("Skins/internet_tmp");
        yield return www;
        if (www.error == null)
        {
            www.LoadImageIntoTexture(tmpTexture);
            // HandleTexture(tmpTexture);
            LoadSkinDirect(tmpTexture);
        }
        else
        {
            base.scene.view.errorCanvas.ShowLoadingSkinOnlineError(random);
        }
        base.scene.view.waitingCanvas.Hide();
    }

    public void LoadSkinDirect(Texture2D _skin)
    {
        _skin.filterMode = FilterMode.Point;

        skin = _skin;
        skin2 = _skin;
        if (skin.height == 64)
        {
            if (skin.GetPixel(50, 44).a == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    playerSlim.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
                }
                SetAnimationClip("Anim2");
                playerSlim.SetActive(true);
                player64x64.SetActive(false);
                player64x32.SetActive(false);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    player64x64.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
                }
                SetAnimationClip("Anim3");
                player64x64.SetActive(true);
                player64x32.SetActive(false);
                playerSlim.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                player64x32.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
            }

            SetAnimationClip("Anim1");
            player64x32.SetActive(true);
            player64x64.SetActive(false);
            playerSlim.SetActive(false);
        }
    }

    //public IEnumerator IELoadSkinResource(string skinName)
    //{
    //    Debug.Log("Skinname : " + skinName);
    //    isSkinLoaded = false;
    //    this.skinName = skinName;
    //    //UnityWebRequest www = UnityWebRequestTexture.GetTexture(SkinPackManager.Instance.SkinPackTextureUrl + skinName+".png");
    //    yield return www.SendWebRequest();

    //    //Debug.LogError(SkinPackManager.Instance.SkinPackTextureUrl + skinName+".png");
    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log("Username invalid or not found!");
    //    }
    //    else
    //    {
    //        skin = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        skin.filterMode = FilterMode.Point;
    //        if (skin.height == 64)
    //        {
    //            if (skin.GetPixel(50, 44).a == 0)
    //            {
    //                for (int i = 0; i < 6; i++)
    //                {
    //                    playerSlim.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
    //                }
    //                playerSlim.SetActive(true);
    //                player64x64.SetActive(false);
    //                player64x32.SetActive(false);
    //            }
    //            else
    //            {
    //                for (int i = 0; i < 6; i++)
    //                {
    //                    player64x64.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
    //                }
    //                player64x64.SetActive(true);
    //                player64x32.SetActive(false);
    //                playerSlim.SetActive(false);
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < 6; i++)
    //            {
    //                player64x32.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
    //            }
    //            player64x32.SetActive(true);
    //            player64x64.SetActive(false);
    //            playerSlim.SetActive(false);
    //        }
    //        isSkinLoaded = true;
    //    }
    //}

    public void IELoadSkinResourceNew(string skinName)
    {
        Debug.Log("Load skin");
        string resourceskinname = "Skins/" + skinName;

        Debug.LogError(resourceskinname);

        skin = Resources.Load<Texture2D>(resourceskinname);
        skin2 = skin;
        //  skin = GetExportableCurrentSkin(resourceskinname);

        //skin = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        //skin.SetPixels(tex.GetPixels());

        if (skin == null)
        {
            Debug.LogError("skin is null");
        }
        skin.filterMode = FilterMode.Point;

        if (skin.height == 64)
        {
            if (skin.GetPixel(50, 44).a == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    playerSlim.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
                }
                SetAnimationClip("Anim2");
                playerSlim.SetActive(true);
                player64x64.SetActive(false);
                player64x32.SetActive(false);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    player64x64.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
                }

                SetAnimationClip("Anim3");
                player64x64.SetActive(true);
                player64x32.SetActive(false);
                playerSlim.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                player64x32.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
            }

            SetAnimationClip("Anim1");
            player64x32.SetActive(true);
            player64x64.SetActive(false);
            playerSlim.SetActive(false);
        }
        isSkinLoaded = true;

    }

    public void LoadSkin(Texture2D skin)
    {
        skin.filterMode = FilterMode.Point;
        this.skin = skin;
        skinName = "";
        if (skin.height == 64)
        {
            if (skin.GetPixel(50, 44).a == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    playerSlim.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
                }
                SetAnimationClip("Anim2");
                playerSlim.SetActive(true);
                player64x64.SetActive(false);
                player64x32.SetActive(false);
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    player64x64.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
                }
                SetAnimationClip("Anim3");
                player64x64.SetActive(true);
                player64x32.SetActive(false);
                playerSlim.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                player64x32.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
            }

            SetAnimationClip("Anim1");
            player64x32.SetActive(true);
            player64x64.SetActive(false);
            playerSlim.SetActive(false);
        }
    }

    public void ResetDefaultRotation()
    {
        base.transform.eulerAngles = new Vector3(0f, defaultRotation, 0f);
    }

    public void OnMouseDown()
    {
        //if (!EventSystem.current.IsPointerOverGameObject() && !IsPointerOverUIObject() )
        //{
        //          if (clicked == null && !isSkinLoaded)
        //          {
        //              MonoBehaviour.print("Clicked is not registered");
        //          }
        //          else
        //          {
        //              if (isSkinLocked)
        //              {
        //                  SceneManager.instance.UnlockSkinIndex = currentskinIndex;
        //                  SceneManager.instance.UnlockScreen.SetActive(true);
        //              }
        //              else
        //              {
        //                  SceneManager.instance.currentskin = null;
        //              clicked(skin2, id);
        //              SceneManager.instance.currentskin = skin2;
        //              }
        //          }
        //          //if (isSkinLoaded)
        //          //{
        //          ////ClickOnThecharacter();
        //          //}
        //      }
    }

    public void SetAnimationClip(string ClipAnim)
    {
        Debug.Log("NAME ::> " + transform.name);
        mainAnimator.SetBool("Anim1", false);
        mainAnimator.SetBool("Anim2", false);
        mainAnimator.SetBool("Anim3", false);
        Debug.LogError("Animation clip:" + ClipAnim);
        mainAnimator.SetBool(ClipAnim, true);
    }

    public void ClickOnThecharacter()
    {
        //SceneManager.instance.currentskin = skin;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, list);
        return list.Count > 0;
    }

    public int dummySkinIndex;
    [Button]
    public void SetSkinOnclick()
    {
        IELoadSkinResourceNew("" + dummySkinIndex);
    }

    public void SetSkinOnclick(int dummyindex)
    {
        IELoadSkinResourceNew("" + dummyindex);
    }
}