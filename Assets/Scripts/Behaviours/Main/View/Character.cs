//using EasyMobile;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace Main.View
{
	public class Character : SceneElement
	{
		public Texture2D skin;

		public Transform head;

		public Transform body;

		public Transform leftArm;

		public Transform rightArm;

		public Transform leftLeg;

		public Transform rightLeg;

		public Transform headCloth;

		public Transform bodyCloth;

		public Transform leftArmCloth;

		public Transform rightArmCloth;

		public Transform leftLegCloth;

		public Transform rightLegCloth;

		private Color[] colorMaps;

		private BodyPart[] bodyParts;

		private Texture2D steveTexture;

		private Texture2D blankTexture;

		private string mcpeCustomSkinPath = "games/com.mojang/minecraftpe/custom.png";

		private string mcpeCustomSkinFolderPath = "games/com.mojang/minecraftpe";

		private bool tryAutoLoad;

		

		private void Awake()
		{
			InitDefaultSkin();
			InitBodyParts();
			tryAutoLoad = false;
		}

		private void Update()
		{
			if (!tryAutoLoad)
			{
				AutoLoad();
				tryAutoLoad = true;
			}
		}

		private void InitDefaultSkin()
		{
			skin = Resources.Load<Texture2D>("Skins/empty");
			blankTexture = Resources.Load<Texture2D>("Skins/blank");
			steveTexture = Resources.Load<Texture2D>("Skins/steve");
			LoadSteveSkin();
		}

		private void InitBodyParts()
		{
			Component[] componentsInChildren = GetComponentsInChildren(typeof(BodyPart), includeInactive: true);
			bodyParts = new BodyPart[componentsInChildren.Length];
			for (int i = 0; i < bodyParts.Length; i++)
			{
				bodyParts[i] = componentsInChildren[i].GetComponent<BodyPart>();
			}
		}

		public Color GetColor(int i, int j)
		{
			return colorMaps[j * skin.width + i];
		}

		public int SkinIndex {

			get { return PlayerPrefs.GetInt("SkinIndex", 0); }
			set { PlayerPrefs.SetInt("SkinIndex",value); }
		}

		public void LoadSteveSkin()
		{
			steveTexture = Resources.Load<Texture2D>("Skins/steve");
			Paint64x64TextureOnSkin(steveTexture);
			colorMaps = skin.GetPixels();
			SSEventManager.Instance.SSGameStarEventCall(SkinIndex);
		}

		public void LoadBlankSkin()
		{
            SSEventManager.Instance.SSGameStarEventCall(SkinIndex);
            Paint64x32TextureOnSkin(blankTexture);
			colorMaps = skin.GetPixels();
		}

        public void LoadSkinGallery()
        {
            NativeGallery.GetImageFromGallery((path) =>
            {
                Debug.Log("Image path: " + path);
                if (path != null)
                {
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, 64, false);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    HandleTexture(texture);
                }
            }, "Select a PNG image", "image/png");
        }


        public void LoadMCPE()
		{
            string path = new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory", Array.Empty<object>()).Call<string>("getPath", Array.Empty<object>());
            string mcpePath = path + "/" + mcpeCustomSkinFolderPath;
            LoadMCPECallBack(mcpePath);
            //SA_Singleton<AndroidNativeUtility>.instance.GetExternalStoragePath();
        }

		public void LoadSkinOnine(string name)
		{
			string url = "https://minotar.net/skin/" + name + ".png";
            string pattern = @"[\s\n\r]+";

            // Use Regex.Replace to remove spaces and newlines
            string cleanedUrl = Regex.Replace(url, pattern, "");
            //url.Replace("\n", "");
			StartCoroutine(Load(cleanedUrl, random: false));
            SSEventManager.Instance.SSGameStarEventCall(SkinIndex);
        }

		public void LoadRandomSkinOnine()
		{
			string str = base.scene.controller.randomSkin.next();
			string url = "https://minotar.net/skin/" + str + ".png";
            string pattern = @"[\s\n\r]+";

            // Use Regex.Replace to remove spaces and newlines
            string cleanedUrl = Regex.Replace(url, pattern, "");
            Debug.Log("Skin URL => " + cleanedUrl);
			StartCoroutine(Load(cleanedUrl, random: true));
            SSEventManager.Instance.SSGameStarEventCall(SkinIndex);
        }

		public void SaveMCPE()
		{
			string path = new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory", Array.Empty<object>()).Call<string>("getPath", Array.Empty<object>());
			string mcpePath = path + "/" + mcpeCustomSkinFolderPath;
			Debug.Log("path :" + mcpePath);
			SaveMCPECallBack(mcpePath);
			//SA_Singleton<AndroidNativeUtility>.instance.GetExternalStoragePath();
			if (Main.Controller.RatingController.instance.Rate == 0)
			{
				Main.Controller.RatingController.instance.rateCanvas.gameObject.SetActive(true);

			}

		}

		public void SaveGallery()
		{
			UpdateSkinFromPixels();
			//Media.Gallery.Pick(PickFromGalleryCallback)
			//SA_Singleton<AndroidCamera>.instance.OnImageSaved += SaveGalleryCallBack;
			//Media.Gallery.SaveImage(skin, "test", ImageFormat.PNG, SaveImageCallback);
			//SA_Singleton<AndroidCamera>.instance.SaveImageToGalalry(skin);


			NativeGallery.SaveImageToGallery(skin, "SkinEditor", "skin", null);
            if (Main.Controller.RatingController.instance.Rate == 0)
            {
				Main.Controller.RatingController.instance.rateCanvas.gameObject.SetActive(true);

			}
			Main.Controller.RatingController.instance.succesfullPooup.SetActive(true);
            SSEventManager.Instance.SSGameWinEventCall(SkinIndex);
			SkinIndex++;
        }
		private void SaveImageCallback(string error)
		{
			if (!string.IsNullOrEmpty(error))
			{
				// There was an error, show it to users. 
				Debug.LogError("Save image has problem");
			}
			else
			{
				// The image's saved successfully. 
				Debug.LogError("Save image Sucess");
			}
		}
		//private void PickFromGalleryCallback(string error, MediaResult[] results)
		//{
		//	if (!string.IsNullOrEmpty(error))
		//	{
		//		// This means there was an error when picking items from Gallery.
		//		// You should show this error to users.
		//		Debug.LogError("image picker have error");
		//	}
		//	else
		//	{
		//		// Items have been selected successfully.
		//		// You can access them through the "results" parameter.

		//		// Loop through all the results.
		//		foreach (MediaResult result in results)
		//		{
		//			// You can use this field to check if the picked item is an image or a video.
		//			MediaType type = result.Type;

		//			// You can use this uri to load the item.
		//			string uri = result.Uri;

		//			LoadTextureFromDisk(uri);
		//		}
		//	}
		//}

		public void AutoSave()
		{
            //AndroidNativeUtility.ExternalStoragePathLoaded += AutoSaveCallBack;
            //SA_Singleton<AndroidNativeUtility>.instance.GetExternalStoragePath();
            string savePath = Application.persistentDataPath;
            AutoSaveCallBack(savePath);


        }

		public void AutoLoad()
		{
            //AndroidNativeUtility.ExternalStoragePathLoaded += AutoLoadCallBack;
            //SA_Singleton<AndroidNativeUtility>.instance.GetExternalStoragePath();
            string loadPath = Application.persistentDataPath;
            AutoLoadCallBack(loadPath);


        }

		private void AutoLoadCallBack(string path)
		{
			//AndroidNativeUtility.ExternalStoragePathLoaded -= AutoLoadCallBack;
			string path2 = path + "/autosave.png";
			if (File.Exists(path2))
			{
				byte[] data = File.ReadAllBytes(path2);
				Texture2D texture2D = Resources.Load<Texture2D>("Skins/disk_tmp");
				texture2D.LoadImage(data);
				Paint64x64TextureOnSkin(texture2D);
				colorMaps = skin.GetPixels();
				try
				{
					BodyPart[] array = bodyParts;
					foreach (BodyPart bodyPart in array)
					{
						if (bodyPart.HasPixelsData())
						{
							bodyPart.RefreshSkinData();
						}
					}
				}
				catch (Exception)
				{
					base.scene.view.errorCanvas.ShowCustomError("Attention", "Can't load autosaved skin.");
				}
			}
			else
			{
				print("Can't find autosaved file.");
			}
		}

		private void AutoSaveCallBack(string path)
		{
			//AndroidNativeUtility.ExternalStoragePathLoaded -= AutoSaveCallBack;
			UpdateSkinFromPixels();
			byte[] bytes = skin.EncodeToPNG();
			File.WriteAllBytes(path + "/autosave.png", bytes);
		}

		//private void SaveGalleryCallBack(GallerySaveResult result)
		//{
		//	SA_Singleton<AndroidCamera>.instance.OnImageSaved -= SaveGalleryCallBack;
		//	if (result.IsSucceeded)
		//	{
		//		base.scene.view.infoCanvas.ShowGalleryExportSuccessInfo();
		//	}
		//	else
		//	{
		//		base.scene.view.errorCanvas.ShowCantSaveGallerySkinError();
		//	}
		//}

		private void SaveMCPECallBack(string path)
		{
			//AndroidNativeUtility.ExternalStoragePathLoaded -= SaveMCPECallBack;
			string text = SearchMCPEFolder(path);
			if (text != null)
			{
				UpdateSkinFromPixels();
				byte[] bytes = skin.EncodeToPNG();
				File.WriteAllBytes(text + "/custom.png", bytes);
				base.scene.view.infoCanvas.ShowMCPEExportSuccessInfo();
			}
			else
			{
				base.scene.view.errorCanvas.ShowCantSaveMCPESkinError();
			}
		}

		public void SaveSkinToPath(string path)
		{
			byte[] bytes = skin.EncodeToPNG();
			File.WriteAllBytes(path, bytes);
		}

		private void UpdateSkinFromPixels()
		{
			Color[,] array = CurrentPixelColors();
			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					skin.SetPixel(i, j, array[i, j]);
				}
			}
			colorMaps = skin.GetPixels();
		}

		private Color[,] CurrentPixelColors()
		{
			Color[,] result = new Color[64, 64];
			BodyPart[] array = bodyParts;
			foreach (BodyPart bodyPart in array)
			{
				for (int j = 0; j < bodyPart.width; j++)
				{
					for (int k = 0; k < bodyPart.height; k++)
					{
						Colorize(result, bodyPart, j, k);
					}
				}
			}
			return result;
		}

		private void Colorize(Color[,] skin, BodyPart bp, int i, int j)
		{
			Color pixelColor = bp.GetPixelColor(i, j);
			if (!(pixelColor == default(Color)))
			{
				skin[bp.skinX + i, bp.skinY + j] = pixelColor;
			}
		}

		private void LoadMCPECallBack(string path)
		{
			//AndroidNativeUtility.ExternalStoragePathLoaded -= LoadMCPECallBack;
			string text = SearchMCPESkin(path);
			if (text != null)
			{
				LoadTextureFromDisk(text);
			}
			else
			{
				base.scene.view.errorCanvas.ShowCantFindMCPESkinError();
			}
		}

		private string SearchMCPESkin(string dataPath)
		{
			string[] array = dataPath.Split('/');
			string text = "/";
			if (FoundMCPESkin(text))
			{
				return text + mcpeCustomSkinPath;
			}
			for (int i = 0; i < array.Length; i++)
			{
				text = text + array[i] + "/";
				if (FoundMCPESkin(text))
				{
					return text + mcpeCustomSkinPath;
				}
			}
			return null;
		}

		private string SearchMCPEFolder(string dataPath)
		{
			string[] array = dataPath.Split('/');
			string text = "/";
			if (FoundMCPEFolder(text))
			{
				return text + mcpeCustomSkinFolderPath;
			}
			for (int i = 0; i < array.Length; i++)
			{
				text = text + array[i] + "/";
				if (FoundMCPEFolder(text))
				{
					return text + mcpeCustomSkinFolderPath;
				}
			}
			return null;
		}

		private void LoadTextureFromDisk(string texturePath)
		{
			try
			{
				byte[] data = File.ReadAllBytes(texturePath);
				Texture2D texture2D = Resources.Load<Texture2D>("Skins/disk_tmp");
				texture2D.LoadImage(data);
				HandleTexture(texture2D);
			}
			catch (Exception)
			{
				base.scene.view.errorCanvas.ShowCantFindMCPESkinError();
			}
		}

		private bool FoundMCPESkin(string path)
		{
			return File.Exists(path + mcpeCustomSkinPath);
		}

		private bool FoundMCPEFolder(string path)
		{
			return Directory.Exists(path + mcpeCustomSkinFolderPath);
		}

		private IEnumerator Load(string url, bool random)
		{

            using (UnityWebRequest www2 = UnityWebRequestTexture.GetTexture(url))
            {
                yield return www2.SendWebRequest();

                if (www2.result == UnityWebRequest.Result.ConnectionError || www2.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(url+ "  Error: " + www2.error);
                    base.scene.view.errorCanvas.ShowLoadingSkinOnlineError(random);
                }
                else
                {
                    Debug.LogError("Sucess! ");
                    // Get the downloaded texture
                    Texture2D texture = DownloadHandlerTexture.GetContent(www2);

                    // Use the texture as needed (e.g., set it to a material)
                   // GetComponent<Renderer>().material.mainTexture = texture;
                    HandleTexture(texture);
                }
            }




            Debug.LogError("Load Random Skin => " + url);
			//WWW www = new WWW(url);
			//Texture2D tmpTexture = Resources.Load<Texture2D>("Skins/internet_tmp");
			//yield return www;
			//if (www.error == null)
			//{
   //             Debug.LogError("Load Random Skin no error ");
   //             www.LoadImageIntoTexture(tmpTexture);
			//	HandleTexture(tmpTexture);
			//}
			//else
			//{
   //             Debug.LogError(" error errorerrorerrorerror ");
   //             Debug.LogError(www.error);
			//	base.scene.view.errorCanvas.ShowLoadingSkinOnlineError(random);
			//}
			base.scene.view.waitingCanvas.Hide();
		}

		public void HandleTexture(Texture2D texture)
		{
			if (texture.width == 64 && texture.height == 32)
			{
				Paint64x32TextureOnSkin(texture);
				colorMaps = skin.GetPixels();
				base.scene.view.newCharacter.ReloadBodyPartsData();
				MonoBehaviour.print("New Skin Loaded: 64x32");
			}
			else if (texture.width == 64 && texture.height == 64)
			{
				Paint64x64TextureOnSkin(texture);
				colorMaps = skin.GetPixels();
				base.scene.view.newCharacter.ReloadBodyPartsData();
				MonoBehaviour.print("New Skin Loaded: 64x64");
			}
			else
			{
				base.scene.view.errorCanvas.ShowSkinWrongSizeError();
				MonoBehaviour.print("Incorrect Size");
			}
		}

		private void Paint64x64TextureOnSkin(Texture2D s)
		{
			if (s.width != 64 || s.height != 64)
			{
				return;
			}
			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					skin.SetPixel(i, j, s.GetPixel(i, j));
				}
			}
		}

		private void Paint64x32TextureOnSkin(Texture2D s)
		{
			if (s.width != 64 || s.height != 32)
			{
				return;
			}
			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					Color color = default(Color);
					color.r = 0f;
					color.g = 0f;
					color.b = 0f;
					color.a = 0f;
					skin.SetPixel(i, j, color);
				}
			}
			for (int k = 0; k < 64; k++)
			{
				for (int l = 0; l < 32; l++)
				{
					skin.SetPixel(k, 32 + l, s.GetPixel(k, l));
				}
			}
			for (int m = 0; m < 4; m++)
			{
				for (int n = 0; n < 12; n++)
				{
					skin.SetPixel(27 - m, n, s.GetPixel(m, n));
				}
			}
			for (int num = 4; num < 8; num++)
			{
				for (int num2 = 0; num2 < 16; num2++)
				{
					skin.SetPixel(27 - num, num2, s.GetPixel(num, num2));
				}
			}
			for (int num3 = 8; num3 < 12; num3++)
			{
				for (int num4 = 0; num4 < 12; num4++)
				{
					skin.SetPixel(27 - num3, num4, s.GetPixel(num3, num4));
				}
			}
			for (int num5 = 12; num5 < 16; num5++)
			{
				for (int num6 = 0; num6 < 12; num6++)
				{
					skin.SetPixel(43 - num5, num6, s.GetPixel(num5, num6));
				}
			}
			for (int num7 = 8; num7 < 12; num7++)
			{
				for (int num8 = 12; num8 < 16; num8++)
				{
					skin.SetPixel(35 - num7, num8, s.GetPixel(num7, num8));
				}
			}
			for (int num9 = 40; num9 < 44; num9++)
			{
				for (int num10 = 0; num10 < 12; num10++)
				{
					skin.SetPixel(83 - num9, num10, s.GetPixel(num9, num10));
				}
			}
			for (int num11 = 44; num11 < 48; num11++)
			{
				for (int num12 = 0; num12 < 16; num12++)
				{
					skin.SetPixel(83 - num11, num12, s.GetPixel(num11, num12));
				}
			}
			for (int num13 = 48; num13 < 52; num13++)
			{
				for (int num14 = 0; num14 < 12; num14++)
				{
					skin.SetPixel(83 - num13, num14, s.GetPixel(num13, num14));
				}
			}
			for (int num15 = 52; num15 < 56; num15++)
			{
				for (int num16 = 0; num16 < 12; num16++)
				{
					skin.SetPixel(99 - num15, num16, s.GetPixel(num15, num16));
				}
			}
			for (int num17 = 48; num17 < 52; num17++)
			{
				for (int num18 = 12; num18 < 16; num18++)
				{
					skin.SetPixel(91 - num17, num18, s.GetPixel(num17, num18));
				}
			}
		}
	}
}
