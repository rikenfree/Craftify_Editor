namespace Main.Controller
{
	public class NewCharacterController1 : SceneElement1
	{
		public void OpenNewCharacterView()
		{
			base.scene.view.newCharacter.gameObject.SetActive(value: true);
		}

		public void CloseNewCharacterView()
		{
			base.scene.view.newCharacter.gameObject.SetActive(value: false);
		}
	}
}
