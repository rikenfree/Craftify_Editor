using System.Collections.Generic;
using UnityEngine;

namespace Main.Controller
{
	public class RandomSkinController1 : SceneElement1
	{
		private string[] skinNames;

		private int index;

		private void Start()
		{
			TextAsset textAsset = Resources.Load("skins_list") as TextAsset;
			skinNames = textAsset.text.Split('\n');
			skinNames = Randomize(skinNames);
			index = 0;
		}

		public string next()
		{
			string result = skinNames[index];
			index = (index + 1) % skinNames.Length;
			return result;
		}

		public string[] Randomize(string[] names)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>(names);
			while (list2.Count > 0)
			{
				int num = Random.Range(0, list2.Count);
				list.Add(list2[num]);
				list2.RemoveAt(num);
			}
			return list.ToArray();
		}
	}
}
