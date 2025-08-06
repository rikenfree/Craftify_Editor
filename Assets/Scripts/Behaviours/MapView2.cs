using SuperStarSdk;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapView2 : MonoBehaviour
{
	public Image mapImage;

	public Text mapName;


	public Text mapFileSize;

	public Image loadingImage;

	public MapData2 mapData;

    public void SetData(MapData2 mapData)
	{
		this.mapData = mapData;
		Reload();
	}
	Coroutine GetSizeCoroutine;


	private void Reload()
	{
        mapName.text = mapData.Name;
		Sprite sprite = CacheManager2.instance.Get(mapData.Id);
		if (sprite != null)
		{
			mapImage.sprite = sprite;
			CacheManager2.instance.Set(mapData.Id, sprite);
		}
		else
		{
			loadingImage.gameObject.SetActive(value: true);
			StartCoroutine(StartLoading());
		}
		if (string.IsNullOrEmpty(mapData.mapFileSize))
		{
            if (GetSizeCoroutine !=null)
            {
				StopCoroutine(GetSizeCoroutine);
				

            }
			GetSizeCoroutine = StartCoroutine(GetSize());
		}
		else 
		{
			mapFileSize.text = mapData.mapFileSize+ "kb";
		}

	}
	


	private IEnumerator StartLoading()
	{

		WWW www = new WWW(mapData.ImageUrl);
		yield return www;
		if (www.error == null && www.url == mapData.ImageUrl)
		{
			Sprite sprite = Sprite.Create(rect: new Rect(0f, 0f, www.texture.width, www.texture.height), texture: www.texture, pivot: new Vector2(0f, 0f));
			mapImage.sprite = sprite;
			loadingImage.gameObject.SetActive(value: false);
			CacheManager2.instance.Set(mapData.Id, sprite);
		}


	}

	private IEnumerator GetSize()
	{
		
		//WWW www = new WWW(mapData.ImageUrl);

		Debug.LogError("GetSize" + mapData.mapFileUrl);


		using (UnityWebRequest uwr = UnityWebRequest.Get(mapData.mapFileUrl))
		{
			uwr.method = "HEAD";
			yield return uwr.Send();

			//Debug.LogError("1  " + uwr.GetResponseHeader("Content-Length"));

			int bytes = int.Parse(uwr.GetResponseHeader("Content-Length")) / 1024;
			mapData.mapFileSize = bytes.ToString();
			mapFileSize.text = mapData.mapFileSize + "kb";
		}


		//UnityWebRequest www = UnityWebRequest.Head(mapData.mapFileUrl);

		//yield return www;
		////yield return new WaitForEndOfFrame();
		//if (www.error == null && www.url == mapData.ImageUrl)
		//{

		//	Debug.LogError(www.GetResponseHeader("Content-Length"));
		//	mapFileSize.text = "1";
		//}
		//else {
		//	Debug.LogError(www.error);
		//}


	}

    public void Clicked()
    {
        StartCoroutine(ShowAdThenOpenDetails());
    }

    IEnumerator ShowAdThenOpenDetails()
    {
        bool adCompleted = false;
        bool callbackCalled = false;

        float timeout = 5f;
        float elapsed = 0f;

        SuperStarAd.Instance.ShowInterstitialTimer((o) =>
        {
            adCompleted = true;
            callbackCalled = true;
        });

        while (!callbackCalled && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Now open the panel regardless of ad success/failure
        SoundManager2.instance.PlayButtonSound();
        GuiManager2.instance.ShowDetailsPanel(mapImage.sprite, mapData);
    }

}
