namespace Main.View
{
	public class ColorGridCanvas : SceneElement
	{
		public void ClickButtonCancel()
		{
			base.scene.controller.sound.PlayClickSound();
			base.gameObject.SetActive(value: false);
		}

		public void ClickButtonMoreColor()
		{
			base.scene.controller.sound.PlayClickSound();
			base.gameObject.SetActive(value: false);
			base.scene.controller.color.OpenColorPicker();
		}
	}
}
