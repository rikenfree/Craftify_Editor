using Main.View;

namespace Main.Controller
{
	public class AutoSaveController : SceneElement
	{
		private Character character;

		private void Start()
		{
			character = base.scene.view.character;
			InvokeRepeating("AutoSave", 5f, 5f);
		}

		private void AutoSave()
		{
			character.AutoSave();
		}
	}
}
