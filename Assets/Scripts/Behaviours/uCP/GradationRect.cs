using System;
using UnityEngine;

namespace uCP
{
	[AddComponentMenu("uCP/Gradation Rect")]
	public class GradationRect : GradationUI
	{
		public ColorAxis X_Axis;

		public ColorAxis Y_Axis;

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
					_colorPicker.OnChange_Color.RemoveListener(UpdateUIC);
					_colorPicker.OnChange_HSV.RemoveListener(UpdateUIH);
				}
				_colorPicker = value;
				_colorPicker.OnChange_Color.AddListener(UpdateUIC);
				_colorPicker.OnChange_HSV.AddListener(UpdateUIH);
			}
		}

		protected override void Awake()
		{
			if (Application.isPlaying)
			{
				base.Awake();
				colorPicker.OnChange_Color.AddListener(UpdateUIC);
				colorPicker.OnChange_HSV.AddListener(UpdateUIH);
			}
		}

		public virtual void UpdateUIC(Color color)
		{
			if (!useAlpha)
			{
				color.a = 1f;
			}
			Color[] array = new Color[4]
			{
				color,
				color,
				color,
				color
			};
			bool flag = false;
			switch (X_Axis)
			{
				case ColorAxis.Red:
					array[0].r = (array[1].r = 0f);
					array[2].r = (array[3].r = 1f);
					flag = true;
					break;
				case ColorAxis.Green:
					array[0].g = (array[1].g = 0f);
					array[2].g = (array[3].g = 1f);
					flag = true;
					break;
				case ColorAxis.Blue:
					array[0].b = (array[1].b = 0f);
					array[2].b = (array[3].b = 1f);
					flag = true;
					break;
			}
			switch (Y_Axis)
			{
				case ColorAxis.Red:
					array[0].r = (array[3].r = 0f);
					array[1].r = (array[2].r = 1f);
					flag = true;
					break;
				case ColorAxis.Green:
					array[0].g = (array[3].g = 0f);
					array[1].g = (array[2].g = 1f);
					flag = true;
					break;
				case ColorAxis.Blue:
					array[0].b = (array[3].b = 0f);
					array[1].b = (array[2].b = 1f);
					flag = true;
					break;
			}
			if (flag)
			{
				colors = array;
				UpdateColors();
			}
		}

		public virtual void UpdateUIH(HSV hsv)
		{
			if (!useAlpha)
			{
				hsv.a = 1f;
			}
			HSV[] array = new HSV[4]
			{
				hsv,
				hsv,
				hsv,
				hsv
			};
			bool flag = false;
			switch (X_Axis)
			{
				case ColorAxis.Hue:
					array[0].h = (array[1].h = 0f);
					array[2].h = (array[3].h = 1f);
					flag = true;
					break;
				case ColorAxis.Saturation:
					array[0].s = (array[1].s = 0f);
					array[2].s = (array[3].s = 1f);
					flag = true;
					break;
				case ColorAxis.Value:
					array[0].v = (array[1].v = 0f);
					array[2].v = (array[3].v = 1f);
					flag = true;
					break;
				case ColorAxis.Alpha:
					array[0].a = (array[1].a = 0f);
					array[2].a = (array[3].a = 1f);
					flag = true;
					break;
			}
			switch (Y_Axis)
			{
				case ColorAxis.Hue:
					array[0].h = (array[3].h = 0f);
					array[1].h = (array[2].h = 1f);
					flag = true;
					break;
				case ColorAxis.Saturation:
					array[0].s = (array[3].s = 0f);
					array[1].s = (array[2].s = 1f);
					flag = true;
					break;
				case ColorAxis.Value:
					array[0].v = (array[3].v = 0f);
					array[1].v = (array[2].v = 1f);
					flag = true;
					break;
				case ColorAxis.Alpha:
					array[0].a = (array[3].a = 0f);
					array[1].a = (array[2].a = 1f);
					flag = true;
					break;
			}
			if (flag)
			{
				colors[0] = array[0];
				colors[1] = array[1];
				colors[2] = array[2];
				colors[3] = array[3];
				UpdateColors();
			}
		}
	}
}
