using UnityEngine;

namespace Main.View
{
	public class RootView : SceneElement
	{
		public Transform camera;

		public Character character;

		public MasterCanvas masterCavas;

		public ColorPickerNewCanvas colorPickerNew;

		public BodyPartsViewer bodyPartsViewer;

		public NewCharacterCanvas newCharacter;

		public WaitingCanvas waitingCanvas;

		public ErrorCanvas errorCanvas;

		public SaveCharacterCanvas saveCharacter;

		public InfoCanvas infoCanvas;

		public MenuCanvas menuCanvas;

		public ColorGridCanvas colorGridCanvas;

		public ColorOptionCanvas colorOptionCanvas;
	}
}
