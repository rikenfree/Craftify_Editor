using UnityEngine;

namespace Main.View
{
	public class BodyPart1 : SceneElement1
	{

		public int itemindex;

		public int width;

		public int height;

		public int skinX;

		public int skinY;

		public Pixel1 pixelPrefab;

		public bool isClothing;

		private Cape1 character;
	//	private Elytra elytra;

		private Pixel1[,] pixels;

		private void Start()
		{
			character = base.scene.view.cape;
		//	elytra = base.scene.view.elytra;
			InitPixels();
            if (itemindex==0)
            {

			RefreshSkinData();
            }
            else if (itemindex==1)
            {
				RefreshElytraData();
            }
		}

		private void InitPixels()
		{
			pixels = new Pixel1[width, height];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Pixel1 pixel = Object.Instantiate(pixelPrefab, default(Vector3), Quaternion.identity);
					pixel.transform.parent = base.transform;
					pixel.transform.localPosition = new Vector3((float)i * 0.2f, (float)j * 0.2f, 0f);
					pixel.transform.rotation = base.transform.rotation;
					pixel.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
					pixel.isClothing = isClothing;
					pixel.i = i;
					pixel.j = j;
					pixel.parent = this;
					pixels[i, j] = pixel;
				}
			}
		}

		public bool HasPixelsData()
		{
			return pixels != null;
		}

		public void RefreshSkinData()
		{
			//Debug.Log("RefreshSkinData");
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Color color = character.GetColor(skinX + i, skinY + j);
					pixels[i, j].ChangeColor(color);
					pixels[i, j].SetDefaultColor(color);
				}
			}
		}

		public void RefreshElytraData()
		{
			Debug.Log("RefreshElytraData");

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Color color = character.GetColor(skinX + i, skinY + j);
					pixels[i, j].ChangeColor(color);
					pixels[i, j].SetDefaultColor(color);
				}
			}
		}

		public void PainAllPixels(Color color)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					pixels[i, j].ChangeColor(color);
				}
			}
		}

		public void SetPixelColor(int i, int j, Color color)
		{
			if (pixels != null && pixels[i, j] != null)
			{
				pixels[i, j].ChangeColor(color);
			}
		}

		public Pixel1 GetPixel(int i, int j)
		{
			return pixels[i, j];
		}

		public Color GetPixelColor(int i, int j)
		{
			if (pixels == null || pixels[i, j] == null)
			{
				return default(Color);
			}
			return pixels[i, j].GetColor();
		}
	}
}
