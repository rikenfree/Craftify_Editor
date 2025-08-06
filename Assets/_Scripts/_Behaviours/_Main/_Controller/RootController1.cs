using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Controller
{
	public class RootController1 : SceneElement1
	{
		public PencilController1 pencil;

		public CameraController1 camera;

		public BucketController1 bucket;

		public EraserController1 eraser;

		public ColorController1 color;

		public DropperController1 dropper;

		public SoundController1 sound;

		public BodyPartsController1 bodyParts;

		public ClothingController1 clothing;

		public UndoRedoController1 undoRedo;

		public NewCharacterController1 newCharacter;

		//public AdController ad;

		public RandomSkinController1 randomSkin;

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
            if (camera.isAbleToRotate)
            {
				return false;
			}

			if (!colorPickerObject.activeSelf && !bodyPartsViewerObject.activeSelf)
			{
				return !newCharacterObject.activeSelf;
			}
			return false;
		}		
	}
}