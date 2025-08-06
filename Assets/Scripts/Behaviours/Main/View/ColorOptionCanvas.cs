namespace Main.View
{
	public class ColorOptionCanvas : SceneElement
	{
		public void ClickButtonGrid()
		{
			base.scene.controller.sound.PlayClickSound();
			base.gameObject.SetActive(value: false);
			base.scene.view.colorGridCanvas.gameObject.SetActive(value: true);
		}

		public void ClickButtonGradient()
		{
			base.scene.controller.sound.PlayClickSound();
			base.gameObject.SetActive(value: false);
			base.scene.view.colorPickerNew.gameObject.SetActive(value: true);
		}

		public void ClickButtonCancel()
		{
			base.scene.controller.sound.PlayClickSound();
			base.gameObject.SetActive(value: false);
		}
	}
}
