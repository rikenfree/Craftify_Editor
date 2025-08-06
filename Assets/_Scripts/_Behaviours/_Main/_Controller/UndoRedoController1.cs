using Main.View;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main.Controller
{
	public class UndoRedoController1 : SceneElement1
	{

		public static UndoRedoController1 Instance;

		private PencilController1 pencil;

		private BucketController1 bucket;

		private EraserController1 eraser;

		private Cape1 character;

		public BodyPart1[] bodyCapeParts;
		public BodyPart1[] bodyelytraParts;

		private List<Color[,]> leftStack;

		private List<Color[,]> rightStack;


        private void Awake()
        {
            if (Instance==null)
            {
				Instance = this;
            }
        }

        private void Start()
		{
			pencil = base.scene.controller.pencil;
			bucket = base.scene.controller.bucket;
			eraser = base.scene.controller.eraser;
			InitStacks();
		//	InitBodyParts();
		}

		public void InitStacks()
		{
			leftStack = new List<Color[,]>();
			rightStack = new List<Color[,]>();
		}

		//private void InitBodyParts()
		//{
		//	character = base.scene.view.cape;
		//	Component[] componentsInChildren = character.GetComponentsInChildren(typeof(BodyPart), includeInactive: false);
		//	bodyParts = new BodyPart[componentsInChildren.Length];
		//	for (int i = 0; i < bodyParts.Length; i++)
		//	{
		//		bodyParts[i] = componentsInChildren[i].GetComponent<BodyPart>();
		//	}
		//}

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


			if (CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C2217 || CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C6432)
			{

				rightStack.Add(PopLast(leftStack));
				Color[,] skin = PeekLast(leftStack);
				BodyPart1[] array = bodyCapeParts;
				//Debug.LogError("Array:" + array.Length);
				foreach (BodyPart1 bodyPart in array)
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
			else if(CapeController.Instance.currentcap.CurrentResolution == CapeResolution.Elytra6432)
			{
				rightStack.Add(PopLast(leftStack));
				Color[,] skin = PeekLast(leftStack);
				BodyPart1[] array = bodyelytraParts;
				//Debug.LogError("Array:" + array.Length);
				foreach (BodyPart1 bodyPart in array)
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



		}

		public void Redo()
		{
			if (rightStack.Count <= 0)
			{
				return;
			}
			if (CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C2217 || CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C6432)
			{
				Color[,] array = PopLast(rightStack);
				leftStack.Add(array);
				BodyPart1[] array2 = bodyCapeParts;
				foreach (BodyPart1 bodyPart in array2)
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
			else if (CapeController.Instance.currentcap.CurrentResolution == CapeResolution.Elytra6432)
			{
				
					Color[,] array = PopLast(rightStack);
				leftStack.Add(array);
				BodyPart1[] array2 = bodyelytraParts;
				foreach (BodyPart1 bodyPart in array2)
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
		}

		private void SaveSkinSnapshot()
		{
			//Color[,] array = new Color[64, 64];//my cmnt

			if (CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C2217 || CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C6432)
			{
				Color[,] array = new Color[22, 17];
				BodyPart1[] array2 = bodyCapeParts;
				foreach (BodyPart1 bodyPart in array2)
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
					//if (!SkinsEqual(c, array))//my cmnt
					if (!SkinsEqual22x17(c, array))
					{
						leftStack.Add(array);
					}
				}
				else
				{
					leftStack.Add(array);
				}

			}
			else if (CapeController.Instance.currentcap.CurrentResolution == CapeResolution.Elytra6432)
			{
				Color[,] array = new Color[24, 22];
				BodyPart1[] array2 = bodyelytraParts;
				foreach (BodyPart1 bodyPart in array2)
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
					//if (!SkinsEqual(c, array))//my cmnt
					if (!SkinsEqual22x17(c, array))
					{
						leftStack.Add(array);
					}
				}
				else
				{
					leftStack.Add(array);
				}


			}
				
		}

		private void Colorize(Color[,] skin, BodyPart1 bp, int i, int j)
		{
			Color pixelColor = bp.GetPixelColor(i, j);
			if (!(pixelColor == default(Color)))
			{
				skin[bp.skinX + i, bp.skinY + j] = pixelColor;
			}
		}

		private void RestoreColor(Color[,] skin, BodyPart1 bp, int i, int j)
		{
            //Debug.LogError("i:" + i + "   :j:" + j + "  :skin:" + skin.Length + "   :i:" + bp.skinX + i + "  :j:" + bp.skinY + j);
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

        private bool SkinsEqual22x17(Color[,] c1, Color[,] c2)
        {
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 17; j++)
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
