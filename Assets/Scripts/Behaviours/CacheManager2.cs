using System.Collections.Generic;
using UnityEngine;

public class CacheManager2 : MonoBehaviour
{
	public static CacheManager2 instance;

	private Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>();

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
	}

	public Sprite Get(string id)
	{
		if (!cache.ContainsKey(id))
		{
			return null;
		}
		return cache[id];
	}

	public void Set(string id, Sprite sprite)
	{
		cache[id] = sprite;
	}
}
