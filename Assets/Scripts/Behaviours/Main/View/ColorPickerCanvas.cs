using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Main.View
{
	public class ColorPickerCanvas : SceneElement
	{
		public Color currentColor;

		public Image colorImage;

		public TMP_InputField colorInput;

		private void Start()
		{
			UpdateColor(currentColor);
		}

		public void UpdateColor(Color color)
		{
			currentColor = color;
			if (colorImage != null)
			{
				colorImage.color = color;
			}
			if (colorInput != null)
			{
				colorInput.text = ColorToHex(color);
			}
		}

		public void ClickButtonCancel()
		{
			base.scene.controller.color.CancelColorPicker();
		}

		public void ClickButtonOk()
		{
			base.scene.controller.color.ConfirmColorPicker();
		}

		private string ColorToHex(Color32 color)
		{
			return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}
	}
}
