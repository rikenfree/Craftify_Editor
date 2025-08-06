using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
	public class BodyPartsViewer : SceneElement
	{
		public Image head;

		public Image body;

		public Image leftArm;

		public Image rightArm;

		public Image leftLeg;

		public Image rightLeg;

		public void Refresh()
		{
			head.color = GetColor(base.scene.view.character.head);
			body.color = GetColor(base.scene.view.character.body);
			leftArm.color = GetColor(base.scene.view.character.leftArm);
			rightArm.color = GetColor(base.scene.view.character.rightArm);
			leftLeg.color = GetColor(base.scene.view.character.leftLeg);
			rightLeg.color = GetColor(base.scene.view.character.rightLeg);
		}

		public void ClickButtonOk()
		{
			base.scene.controller.sound.PlayClickSound();
			base.scene.controller.bodyParts.CloseBodyPartsViewer();
		}

		public Color GetColor(Transform part)
		{
			if (!part.gameObject.activeSelf)
			{
				return Color.red;
			}
			return Color.green;
		}

		public void Toggle(Transform part)
		{
			bool activeSelf = part.gameObject.activeSelf;
			part.gameObject.SetActive(!activeSelf);
		}

		public void ToggleHead()
		{
			Toggle(base.scene.view.character.head);
			base.scene.controller.sound.PlayClickSound();
			Refresh();
		}

		public void ToggleBody()
		{
			Toggle(base.scene.view.character.body);
			base.scene.controller.sound.PlayClickSound();
			Refresh();
		}

		public void ToggleLeftArm()
		{
			Toggle(base.scene.view.character.leftArm);
			base.scene.controller.sound.PlayClickSound();
			Refresh();
		}

		public void ToggleRightArm()
		{
			Toggle(base.scene.view.character.rightArm);
			base.scene.controller.sound.PlayClickSound();
			Refresh();
		}

		public void ToggleLeftLeg()
		{
			Toggle(base.scene.view.character.leftLeg);
			base.scene.controller.sound.PlayClickSound();
			Refresh();
		}

		public void ToggleRightLeg()
		{
			Toggle(base.scene.view.character.rightLeg);
			base.scene.controller.sound.PlayClickSound();
			Refresh();
		}
	}
}
