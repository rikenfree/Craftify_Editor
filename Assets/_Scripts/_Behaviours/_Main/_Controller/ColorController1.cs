using System.Globalization;
using UnityEngine;

namespace Main.Controller
{
	public class ColorController1 : SceneElement1
	{
		public Color currentColor;

		private void Start()
		{
			UpdateCurrentColor(HexToColor("3B99D8"));
			base.scene.view.colorPickerNew.SyncOutsideColor(currentColor);
		}

		public void OpenColorPicker()
		{
			base.scene.view.colorPickerNew.gameObject.SetActive(value: true);
		}

		public void CancelColorPicker()
		{
            SoundController1.Instance.PlayClickSound();
			base.scene.view.colorPickerNew.gameObject.SetActive(value: false);
		}

		public void UpdateCurrentColor(Color color)
		{
			currentColor = color;
			base.scene.view.masterCavas.UpdateButtonColor(currentColor);
		}

		public void ConfirmColorPicker()
		{
			base.scene.view.colorPickerNew.gameObject.SetActive(value: false);
			UpdateCurrentColor(base.scene.view.colorPickerNew.currentColor);
		}

		public void SetColor(Color color)
		{
			UpdateCurrentColor(color);
		}

		private Color HexToColor(string hex)
		{
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			return new Color32(r, g, b, byte.MaxValue);
		}
	}
}
