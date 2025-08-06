using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.UI;

public class InAppPurches : MonoBehaviour
{
    private string jsonUrl = "https://drive.google.com/uc?export=download&id=1sBw75FXUbZkP_TzcYcfy_UMWymgw2Iwl";

    public string PrivacyPolicyLink;
    public string TermsAndConditionLink;
    public List<GameObject> OffImage = new List<GameObject>();
    public GameObject IAPPopup;

    void Start()
    {
        //StartCoroutine(LoadJSONAndCreatePopup());
    }

    public void ClosePopup()
    {
        IAPPopup= GameObject.Find("FullImage");
        IAPPopup.SetActive(false);
    }

    public void OpenPopup()
    {
        IAPPopup.SetActive(true); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { OpenPopup(); }

    }
    IEnumerator LoadJSONAndCreatePopup()
    {
        UnityWebRequest request = UnityWebRequest.Get(jsonUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonContent = request.downloadHandler.text;
            Debug.Log("JSON Loaded: " + jsonContent);

            UIHierarchy uiHierarchy = JsonUtility.FromJson<UIHierarchy>(jsonContent);

            if (uiHierarchy != null)
            {
                CreateUIElements(uiHierarchy);
            }
            else
            {
                Debug.LogError("Failed to parse UI JSON");
            }
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + request.error);
        }
    }
    public void TermsBtnClick()
    {
        Debug.Log("TermsButtonClicked");
        Application.OpenURL(TermsAndConditionLink);

    }
    public void PrivacyBtnClick()
    {
        Debug.Log("PrivacyButtonClicked");
        Application.OpenURL(PrivacyPolicyLink);

    }
    public void RestoreBtnClick()
    {
        Debug.Log("RestoreButtonClicked");

    }

    private int Count = 0;
    public void Btn299Click()
    {
       Debug.Log("Btn299");

        for (int i = 0; i < OffImage.Count; i++)
        {
            OffImage[i].SetActive(false);
        }

        OffImage[0].SetActive(true);

        Count = 299;
    }

    public void Btn999Click()
    {
        Debug.Log("Btn999");


        for (int i = 0; i < OffImage.Count; i++)
        {
            OffImage[i].SetActive(false);
        }

        OffImage[1].SetActive(true);

        Count = 999;
    }

    public void Btn1999Click()
    {
        Debug.Log("Btn1999");

        for (int i = 0; i < OffImage.Count; i++)
        {
            OffImage[i].SetActive(false);
        }

        OffImage[2].SetActive(true);

        Count = 1999;
    }

    public void ContinueBtnClickChack()
    {
        switch (Count)
        {
            case 299:
                Debug.Log("299");
                StoreController.Instance.Buy_Product(StoreController.Instance.Consumable_ProductIdList[0]);
                break;
            case 999:
                StoreController.Instance.Buy_Product(StoreController.Instance.Consumable_ProductIdList[1]);
                Debug.Log("999");
                break;
            case 1999:
                StoreController.Instance.Buy_Product(StoreController.Instance.NonConsumable_ProductIdList[0]);
                Debug.Log("1999");
                break;
        }
            
    }

    void CreateUIElements(UIHierarchy uiHierarchy)
    {
        foreach (var element in uiHierarchy.elements)
        {
            if (element.type == "TextMeshProUGUI")
            {

                GameObject textGO = new GameObject(element.name);

                GameObject Text = GameObject.Find(element.Parent);

                textGO.transform.SetParent(Text.transform);

                TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
                if (element.textContent== "smallpack")
                {
                    text.text = StoreController.Instance.GetPrice(StoreController.Instance.Consumable_ProductIdList[0]);
                }
                else if (element.textContent == "megapack")
                {
                    text.text = StoreController.Instance.GetPrice(StoreController.Instance.Consumable_ProductIdList[1]);
                }
                else if(element.textContent == "noads")
                {
                    text.text = StoreController.Instance.GetPrice(StoreController.Instance.NonConsumable_ProductIdList[0]);
                }
                
                //else if (element.textContent == "megapack1")
                //{
                //    text.text = StoreController.Instance.GetPrice(StoreController.Instance.Consumable_ProductIdList[1] + "/" + "WEEK");
                //}
                //else if (element.textContent == "noads1")
                //{
                //    text.text = StoreController.Instance.GetPrice(StoreController.Instance.NonConsumable_ProductIdList[0] + "/" + "WEEK");
                //}
                else
                {
                  text.text = element.textContent;
                }

                text.fontSize = element.fontSize;  // Set font size from JSON
                text.color = ColorUtility.TryParseHtmlString(element.ButtonColor, out var color) ? color : Color.white;
                if(element.textAlignment == "Center")
                {
                    text.alignment = TextAlignmentOptions.Center;
                }
                else if(element.textAlignment == "Left")
                {
                    text.alignment = TextAlignmentOptions.Left;
                }
              


                RectTransform textRect = text.GetComponent<RectTransform>();


                textRect.anchorMax = new Vector2(element.anchorMax.x, element.anchorMax.y);
                textRect.anchorMin = new Vector2(element.anchorMin.x, element.anchorMin.y);
                textRect.pivot = new Vector2(element.pivot.x, element.pivot.y);
                textRect.localScale = Vector3.one;


                textRect.anchoredPosition = new Vector2(element.position.x, element.position.y);
                textRect.sizeDelta = new Vector2(element.size.x, element.size.y);

            }
            if (element.type == "Gameobject")
            {

                GameObject Gameobj = new GameObject(element.name);

                GameObject Gameo = GameObject.Find(element.Parent);

                Gameobj.transform.SetParent(Gameo.transform);

             
               RectTransform GameObjRect = Gameobj.AddComponent<RectTransform>();


                GameObjRect.anchorMax = new Vector2(element.anchorMax.x, element.anchorMax.y);
                GameObjRect.anchorMin = new Vector2(element.anchorMin.x, element.anchorMin.y);
                GameObjRect.pivot = new Vector2(element.pivot.x, element.pivot.y);
                GameObjRect.localScale = Vector3.one;


                GameObjRect.anchoredPosition = new Vector2(element.position.x, element.position.y);
                GameObjRect.sizeDelta = new Vector2(element.size.x, element.size.y);

            }
            if (element.type == "Image")
            {
                GameObject imageGO = new GameObject(element.name);

                GameObject Image = GameObject.Find(element.Parent);

                imageGO.transform.SetParent(Image.transform);

                UnityEngine.UI.Image image = imageGO.AddComponent<UnityEngine.UI.Image>();

                RectTransform imageRect = imageGO.GetComponent<RectTransform>();
                imageRect.anchorMax = new Vector2(element.anchorMax.x, element.anchorMax.y);
                imageRect.anchorMin = new Vector2(element.anchorMin.x, element.anchorMin.y);
                imageRect.pivot = new Vector2(element.pivot.x, element.pivot.y);

                imageRect.anchoredPosition = new Vector2(element.position.x, element.position.y);
                imageRect.sizeDelta = new Vector2(element.size.x, element.size.y);

                imageRect.localScale = Vector3.one;

                if (!string.IsNullOrEmpty(element.imageSource))
                {
                    Sprite loadedSprite = Resources.Load<Sprite>(element.imageSource);

                    if (loadedSprite != null)
                    {
                        Debug.Log("Enter");
                        image.sprite = loadedSprite;
                    }
                    else
                    {
                        Debug.Log("Enter2");
                        Debug.LogError("Failed to load image for button from Resources: " + element.imageSource);
                    }
                }

                if (!string.IsNullOrEmpty(element.ButtonColor))
                {
                    image.color = ColorUtility.TryParseHtmlString(element.ButtonColor, out var imgColor) ? imgColor : Color.white;
                }
            }
            if (element.type == "ProceduralImage")
            {

                GameObject ProceduralImageGo = new GameObject(element.name);

                GameObject BtnParent = GameObject.Find(element.Parent);
                Debug.Log(BtnParent.name);
                ProceduralImageGo.transform.SetParent(BtnParent.transform);

                ProceduralImage Proceduralimage = ProceduralImageGo.AddComponent<ProceduralImage>();
                FreeModifier Free = ProceduralImageGo.AddComponent<FreeModifier>();

                Free.Radius = new Vector4(element.Modifier.x, element.Modifier.y, element.Modifier.z, element.Modifier.w);

                Proceduralimage.SetVerticesDirty();

                RectTransform imageRect = ProceduralImageGo.GetComponent<RectTransform>();

                imageRect.anchorMax = new Vector2(element.anchorMax.x, element.anchorMax.y);
                imageRect.anchorMin = new Vector2(element.anchorMin.x, element.anchorMin.y);
                imageRect.pivot = new Vector2(element.pivot.x, element.pivot.y);

                imageRect.anchoredPosition = new Vector2(element.position.x, element.position.y);
                imageRect.sizeDelta = new Vector2(element.size.x, element.size.y);
                imageRect.localScale = Vector3.one;
                Proceduralimage.color = ColorUtility.TryParseHtmlString(element.ButtonColor, out var imgColor) ? imgColor : Color.white;

                if(element.textContent == "1" || element.textContent == "2" || element.textContent == "3")
                {
                    
                    OffImage.Add(ProceduralImageGo);
                        ProceduralImageGo.SetActive(false);
                }

                if (element.BtnText == "true")
                {
                    if (!string.IsNullOrEmpty(element.imageSource))
                    {
                        Sprite loadedSprite = Resources.Load<Sprite>(element.imageSource);
                        if (loadedSprite != null)
                        {
                            Proceduralimage.sprite = loadedSprite;
                        }
                        else
                        {
                            Debug.LogError("Failed to load image for button from Resources: " + element.imageSource);
                        }
                    }
                }


            }
            if (element.type == "Button")
            {
                GameObject buttonGO = new GameObject(element.name);


                GameObject BtnParent = GameObject.Find(element.Parent);

                Debug.Log(BtnParent.name);

                buttonGO.transform.SetParent(BtnParent.transform);

                UnityEngine.UI.Button button = buttonGO.AddComponent<UnityEngine.UI.Button>();
                ProceduralImage Pimage = buttonGO.AddComponent<ProceduralImage>();
                FreeModifier Free = buttonGO.AddComponent<FreeModifier>();

                Free.Radius = new Vector4(element.Modifier.x, element.Modifier.y, element.Modifier.z, element.Modifier.w);

                Pimage.SetVerticesDirty();

                Pimage.color = ColorUtility.TryParseHtmlString(element.ButtonColor, out var ImageColor) ? ImageColor : Color.white;
                RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();

                buttonRect.anchorMax = new Vector2(element.anchorMax.x, element.anchorMax.y);
                buttonRect.anchorMin = new Vector2(element.anchorMin.x, element.anchorMin.y);
                buttonRect.pivot = new Vector2(element.pivot.x, element.pivot.y);

                buttonRect.anchoredPosition = new Vector2(element.position.x, element.position.y);
                buttonRect.sizeDelta = new Vector2(element.size.x, element.size.y);
                buttonRect.localScale = Vector3.one;

                if(element.name == "TermsBtn" || element.name == "PrivacyBtn" || element.name == "RestoreBtn")
                {
                    buttonGO.GetComponent<ProceduralImage>().enabled = false;

                }


                switch (element.Nametype)
                {

                    case "CloseBtn":
                        Debug.Log("PrivacyBtn");
                        button.onClick.AddListener(() => ClosePopup());
                        break;
                    case "299Btn":
                        Debug.Log("299Btn");
                        button.onClick.AddListener(() => Btn299Click());
                        break;
                    case "999Btn":
                        Debug.Log("999Btn");
                        button.onClick.AddListener(() => Btn999Click());

                        break;
                    case "1999Btn":
                        Debug.Log("999Btn");
                        button.onClick.AddListener(() => Btn1999Click());
                        break;
                    case "TermsBtn":
                        Debug.Log("TermsBtn");
                        button.onClick.AddListener(() => TermsBtnClick());
                        break;
                    case "PrivacyBtn":
                        Debug.Log("PrivacyBtn");
                        button.onClick.AddListener(() => PrivacyBtnClick());

                        break;
                    case "RestoreBtn":
                        Debug.Log("RestoreBtn");
                        button.onClick.AddListener(() => RestoreBtnClick());
                        break;
                    case "ContinueBtn":
                        Debug.Log("ContinueBtn");
                        button.onClick.AddListener(() => ContinueBtnClickChack());
                        break;

                }

                if (!string.IsNullOrEmpty(element.imageSource))
                {
                    Sprite loadedSprite = Resources.Load<Sprite>(element.imageSource);
                    if (loadedSprite != null)
                    {
                        Pimage.sprite = loadedSprite;
                    }
                    else
                    {
                        Debug.LogError("Failed to load image for button from Resources: " + element.imageSource);
                    }
                }


                if (element.Prespective == "true")
                {
                    Pimage.preserveAspect = true;
                }
                else
                {
                    Pimage.preserveAspect = false;
                }

                if (element.BtnText == "true")
                {

                    Debug.Log("BtnTextTrue");

                    GameObject buttonTextGO = new GameObject("ButtonText");
                    buttonTextGO.transform.SetParent(buttonGO.transform);
                    TextMeshProUGUI buttonText = buttonTextGO.AddComponent<TextMeshProUGUI>();
                    buttonText.text = element.textContent;
                    buttonText.fontSize = element.fontSize;
                    buttonText.color = ColorUtility.TryParseHtmlString(element.fontColor, out var FontColor) ? FontColor : Color.white;
                    buttonText.alignment = TextAlignmentOptions.Center;

                    RectTransform buttonTextRect = buttonText.GetComponent<RectTransform>();
                    buttonTextRect.anchoredPosition = Vector2.zero;
                    buttonTextRect.sizeDelta = buttonRect.sizeDelta;
                    buttonTextRect.localScale = Vector3.one;
                }



            }
            if (element.type == "Scroll")
            {
                GameObject scrollView = new GameObject(element.name);

                GameObject ScrollParent = GameObject.Find(element.Parent);

                Debug.Log(ScrollParent.name);

                scrollView.transform.SetParent(ScrollParent.transform);

                scrollView.AddComponent<UnityEngine.UI.Image>();
                ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();

                RectTransform scrollRectTransform = scrollView.GetComponent<RectTransform>();

                scrollRect.horizontal = false;

                scrollView.GetComponent<UnityEngine.UI.Image>().enabled = false;

                scrollRectTransform.offsetMin = new Vector2(element.position.x, element.position.y);
                scrollRectTransform.offsetMax = new Vector2(element.size.x, element.size.y);

                scrollRectTransform.anchorMax = new Vector2(element.anchorMax.x, element.anchorMax.y);
                scrollRectTransform.anchorMin = new Vector2(element.anchorMin.x, element.anchorMin.y);
                scrollRectTransform.pivot = new Vector2(element.pivot.x, element.pivot.y);
                scrollRectTransform.localScale = Vector3.one;
                // Create Viewport
                GameObject viewport = new GameObject("Viewport");
                RectTransform viewportRect = viewport.AddComponent<RectTransform>();



                viewportRect.SetParent(scrollView.transform);
                viewport.AddComponent<UnityEngine.UI.Image>();
                viewport.AddComponent<Mask>().showMaskGraphic = false;

                viewportRect.anchorMax = new Vector2(element.anchorMax.x, element.anchorMax.y);
                viewportRect.anchorMin = new Vector2(element.anchorMin.x, element.anchorMin.y);
                viewportRect.pivot = new Vector2(element.pivot.x, element.pivot.y);
                viewportRect.localScale = Vector3.one;
                viewportRect.anchoredPosition = Vector2.zero;
                viewportRect.sizeDelta = Vector2.zero;

                scrollRect.viewport = viewportRect;

                // Create Content
                GameObject content = new GameObject("Content");
                RectTransform contentRect = content.AddComponent<RectTransform>();
                contentRect.SetParent(viewport.transform);

                contentRect.anchorMax = new Vector2(1, 1);
                contentRect.anchorMin = new Vector2(0, 1);
                contentRect.pivot = new Vector2(0, 1);
                contentRect.localScale = Vector3.one;
                ContentSizeFitter ContenteSize = content.AddComponent<ContentSizeFitter>();

                contentRect.anchoredPosition = Vector2.zero;
                contentRect.sizeDelta = new Vector2(0, 300);
                ContenteSize.verticalFit = ContentSizeFitter.FitMode.PreferredSize;


                VerticalLayoutGroup verticalLayout = content.AddComponent<VerticalLayoutGroup>();
                verticalLayout.childControlHeight = false;
                verticalLayout.childControlWidth = false;
                verticalLayout.childAlignment = TextAnchor.MiddleCenter;

                verticalLayout.padding.top = 50;
                verticalLayout.spacing = 50;
                scrollRect.content = contentRect;

            }

        }
        ClosePopup();
    }


    //public void BtnCreatFunction(UIElement element, RectTransform Perent)
    //{
    //    GameObject buttonGO = new GameObject(element.name);
    //    buttonGO.transform.SetParent(Perent);

    //    UnityEngine.UI.Button button = buttonGO.AddComponent<UnityEngine.UI.Button>();
    //    UnityEngine.UI.Image image = buttonGO.AddComponent<UnityEngine.UI.Image>();
    //    image.color = ColorUtility.TryParseHtmlString(element.ButtonColor, out var ImageColor) ? ImageColor : Color.white;
    //    RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
    //    buttonRect.anchoredPosition = new Vector2(element.position.x, element.position.y);
    //    buttonRect.sizeDelta = new Vector2(element.size.x, element.size.y);

    //    buttonRect.anchorMax = new Vector2(element.anchorMax.x, element.anchorMax.y);
    //    buttonRect.anchorMin = new Vector2(element.anchorMin.x, element.anchorMin.y);
    //    buttonRect.pivot = new Vector2(element.pivot.x, element.pivot.y);


    //    if (element.BtnText == "true")
    //    {

    //        Debug.Log("BtnTextTrue");

    //        GameObject buttonTextGO = new GameObject("ButtonText");
    //        buttonTextGO.transform.SetParent(buttonGO.transform);
    //        TextMeshProUGUI buttonText = buttonTextGO.AddComponent<TextMeshProUGUI>();
    //        buttonText.text = element.textContent;
    //        buttonText.fontSize = element.fontSize;
    //        buttonText.color = ColorUtility.TryParseHtmlString(element.ButtonColor, out var btnColor) ? btnColor : Color.white;
    //        buttonText.alignment = TextAlignmentOptions.Center;

    //        RectTransform buttonTextRect = buttonText.GetComponent<RectTransform>();

    //        buttonTextRect.anchoredPosition = Vector2.zero;
    //        buttonTextRect.sizeDelta = buttonRect.sizeDelta;
    //    }

    //}

    //public void ImageCreatFunction(UIElement element, RectTransform Perent)
    //{
    //    GameObject imageGO = new GameObject(element.name);
    //    imageGO.transform.SetParent(Perent);
    //    UnityEngine.UI.Image image = imageGO.AddComponent<UnityEngine.UI.Image>();

    //    RectTransform imageRect = imageGO.GetComponent<RectTransform>();
    //    imageRect.anchorMax = new Vector2(1, 1);
    //    imageRect.anchorMin = new Vector2(0, 0);
    //    imageRect.pivot = new Vector2(0.5f, 0.5f);

    //    imageRect.anchoredPosition = new Vector2(element.position.x, element.position.y);
    //    imageRect.sizeDelta = new Vector2(element.size.x, element.size.y);



    //    if (!string.IsNullOrEmpty(element.ButtonColor))
    //    {
    //        image.color = ColorUtility.TryParseHtmlString(element.ButtonColor, out var imgColor) ? imgColor : Color.white;
    //    }
    //}
}

[System.Serializable]
public class UIHierarchy
{
    public string parentName;
    public UIElement[] elements;
}

[System.Serializable]
public class UIElement
{
    public string name;
    public string type;
    public Vector2 position;
    public Vector2 size;
    public Vector2 anchorMax;
    public Vector2 anchorMin;
    public Vector2 pivot;
    public Vector4 Modifier;
    public string textAlignment;
    public string Prespective;
    public string Parent;
    public string BtnText;
    public string textContent;
    public string imageSource;
    public float fontSize;
    public string ButtonColor;
    public string fontColor;
    public string Nametype;
    public string PrivacypolicyUrl;
}

