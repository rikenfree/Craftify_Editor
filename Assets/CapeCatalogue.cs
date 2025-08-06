using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CapeCatalogue : MonoBehaviour
{
    public Image[] images;
    public Sprite[] capImages;
    public int pageNum ;
    public TextMeshProUGUI pagenumText;
    void Start()
    {
        //  pageNum = 1;
        OnStartSetUp();
    }
    public void OnStartSetUp()
    {
        capIndex = 0;
        Debug.LogError("pageNum: " + CapeController.Instance.pageNum);
        pagenumText.text = CapeController.Instance.pageNum.ToString();
        SetCapSkin();
    }
    int capIndex;
    public void SetCapSkin()
    {
       // capIndex = 0;
        for (int i = 0; i < images.Length; i++)
        {
            
            images[i].sprite = capImages[capIndex];
            if (capIndex < (capImages.Length - 1))
            {
                capIndex += 1;
            }
        }
       
    }
    public void OnClickNextButton()
    {
        if (CapeController.Instance.pageNum < CapeController.Instance.maxPage)
        {
            CapeController.Instance.pageNum += 1;
            // pageNum += 1;
            pagenumText.text = CapeController.Instance.pageNum.ToString();
            capIndex = ((CapeController.Instance.pageNum - 1) * 8);
            SetCapSkin();
        }
    }
    public void OnClickPreviewButton()
    {
        if (CapeController.Instance.pageNum > CapeController.Instance.minPage)
        {
            CapeController.Instance.pageNum -= 1;
            //pageNum -= 1;
            pagenumText.text = CapeController.Instance.pageNum.ToString();
            capIndex = ((CapeController.Instance.pageNum - 1) * 8);
            SetCapSkin();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
