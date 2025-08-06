using Main.View;

namespace Main.Controller
{
	public class ClothingController : SceneElement
	{
		public bool clothingOn;

		private void Start()
		{
			clothingOn = true;
			ToggleClothing();
		}

		public void ToggleClothing()
		{
			clothingOn = !clothingOn;
			Character character = base.scene.view.character;
			character.headCloth.gameObject.SetActive(clothingOn);
			character.bodyCloth.gameObject.SetActive(clothingOn);
			character.leftArmCloth.gameObject.SetActive(clothingOn);
			character.rightArmCloth.gameObject.SetActive(clothingOn);
			character.leftLegCloth.gameObject.SetActive(clothingOn);
			character.rightLegCloth.gameObject.SetActive(clothingOn);
		}
	}
}
