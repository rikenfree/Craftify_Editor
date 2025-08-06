using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SuperStarSdk;
using System.Collections;

public class MapsManager2 : MonoBehaviour
{
	public static MapsManager2 instance;
	public static readonly int MAP_PER_PAGE = 12;

	public MapView2[] mapViews = new MapView2[MAP_PER_PAGE];

	//public List<MapData> mapsData;

	private int currentPageIndex;

	//public List<MapData> MapsData => mapsData;
	//public List<MapData> MapsData => mapsData;

    public GameObject tostObject;
    public Transform tostParent;

    public void ShowNoInternetToast()
    {
        ShowTost("Please check Your Connection");
    }
    public void ShowDataLoading()
    {
        ShowTost("Data is Loading...");
    }

    public void ShowTost(string data) {

        GameObject tost =  Instantiate(tostObject, tostParent);
        tost.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
        tost.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,300),0.3f);
        tost.GetComponentInChildren<Text>().text = data;
        Destroy(tost,1f);

    }

	public void PreviousPage()
	{
		if (currentPageIndex > 0)
		{
			currentPageIndex--;
			RefreshMapViews();
			GuiManager2.instance.UpdatePage(currentPageIndex + 1);
			SoundManager2.instance.PlayButtonSound();
            SuperStarAd.Instance.ShowInterstitialTimer(null);
        }
	}

	public void NextPage()
	{
		if (currentPageIndex < APIManager2.Instance.mapsData.Count / MAP_PER_PAGE - 1)
		{
			currentPageIndex++;
			RefreshMapViews();
			GuiManager2.instance.UpdatePage(currentPageIndex + 1);
			SoundManager2.instance.PlayButtonSound();
            SuperStarAd.Instance.ShowInterstitialTimer(null);
        }
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ShowTost("No internet");
        }
    }

    public void FirstPage()
	{
		currentPageIndex = 0;
		RefreshMapViews();
		GuiManager2.instance.UpdatePage(currentPageIndex + 1);
		SoundManager2.instance.PlayButtonSound();
        SuperStarAd.Instance.ShowInterstitialTimer(null);
    }

	public void RandomPage()
	{
		int max = APIManager2.Instance.mapsData.Count / MAP_PER_PAGE;
		currentPageIndex = UnityEngine.Random.Range(0, max);
		RefreshMapViews();
		GuiManager2.instance.UpdatePage("Random");
		SoundManager2.instance.PlayButtonSound();
        SuperStarAd.Instance.ShowInterstitialTimer(null);
    }

	public List<MapData2> MapsDataByTag(string tag)
	{
		List<MapData2> list = new List<MapData2>();
		foreach (MapData2 mapsDatum in APIManager2.Instance.mapsData)
		{
			if (mapsDatum.Name.Contains(tag))
			{
				list.Add(mapsDatum);
			}
		}
		return list;
	}

	public List<MapData2> MapsDataByIds(string[] ids)
	{
		List<MapData2> list = new List<MapData2>();
		foreach (string b in ids)
		{
			foreach (MapData2 mapsDatum in APIManager2.Instance.mapsData)
			{
				if (mapsDatum.Id == b)
				{
					list.Add(mapsDatum);
					break;
				}
			}
		}
		return list;
	}

	public List<MapData2> MapsDataByKeyword(string keyword)
	{
		List<MapData2> list = new List<MapData2>();
		foreach (MapData2 mapsDatum in APIManager2.Instance.mapsData)
		{
			if (mapsDatum.Name.ToLower().Contains(keyword.Trim().ToLower()))
			{
				list.Add(mapsDatum);
			}
		}
		return list;
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		//mapsData = LoadMapsData(LoadArrayMapsData());
		currentPageIndex = 0;
	}

    //public bool AllMapDataArrived = false;
    public GameObject restartButtonObject;

	private void Start()
	{

        //InternetCheckingManager.Instance._connectionTester.TestInternet(hasInternet =>
        //{
            if (!InternetCheckingManager.Instance.isinternetavailable)
            {
            }
            else
            {
               
                ShowNoInternetToast();
            }


        //});


		RefreshMapViews();
	}



    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

	//private string[] LoadArrayMapsData()
	//{
	//	TextAsset textAsset = (TextAsset)Resources.Load("Database", typeof(TextAsset));
	//	string[] separator = new string[1]
	//	{
	//		","
	//	};
	//	return textAsset.text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
	//}

	//private List<MapData> LoadMapsData(string[] arrayMapsData)
	//{
	//	List<MapData> list = new List<MapData>();
	//	string[] separator = new string[1]
	//	{
	//		" "
	//	};
	//	for (int i = 0; i < arrayMapsData.Length; i++)
	//	{
	//		string[] array = arrayMapsData[i].Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
	//		string text = array[0];
	//		string name = array[1];
	//		string imageUrl = IMAGES_PATH + text + IMAGE_EXT;
	//		string mapFileUrl = MAP_FILES_PATH + text + MAP_FILE_EXT;
	//		MapData item = new MapData(text, name, imageUrl, mapFileUrl);
	//		list.Add(item);
	//	}
	//	return list;
	//}

	Coroutine CRRefreshMapViews;

    public void RefreshMapViews()
	{
		if (CRRefreshMapViews!=null)
		{
			StopCoroutine(CRRefreshMapViews);
		}
		CRRefreshMapViews = StartCoroutine(IERefreshMapViews());


    }
	public IEnumerator IERefreshMapViews()
	{
		for (int i = 0; i < MAP_PER_PAGE; i++)
		{
			mapViews[i].SetData(APIManager2.Instance.mapsData[currentPageIndex * MAP_PER_PAGE + i]);
			yield return new WaitForSeconds(0.1f);
		}
	}
}
