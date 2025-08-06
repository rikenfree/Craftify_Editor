using uCP;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Main.View
{
	public class ColorPickerNewCanvas : SceneElement
	{
		public ColorPicker colorPicker;

		public Color currentColor;

		public Image colorImage;

		public TMP_InputField colorInput;

		public void SyncOutsideColor(Color c)
		{
			colorImage.color = c;
			colorInput.text = "#" + ColorToHex(c);
			currentColor = c;
			colorPicker.color = c;
			colorPicker.UpdateUI();
		}

		public void SetColor(Color c)
		{
			colorImage.color = c;
			colorInput.text = "#" + ColorToHex(c);
			currentColor = c;
		}

		private string ColorToHex(Color32 color)
		{
			return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}

		public void ClickButtonCancel()
		{
			base.scene.controller.color.CancelColorPicker();
		}

		public void ClickButtonOk()
		{
			base.scene.controller.color.ConfirmColorPicker();
		}
	}
}
