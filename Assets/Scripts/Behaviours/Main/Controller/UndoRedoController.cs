using Main.View;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main.Controller
{
	public class UndoRedoController : SceneElement
	{
		private PencilController pencil;

		private BucketController bucket;

		private EraserController eraser;

		private Character character;

		private BodyPart[] bodyParts;

		private List<Color[,]> leftStack;

		private List<Color[,]> rightStack;

		private void Start()
		{
			pencil = base.scene.controller.pencil;
			bucket = base.scene.controller.bucket;
			eraser = base.scene.controller.eraser;
			InitStacks();
			InitBodyParts();
		}

		private void InitStacks()
		{
			leftStack = new List<Color[,]>();
			rightStack = new List<Color[,]>();
		}

		private void InitBodyParts()
		{
			character = base.scene.view.character;
			Component[] componentsInChildren = character.GetComponentsInChildren(typeof(BodyPart), includeInactive: true);
			bodyParts = new BodyPart[componentsInChildren.Length];
			for (int i = 0; i < bodyParts.Length; i++)
			{
				bodyParts[i] = componentsInChildren[i].GetComponent<BodyPart>();
			}
		}

		private bool AllowDetection()
		{
			if (!pencil.gameObject.activeSelf && !bucket.gameObject.activeSelf)
			{
				return eraser.gameObject.activeSelf;
			}
			return true;
		}

		private bool BlockedByGui()
		{
			return IsPointerOverUIObject();
		}

		private bool IsPointerOverUIObject()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, list);
			return list.Count > 0;
		}

		private void Update()
		{
			if (AllowDetection())
			{
				if (leftStack.Count == 0 && Input.GetMouseButtonDown(0) && !BlockedByGui())
				{
					SaveSkinSnapshot();
				}
				if (Input.GetMouseButtonUp(0) && !BlockedByGui())
				{
					SaveSkinSnapshot();
					rightStack.Clear();
				}
			}
		}

		public void Undo()
		{
			if (leftStack.Count <= 1)
			{
				return;
			}
			rightStack.Add(PopLast(leftStack));
			Color[,] skin = PeekLast(leftStack);
			BodyPart[] array = bodyParts;
			foreach (BodyPart bodyPart in array)
			{
				for (int j = 0; j < bodyPart.width; j++)
				{
					for (int k = 0; k < bodyPart.height; k++)
					{
						RestoreColor(skin, bodyPart, j, k);
					}
				}
			}
		}

		public void Redo()
		{
			if (rightStack.Count <= 0)
			{
				return;
			}
			Color[,] array = PopLast(rightStack);
			leftStack.Add(array);
			BodyPart[] array2 = bodyParts;
			foreach (BodyPart bodyPart in array2)
			{
				for (int j = 0; j < bodyPart.width; j++)
				{
					for (int k = 0; k < bodyPart.height; k++)
					{
						RestoreColor(array, bodyPart, j, k);
					}
				}
			}
		}

		private void SaveSkinSnapshot()
		{
			Color[,] array = new Color[64, 64];
			BodyPart[] array2 = bodyParts;
			foreach (BodyPart bodyPart in array2)
			{
				for (int j = 0; j < bodyPart.width; j++)
				{
					for (int k = 0; k < bodyPart.height; k++)
					{
						Colorize(array, bodyPart, j, k);
					}
				}
			}
			if (leftStack.Count > 0)
			{
				Color[,] c = leftStack[leftStack.Count - 1];
				if (!SkinsEqual(c, array))
				{
					leftStack.Add(array);
				}
			}
			else
			{
				leftStack.Add(array);
			}
		}

		private void Colorize(Color[,] skin, BodyPart bp, int i, int j)
		{
			Color pixelColor = bp.GetPixelColor(i, j);
			if (!(pixelColor == default(Color)))
			{
				skin[bp.skinX + i, bp.skinY + j] = pixelColor;
			}
		}

		private void RestoreColor(Color[,] skin, BodyPart bp, int i, int j)
		{
			bp.SetPixelColor(i, j, skin[bp.skinX + i, bp.skinY + j]);
		}

		private bool SkinsEqual(Color[,] c1, Color[,] c2)
		{
			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					if (c1[i, j] != c2[i, j])
					{
						return false;
					}
				}
			}
			return true;
		}

		private Color[,] PopLast(List<Color[,]> stack)
		{
			Color[,] result = stack[stack.Count - 1];
			stack.RemoveRange(stack.Count - 1, 1);
			return result;
		}

		private Color[,] PeekLast(List<Color[,]> stack)
		{
			return stack[stack.Count - 1];
		}
	}
}
