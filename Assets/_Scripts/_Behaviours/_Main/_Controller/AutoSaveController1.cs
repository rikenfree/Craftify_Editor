using Main.View;

namespace Main.Controller
{
	public class AutoSaveController1 : SceneElement1
	{
		//private Character character;
        private Cape1 cape;

		private void Start()
		{
            //character = base.scene.view.character;
            //InvokeRepeating("AutoSave", 5f, 5f);

            cape = base.scene.view.cape;
            InvokeRepeating("AutoSave", 1f, 1f);
        }

		private void AutoSave()
		{
            //character.AutoSave();
            cape.AutoSave();
		}
	}
}
