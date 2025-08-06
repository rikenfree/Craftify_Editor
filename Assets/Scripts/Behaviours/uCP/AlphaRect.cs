using System;
using UnityEngine;

namespace uCP
{
	[AddComponentMenu("uCP/Alpha Rect")]
	public class AlphaRect : GradationUI
	{
		public ColorAxis Property;

		public bool InverseValue;

		[NonSerialized]
		public ColorPicker _colorPicker;

		public ColorPicker colorPicker
		{
			get
			{
				if (_colorPicker == null)
				{
					_colorPicker = GetComponentInParent<ColorPicker>();
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

		public virtual void UpdateUI(HSV hsv)
		{
			Color color = hsv;
			float num = 1f;
			switch (Property)
			{
				case ColorAxis.Red:
					num = color.r;
					break;
				case ColorAxis.Green:
					num = color.g;
					break;
				case ColorAxis.Blue:
					num = color.b;
					break;
				case ColorAxis.Hue:
					num = hsv.h;
					break;
				case ColorAxis.Saturation:
					num = hsv.s;
					break;
				case ColorAxis.Value:
					num = hsv.v;
					break;
				case ColorAxis.Alpha:
					num = hsv.a;
					break;
			}
			colors[0].a = (colors[1].a = (colors[2].a = (colors[3].a = (InverseValue ? (1f - num) : num))));
			UpdateColors();
		}
	}
}
