using Main.Controller;

namespace Main.View
{
	public class ColorGridCanvas1 : SceneElement1
	{
		public void ClickButtonCancel()
		{
            SoundController1.Instance.PlayClickSound();
            base.gameObject.SetActive(value: false);
		}

		public void ClickButtonMoreColor()
		{
            SoundController1.Instance.PlayClickSound();
            base.gameObject.SetActive(value: false);
			base.scene.controller.color.OpenColorPicker();
		}
	}
}
