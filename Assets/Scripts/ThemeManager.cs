using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public ColorThemeClass[] colorThemes;

    public static ThemeManager Instance;

    public MeshRenderer BGMR;
    public Image[] ThemeImageColor1;
    public Text[] ThemeTextColor1;

    public Image[] ThemeImageColor2;
    public Text[] ThemeTextColor2;

    public int ThemeIndex
    {
        get
        {
            return (PlayerPrefs.GetInt("ThemeIndex", 8));
        }
        set
        {
            PlayerPrefs.SetInt("ThemeIndex", value);
        }
    }

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ChangeTheThemeWithIndex(ThemeIndex);
    }

    public void ChangeTheThemeWithIndex(int index) {
        BGMR.material = colorThemes[index].GridMaterial;
        ThemeIndex = index;
    }


    public GameObject[] iosGameObjects;
    public void RefreshUIBasedOnDevice() {

        for (int i = 0; i < iosGameObjects.Length; i++)
        {
            iosGameObjects[i].SetActive(false);
        }

#if UNITY_ANDROID

      
#elif UNITY_IOS
        
#endif
    }

}
[System.Serializable]
public class ColorThemeClass {

    public Material GridMaterial;
   // public Color UIHeaderColor1;
    //public Color UIHeaderColor2;
    
}