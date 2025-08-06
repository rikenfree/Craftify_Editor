using System;
using UnityEngine;
using UnityEngine.UI;

namespace uCP
{
	[AddComponentMenu("uCP/Color Input Field")]
	[RequireComponent(typeof(InputField))]
	public class ColorInputField1 : MonoBehaviour
	{
		public ColorAxis1 Property;

		[NonSerialized]
		public InputField inputField;

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
			set
			{
				if (_colorPicker != null)
				{
					_colorPicker.OnChange_HSV.RemoveListener(UpdateUI);
				}
				_colorPicker = value;
				_colorPicker.OnChange_HSV.AddListener(UpdateUI);
			}
		}

		protected virtual void Awake()
		{
			inputField = GetComponent<InputField>();
			inputField.onEndEdit.AddListener(endEdit);
			colorPicker.OnChange_HSV.AddListener(UpdateUI);
		}

		protected virtual void endEdit(string s)
		{
			if (!string.IsNullOrEmpty(s))
			{
				float num = Convert.ToSingle(s);
				num = ((Property != ColorAxis1.Hue) ? (num / 255f) : (num / 359f));
				num = Mathf.Clamp01(num);
				Color color = colorPicker.color;
				switch (Property)
				{
					case ColorAxis1.Red:
						color.r = num;
						break;
					case ColorAxis1.Green:
						color.g = num;
						break;
					case ColorAxis1.Blue:
						color.b = num;
						break;
				}
				if (color != colorPicker.color)
				{
					colorPicker.color = color;
				}
				switch (Property)
				{
					case ColorAxis1.Hue:
						colorPicker.hsv.h = num;
						break;
					case ColorAxis1.Saturation:
						colorPicker.hsv.s = num;
						break;
					case ColorAxis1.Value:
						colorPicker.hsv.v = num;
						break;
					case ColorAxis1.Alpha:
						colorPicker.hsv.a = num;
						break;
				}
				colorPicker.UpdateUI();
			}
		}

		protected virtual void UpdateUI(HSV1 hsv)
		{
			switch (Property)
			{
				case ColorAxis1.Red:
					inputField.text = (((Color)hsv).r * 255f).ToString("F0");
					break;
				case ColorAxis1.Green:
					inputField.text = (((Color)hsv).g * 255f).ToString("F0");
					break;
				case ColorAxis1.Blue:
					inputField.text = (((Color)hsv).b * 255f).ToString("F0");
					break;
				case ColorAxis1.Hue:
					inputField.text = (hsv.h * 359f).ToString("F0");
					break;
				case ColorAxis1.Saturation:
					inputField.text = (hsv.s * 255f).ToString("F0");
					break;
				case ColorAxis1.Value:
					inputField.text = (hsv.v * 255f).ToString("F0");
					break;
				case ColorAxis1.Alpha:
					inputField.text = (hsv.a * 255f).ToString("F0");
					break;
			}
		}
	}
}
