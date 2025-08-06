using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager2 : MonoBehaviour
{
	public static StorageManager2 instance;

	public static readonly string KEY_MAP_IDS = "KEY_MAP_IDS";

	public static readonly string KEY_PLAY_COUNT = "KEY_PLAY_COUNT";

	public static readonly string KEY_RATED = "KEY_RATED";

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
		IncreasePlayCount();
	}

	public void ResetAll()
	{
		PlayerPrefs.DeleteAll();
	}

	public int LoadPlayCount()
	{
		return PlayerPrefs.GetInt(KEY_PLAY_COUNT, 0);
	}

	public void SavePlayCount(int playCount)
	{
		PlayerPrefs.SetInt(KEY_PLAY_COUNT, playCount);
	}

	public void IncreasePlayCount()
	{
		int num = LoadPlayCount();
		SavePlayCount(num + 1);
	}

	public bool LoadRated()
	{
		return PlayerPrefs.GetInt(KEY_RATED, 0) != 0;
	}

	public void SaveRated(bool rated)
	{
		int value = rated ? 1 : 0;
		PlayerPrefs.SetInt(KEY_RATED, value);
	}

	public bool AddFavouriteMapId(string mapId)
	{
		string[] array = LoadFavouriteMapIds();
		if (array.Length == 0)
		{
			PlayerPrefs.SetString(KEY_MAP_IDS, mapId);
		}
		else if (Array.IndexOf(array, mapId) <= -1)
		{
			string value = string.Join(",", array) + "," + mapId;
			PlayerPrefs.SetString(KEY_MAP_IDS, value);
		}
		return true;
	}

	public bool RemoveFavouriteMapId(string mapId)
	{
		string[] array = LoadFavouriteMapIds();
		List<string> list = new List<string>();
		bool result = false;
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text == mapId)
			{
				result = true;
			}
			else
			{
				list.Add(text);
			}
		}
		string[] value = list.ToArray();
		PlayerPrefs.SetString(KEY_MAP_IDS, string.Join(",", value));
		return result;
	}

	public bool ResetFavouriteMaps()
	{
		PlayerPrefs.SetString(KEY_MAP_IDS, "");
		return true;
	}

	public string[] LoadFavouriteMapIds()
	{
		string @string = PlayerPrefs.GetString(KEY_MAP_IDS, "");
		if (@string == "")
		{
			return new string[0];
		}
		return @string.Split(',');
	}

	public bool HasFavouriteMapId(string mapId)
	{
		return Array.IndexOf(LoadFavouriteMapIds(), mapId) > -1;
	}
}
