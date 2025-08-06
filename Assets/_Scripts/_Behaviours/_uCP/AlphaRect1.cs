using System;
using UnityEngine;

namespace uCP
{
	[AddComponentMenu("uCP/Alpha Rect")]
	public class AlphaRect1 : GradationUI1
	{
		public ColorAxis1 Property;

		public bool InverseValue;

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

		protected override void Awake()
		{
			if (Application.isPlaying)
			{
				base.Awake();
				colorPicker.OnChange_HSV.AddListener(UpdateUI);
			}
		}

		public virtual void UpdateUI(HSV1 hsv)
		{
			Color color = hsv;
			float num = 1f;
			switch (Property)
			{
				case ColorAxis1.Red:
					num = color.r;
					break;
				case ColorAxis1.Green:
					num = color.g;
					break;
				case ColorAxis1.Blue:
					num = color.b;
					break;
				case ColorAxis1.Hue:
					num = hsv.h;
					break;
				case ColorAxis1.Saturation:
					num = hsv.s;
					break;
				case ColorAxis1.Value:
					num = hsv.v;
					break;
				case ColorAxis1.Alpha:
					num = hsv.a;
					break;
			}
			colors[0].a = (colors[1].a = (colors[2].a = (colors[3].a = (InverseValue ? (1f - num) : num))));
			UpdateColors();
		}
	}
}
