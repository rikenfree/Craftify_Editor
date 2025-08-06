using Main.Controller;
using SuperStarSdk;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;

namespace Main.View
{
	public class CatalogCanvas : SceneElement
	{
		public TMP_InputField pageInput;

		public TextMeshProUGUI skinInfo;

		public Image[] images;

		private Texture2D[] skinsArray;

		private string[] namesArray;

		private string[] skinNames;

		private int page;

		private string skinUrl = "https://minotar.net/skin/";

		private string skinSuffix = ".png";

		private RootController ctrl;

		private void Awake()
		{
			ctrl = base.scene.controller;
		}

		private IEnumerator Load(string skinName, int index)
		{
			string url = skinUrl + skinName + skinSuffix;
            string pattern = @"[\s\n\r]+";

            // Use Regex.Replace to remove spaces and newlines
            string cleanedUrl = Regex.Replace(url, pattern, "");
            WWW www = new WWW(cleanedUrl);
			Texture2D downloadedTexture = new Texture2D(1, 1);
			yield return www;
			if (www.error == null)
			{
				www.LoadImageIntoTexture(downloadedTexture);
				Sprite sprite = Sprite.Create(formattedSkin(downloadedTexture), new Rect(0f, 0f, 160f, 320f), new Vector2(0.5f, 0.5f));
				images[index].sprite = sprite;
				skinsArray[index] = downloadedTexture;
				namesArray[index] = skinName;
			}
		}

		public void ApplySkin(int index)
		{
			if (!(skinsArray[index] == null))
			{
                SuperStarAd.Instance.ShowInterstitialTimer((result) => {
				base.scene.controller.sound.PlayClickSound();
				base.scene.view.character.HandleTexture(skinsArray[index]);
				//ctrl.ga.SendEvent("Button Click", "Apply Catalog", namesArray[index]);
				base.gameObject.SetActive(value: false);
                    Debug.Log("Show Intrestitial  => " + result);
                });
            }
		}

		private Sprite skinSprite(Texture2D texture)
		{
			return null;
		}

		private Texture2D formattedSkin(Texture2D texture)
		{
			int width = texture.width;
			int height = texture.height;
			Texture2D texture2D = new Texture2D(160, 320);
			for (int i = 0; i < 160; i++)
			{
				for (int j = 0; j < 320; j++)
				{
					texture2D.SetPixel(i, j, new Color(0f, 0f, 0f, 0f));
				}
			}
			for (int k = 0; k < 80; k++)
			{
				for (int l = 0; l < 80; l++)
				{
					texture2D.SetPixel(40 + k, 240 + l, texture.GetPixel(8 + k / 10, height - 16 + l / 10));
				}
			}
			for (int m = 0; m < 80; m++)
			{
				for (int n = 0; n < 120; n++)
				{
					texture2D.SetPixel(40 + m, 120 + n, texture.GetPixel(20 + m / 10, height - 32 + n / 10));
				}
			}
			for (int num = 0; num < 40; num++)
			{
				for (int num2 = 0; num2 < 120; num2++)
				{
					texture2D.SetPixel(120 + num, 120 + num2, texture.GetPixel(44 + num / 10, height - 32 + num2 / 10));
				}
			}
			for (int num3 = 0; num3 < 40; num3++)
			{
				for (int num4 = 0; num4 < 120; num4++)
				{
					texture2D.SetPixel(num3, 120 + num4, texture.GetPixel(44 + num3 / 10, height - 32 + num4 / 10));
				}
			}
			for (int num5 = 0; num5 < 40; num5++)
			{
				for (int num6 = 0; num6 < 120; num6++)
				{
					texture2D.SetPixel(80 + num5, num6, texture.GetPixel(4 + num5 / 10, height - 32 + num6 / 10));
				}
			}
			for (int num7 = 0; num7 < 40; num7++)
			{
				for (int num8 = 0; num8 < 120; num8++)
				{
					texture2D.SetPixel(40 + num7, num8, texture.GetPixel(4 + num7 / 10, height - 32 + num8 / 10));
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
			Refresh();
		}

		public void Refresh()
		{
			if (skinNames == null || skinNames.Length == 0)
			{
				InitCatalog();
				InitPage();
			}
			InitSkinsArray();
			LoadSkinsOnPage();
		}

		private void InitCatalog()
		{
			TextAsset textAsset = Resources.Load("catalog") as TextAsset;
			skinNames = textAsset.text.Split('\n');
		}

		private void InitPage()
		{
			page = 1;
			RefreshPageInput();
			RefreshSkinInfo();
		}

		private void InitSkinsArray()
		{
			namesArray = new string[8];
			skinsArray = new Texture2D[8];
		}

		private void LoadSkinsOnPage()
		{
			int num = (page - 1) * 8;
			int num2 = page * 8;
			for (int i = num; i < num2; i++)
			{
				string skinName = skinNames[i];
				StartCoroutine(Load(skinName, i % 8));
			}

		}

		private void RefreshPageInput()
		{
			pageInput.text = page.ToString();
		}

		private void RefreshSkinInfo()
		{
			int num = 1 + (page - 1) * 8;
			int num2 = page * 8;


			skinInfo.text = num + " - " + num2;
        }

		private int totalPage()
		{
			if (skinNames == null || skinNames.Length == 0)
			{
				return 1;
			}
			return skinNames.Length / 8;
		}

		public void PreviousPage()
		{
			base.scene.controller.sound.PlayClickSound();
			page--;
			if (page <= 0)
			{
				page = totalPage();
			}
			RefreshPageInput();
			RefreshSkinInfo();
			LoadSkinsOnPage();
            SuperStarAd.Instance.ShowInterstitialTimer((result) => {
                Debug.Log("Show Intrestitial  => " + result);
            });
        }

		public void NextPage()
		{
			base.scene.controller.sound.PlayClickSound();
			page++;
			if (page > totalPage())
			{
				page = 1;
			}
			RefreshPageInput();
			RefreshSkinInfo();
			LoadSkinsOnPage();
            SuperStarAd.Instance.ShowInterstitialTimer((result) => {
                Debug.Log("Show Intrestitial  => " + result);
            });
        }

		public void RandomPage()
		{
			base.scene.controller.sound.PlayClickSound();
			page = Random.Range(1, totalPage() - 1);
			RefreshPageInput();
			RefreshSkinInfo();
			LoadSkinsOnPage();
            SuperStarAd.Instance.ShowInterstitialTimer((result) => {
                Debug.Log("Show Intrestitial  => " + result);
            });
        }

		public void SelectedPage()
		{
            base.scene.controller.sound.PlayClickSound();
			page = int.Parse(pageInput.text);

			if (page > 2824)
			{
				page = 2824;
                pageInput.text = page.ToString();
            }

            RefreshPageInput();
            RefreshSkinInfo();
            LoadSkinsOnPage();
            SuperStarAd.Instance.ShowInterstitialTimer((result) => {
                Debug.Log("Show Intrestitial  => " + result);
            });
        }

		public void ButtonCancelClicked()
		{
			base.gameObject.SetActive(value: false);
			base.scene.controller.sound.PlayClickSound();
			//base.scene.controller.newCharacter.CloseNewCharacterView();
		}
	}
}
