using Main.Controller;

namespace Main.View
{
	public class ColorOptionCanvas1 : SceneElement1
	{
		public void ClickButtonGrid()
		{
            SoundController1.Instance.PlayClickSound();
			base.gameObject.SetActive(value: false);
			base.scene.view.colorGridCanvas.gameObject.SetActive(value: true);
		}

		public void ClickButtonGradient()
		{
            SoundController1.Instance.PlayClickSound();
			base.gameObject.SetActive(value: false);
			base.scene.view.colorPickerNew.gameObject.SetActive(value: true);
		}

		public void ClickButtonCancel()
		{
            SoundController1.Instance.PlayClickSound();
            base.gameObject.SetActive(value: false);
		}
	}
}
