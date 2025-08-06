using UnityEngine;

namespace Main.Controller
{
	public class RootController : SceneElement
	{
		public PencilController pencil;

		public CameraController camera;

		public BucketController bucket;

		public EraserController eraser;

		public ColorController color;

		public DropperController dropper;

		public SoundController sound;

		public BodyPartsController bodyParts;

		public ClothingController clothing;

		public UndoRedoController undoRedo;

		public NewCharacterController newCharacter;

		//public AdController ad;

		public RandomSkinController randomSkin;

		//public GAController ga;

		private GameObject colorPickerObject;

		private GameObject bodyPartsViewerObject;

		private GameObject newCharacterObject;

		private void Awake()
		{
			colorPickerObject = base.scene.view.colorPickerNew.gameObject;
			bodyPartsViewerObject = base.scene.view.bodyPartsViewer.gameObject;
			newCharacterObject = base.scene.view.newCharacter.gameObject;
		}

		public bool AllowAction()
		{
			if (!colorPickerObject.activeSelf && !bodyPartsViewerObject.activeSelf)
			{
				return !newCharacterObject.activeSelf;
			}
			return false;
		}

		
	}
}
