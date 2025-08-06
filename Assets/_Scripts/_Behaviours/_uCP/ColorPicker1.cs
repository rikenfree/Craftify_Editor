using System;
using UnityEngine;
using UnityEngine.Events;

namespace uCP
{
	[AddComponentMenu("uCP/Color Picker")]
	public class ColorPicker1 : MonoBehaviour
	{
		[Serializable]
		public class UnityEvent_Color : UnityEvent<Color>
		{
		}

		[Serializable]
		public class UnityEvent_HSV : UnityEvent<HSV1>
		{
		}

		public UnityEvent_Color OnChange;

		[HideInInspector]
		public UnityEvent_Color OnChange_Color;

		[HideInInspector]
		public UnityEvent_HSV OnChange_HSV;

		public HSV1 hsv = HSV1.red;

		public float outputMultiplier = 1f;

		public virtual Color color
		{
			get
			{
				return hsv;
			}
			set
			{
				HSV1 hSV = hsv;
				hsv = value;
				if (value == Color.black || value == Color.white)
				{
					hsv.h = hSV.h;
				}
			}
		}

		protected virtual void Start()
		{
			UpdateUI();
		}

		public virtual void SetColorByColorCode(string code)
		{
			try
			{
				if (code.Length != 7 || code[0] != '#')
				{
					UnityEngine.Debug.LogWarning("Can't get a color code.");
				}
				string value = code.Substring(1, 2);
				string value2 = code.Substring(3, 2);
				string value3 = code.Substring(5, 2);
				int num = Convert.ToInt32(value, 16);
				int num2 = Convert.ToInt32(value2, 16);
				int num3 = Convert.ToInt32(value3, 16);
				int num4 = num / 255;
				int num5 = num2 / 255;
				int num6 = num3 / 255;
				color = new Color(num4, num5, num6);
				UpdateUI();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
		}

		public virtual void Show(Color color)
		{
			Show((HSV1)color);
		}

		public virtual void Show(HSV1 hsvColor)
		{
			hsv = hsvColor;
			UpdateUI();
			base.gameObject.SetActive(value: true);
		}

		public virtual void UpdateUI()
		{
			Color color = this.color;
			if (OnChange != null)
			{
				OnChange.Invoke(color * outputMultiplier);
			}
			if (OnChange_Color != null)
			{
				OnChange_Color.Invoke(color);
			}
			if (OnChange_HSV != null)
			{
				OnChange_HSV.Invoke(hsv);
			}
		}
	}
}
