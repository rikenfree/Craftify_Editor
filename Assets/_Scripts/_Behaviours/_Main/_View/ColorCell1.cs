using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
	public class ColorCell1 : SceneElement1
	{
		private Color color;

		public void SetColor(Color color)
		{
			this.color = color;
			GetComponent<Image>().color = color;
		}

		public void ClickCell()
		{
			base.scene.controller.color.SetColor(color);
			base.scene.view.colorGridCanvas.gameObject.SetActive(value: false);
			base.scene.view.colorPickerNew.SyncOutsideColor(color);
		}
	}
}
