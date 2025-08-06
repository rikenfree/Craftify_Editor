using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController1 : MonoBehaviour
{
    public int dummySkinIndex;
    public Texture2D skin;
    public Texture2D skin2;

    public GameObject MainCharacter;

    public void OnEnable()
    {
        SetSkinOnclick();
    }

    public void SetSkinOnclick()
    {
        IELoadSkinResourceNew("" + dummySkinIndex);
    }

    public void IELoadSkinResourceNew(string skinName)
    {
        Debug.Log("Load skin");
        string resourceskinname = "Skins/" + skinName;

        Debug.LogError(resourceskinname);

        skin = Resources.Load<Texture2D>(resourceskinname);
        Debug.Log("SkinName" + skin.name);
        skin2 = skin;
      
        if (skin == null)
        {
            Debug.LogError("skin is null");
        }
        skin.filterMode = FilterMode.Point;

        for (int i = 0; i < 6; i++)
        {
            MainCharacter.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
        }
    }
}
