using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader2 : MonoBehaviour
{
    public GameObject imagePrefab;
    public RectTransform content;
    public ScrollRect scrollRect;

  //  private int totalImages = 500;
    private int visibleImages = 10;
    private int imageHeight = 400;
    private List<GameObject> imagePool = new List<GameObject>();
    private int lastStartIndex = -1;

    private void Start()
    {

        foreach (Transform item in content)
        {
            Destroy(item.gameObject);
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, (RandomSkinManager2.Instance.MaxSkins/2) * imageHeight);

        for (int i = 0; i < visibleImages; i++)
        {
            GameObject imageObject = Instantiate(imagePrefab, content);
            imagePool.Add(imageObject);
            imageObject.SetActive(false);
        }

       
    }

    
    public int startIndex;
    public int imageIndex;
    private void LateUpdate()
    {
        startIndex = Mathf.Clamp(Mathf.FloorToInt((1-scrollRect.normalizedPosition.y) * ((RandomSkinManager2.Instance.MaxSkins / 2) - visibleImages)), 0, (RandomSkinManager2.Instance.MaxSkins / 2) - visibleImages);

        if (startIndex == lastStartIndex) return;
        lastStartIndex = startIndex;

        for (int i = 0; i < visibleImages; i++)
        {
             imageIndex = startIndex + i;
            GameObject imageObject = imagePool[i];
            imageObject.SetActive(true);
            imageObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -imageIndex * imageHeight);
            imageObject.GetComponent<ItemSkins2>().LoadSkinToPrefab(imageIndex * 2, (imageIndex * 2) + 1);
            StartCoroutine(LoadImageAsync("DefaultSkins/" + imageIndex, imageObject.GetComponent<Image>()));
        }
    }

    private IEnumerator LoadImageAsync(string path, Image image)
    {
        ResourceRequest request = Resources.LoadAsync<Sprite>(path);
        yield return request;

        if (request.asset != null)
        {
            image.sprite = request.asset as Sprite;
        }
        else {
            Debug.Log("null");
        }
    }
}
