using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace uCP
{
	[AddComponentMenu("uCP/Get Color In Rect")]
	public class GetColorInRect1 : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
	{
		public ColorAxis1 X_Axis;

		public ColorAxis1 Y_Axis;

		public RectTransform colorPointer;

		[NonSerialized]
		public Canvas _canvas;

		[NonSerialized]
		public ColorPicker1 _colorPicker;

		[NonSerialized]
		public RectTransform rect;

		public Canvas canvas
		{
			get
			{
				if (_canvas == null)
				{
					_canvas = GetComponentInParent<Canvas>();
				}
				return _canvas;
			}
			set
			{
				_canvas = value;
			}
		}

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

		protected virtual void Start()
		{
			rect = GetComponent<RectTransform>();
			colorPicker.OnChange_HSV.AddListener(UpdateUI);
		}

		public virtual void OnPointerDown(PointerEventData e)
		{
			OnDrag(e);
		}

		public virtual void OnDrag(PointerEventData e)
		{
			Vector2 localPoint;
			if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, UnityEngine.Input.mousePosition, null, out localPoint);
			}
			else if (canvas.worldCamera != null)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, UnityEngine.Input.mousePosition, canvas.worldCamera, out localPoint);
			}
			else
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, UnityEngine.Input.mousePosition, Camera.main, out localPoint);
			}
			localPoint.x += rect.rect.width * rect.pivot.x;
			localPoint.y += rect.rect.height * rect.pivot.y;
			localPoint.x /= rect.rect.width;
			localPoint.y /= rect.rect.height;
			localPoint.x = Mathf.Clamp01(localPoint.x);
			localPoint.y = Mathf.Clamp01(localPoint.y);
			Color color = colorPicker.color;
			switch (X_Axis)
			{
				case ColorAxis1.Red:
					color.r = localPoint.x;
					colorPicker.color = color;
					break;
				case ColorAxis1.Green:
					color.g = localPoint.x;
					colorPicker.color = color;
					break;
				case ColorAxis1.Blue:
					color.b = localPoint.x;
					colorPicker.color = color;
					break;
				case ColorAxis1.Hue:
					colorPicker.hsv.h = localPoint.x;
					break;
				case ColorAxis1.Saturation:
					colorPicker.hsv.s = localPoint.x;
					break;
				case ColorAxis1.Value:
					colorPicker.hsv.v = localPoint.x;
					break;
				case ColorAxis1.Alpha:
					colorPicker.hsv.a = localPoint.x;
					break;
			}
			switch (Y_Axis)
			{
				case ColorAxis1.Red:
					color.r = localPoint.y;
					colorPicker.color = color;
					break;
				case ColorAxis1.Green:
					color.g = localPoint.y;
					colorPicker.color = color;
					break;
				case ColorAxis1.Blue:
					color.b = localPoint.y;
					colorPicker.color = color;
					break;
				case ColorAxis1.Hue:
					colorPicker.hsv.h = localPoint.y;
					break;
				case ColorAxis1.Saturation:
					colorPicker.hsv.s = localPoint.y;
					break;
				case ColorAxis1.Value:
					colorPicker.hsv.v = localPoint.y;
					break;
				case ColorAxis1.Alpha:
					colorPicker.hsv.a = localPoint.y;
					break;
			}
			colorPicker.UpdateUI();
		}

		public virtual void UpdateUI(HSV1 hsv)
		{
			if (!(colorPointer == null))
			{
				float num = 0f;
				float num2 = 0f;
				Color color = hsv;
				switch (X_Axis)
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
					default:
						num = 0.5f;
						break;
				}
				switch (Y_Axis)
				{
					case ColorAxis1.Red:
						num2 = color.r;
						break;
					case ColorAxis1.Green:
						num2 = color.g;
						break;
					case ColorAxis1.Blue:
						num2 = color.b;
						break;
					case ColorAxis1.Hue:
						num2 = hsv.h;
						break;
					case ColorAxis1.Saturation:
						num2 = hsv.s;
						break;
					case ColorAxis1.Value:
						num2 = hsv.v;
						break;
					case ColorAxis1.Alpha:
						num2 = hsv.a;
						break;
					default:
						num2 = 0.5f;
						break;
				}
				num = (num - rect.pivot.x) * rect.rect.width;
				num2 = (num2 - rect.pivot.y) * rect.rect.height;
				colorPointer.localPosition = new Vector2(num, num2);
			}
		}
	}
}
