using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTextureOnModel : MonoBehaviour
{
    public CapeEltraView CEV;
    public GameObject[] cape;
    public Texture2D assignTexture;


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
                cape[i].SetActive(true);
            }
        texture.filterMode = FilterMode.Point;
        assignTexture = texture;
        for (int i = 0; i < cape.Length; i++)
        {
            cape[i].GetComponent<Renderer>().material.mainTexture = texture;
        }
        }


    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");

        if (assignTexture!=null)
        {

            CEV.SelectCape(assignTexture);
            CEV.OpenSelectedCapeBigView();
        }
    }
}
