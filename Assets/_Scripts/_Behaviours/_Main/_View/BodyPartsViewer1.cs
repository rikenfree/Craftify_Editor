using Main.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
	public class BodyPartsViewer1 : SceneElement1
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
            SoundController1.Instance.PlayClickSound();
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
            SoundController1.Instance.PlayClickSound();
            Refresh();
		}

		public void ToggleBody()
		{
			Toggle(base.scene.view.character.body);
            SoundController1.Instance.PlayClickSound();
            Refresh();
		}

		public void ToggleLeftArm()
		{
			Toggle(base.scene.view.character.leftArm);
            SoundController1.Instance.PlayClickSound();
            Refresh();
		}

		public void ToggleRightArm()
		{
			Toggle(base.scene.view.character.rightArm);
            SoundController1.Instance.PlayClickSound();
            Refresh();
		}

		public void ToggleLeftLeg()
		{
			Toggle(base.scene.view.character.leftLeg);
            SoundController1.Instance.PlayClickSound();
            Refresh();
		}

		public void ToggleRightLeg()
		{
			Toggle(base.scene.view.character.rightLeg);
            SoundController1.Instance.PlayClickSound();
            Refresh();
		}
	}
}
