using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace uCP
{
	public class MemoryColor1 : MonoBehaviour
	{
		public GameObject buttonPrefab;

		public int MaxMemory = 18;

		[NonSerialized]
		public List<GradationUI1> Memories = new List<GradationUI1>();

		[NonSerialized]
		public GradationUI1 currentMemory;

		[NonSerialized]
		public ColorPicker1 _colorPicker;

		public ColorPicker1 colorPicker
		{
			get
			{
				if (_colorPicker == null)
				{
					_colorPicker = GetComponentInParent<ColorPicker1>();
				}
				return _colorPicker;
			}
		}

		public virtual void ColorButton(GradationUI1 memory)
		{
			currentMemory = memory;
			colorPicker.color = memory.colors[1];
			colorPicker.UpdateUI();
		}

		public virtual void AddButton()
		{
			CreateNewButton(colorPicker.color);
		}

		public virtual void DeleteButton()
		{
			if (Memories.Count == 0)
			{
				return;
			}
			int num = Memories.IndexOf(currentMemory);
			if (num >= 0)
			{
				UnityEngine.Object.Destroy(Memories[num].gameObject);
				Memories.RemoveAt(num);
				if (num < Memories.Count && Memories[num] != null)
				{
					currentMemory = Memories[num];
				}
			}
			else
			{
				int index = Memories.Count - 1;
				UnityEngine.Object.Destroy(Memories[index].gameObject);
				Memories.RemoveAt(index);
			}
		}

		public virtual void CreateNewButton(Color color)
		{
			if (Memories.Count < MaxMemory)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(buttonPrefab);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.SetSiblingIndex(Memories.Count);
				GradationUI1 grad = gameObject.GetComponent<GradationUI1>();
				Memories.Add(grad);
				grad.colors[0] = new Color(color.r, color.g, color.b);
				grad.colors[1] = (grad.colors[3] = color);
				grad.colors[2] = new Color(color.r, color.g, color.b, Mathf.Pow(color.a, 2f));
				gameObject.GetComponent<Button>().onClick.AddListener(delegate
				{
					ColorButton(grad);
				});
				currentMemory = grad;
			}
		}

		public virtual void SetColors(Color[] colors)
		{
			foreach (Color color in colors)
			{
				CreateNewButton(color);
			}
		}

		public virtual Color[] GetColors()
		{
			return Memories.ConvertAll((GradationUI1 x) => x.colors[1]).ToArray();
		}

		public virtual void Clear()
		{
			while (Memories.Count > 0)
			{
				DeleteButton();
			}
		}
	}
}
