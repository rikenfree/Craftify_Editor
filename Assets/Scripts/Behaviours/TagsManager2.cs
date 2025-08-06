using System;
using System.Collections.Generic;
using UnityEngine;

public class TagsManager2 : MonoBehaviour
{
	public static TagsManager2 instance;
    public string tagsDataFile = "";
	public List<TagData2> tagsData;

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
		tagsData = LoadTagsData(LoadArrayTagsData());
	}

	private void Start()
	{
		ShuffleTagsData();
		GuiManager2.instance.PopulateTags(tagsData);
	}

	private string[] LoadArrayTagsData()
	{
		//TextAsset textAsset = (TextAsset)Resources.Load("Tags", typeof(TextAsset));
		string[] separator = new string[1]
		{
			","
		};
		return tagsDataFile.Split(separator, StringSplitOptions.RemoveEmptyEntries);
	}

	private List<TagData2> LoadTagsData(string[] arrayTagsData)
	{
		List<TagData2> list = new List<TagData2>();
		string[] separator = new string[1]
		{
			" "
		};
		for (int i = 0; i < arrayTagsData.Length; i++)
		{
			string[] array = arrayTagsData[i].Split(separator, StringSplitOptions.RemoveEmptyEntries);
			string name = array[0];
			int count = int.Parse(array[1]);
			TagData2 item = new TagData2(name, count);
			list.Add(item);
		}
		return list;
	}

	private void ShuffleTagsData()
	{
		if (tagsData != null)
		{
			List<TagData2> list = new List<TagData2>();
			while (tagsData.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, tagsData.Count);
				TagData2 item = tagsData[index];
				tagsData.RemoveAt(index);
				list.Add(item);
			}
			tagsData = list;
		}
	}
}
