using Main.Controller;
using UnityEngine;
using UnityEngine.UI;
using SuperStarSdk;
using TMPro;
namespace Main.View
{
	public class NewCharacterCanvas : SceneElement
	{
		public RectTransform onlineNameCanvas;

		public CatalogCanvas catalogCanvas;

		private Character character;

		private BodyPart[] bodyParts;

		private RootController ctrl;

		public GameObject daynamicTool;

		private void Awake()
		{
			ctrl = base.scene.controller;
			InitBodyParts();
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

		public void ReloadBodyPartsData()
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

		public void ClickCatalogButton()
		{
            SuperStarAd.Instance.ShowInterstitialTimer((result) => 
			{
			catalogCanvas.Show();
			base.scene.controller.sound.PlayClickSound();
			//base.scene.controller.newCharacter.CloseNewCharacterView();
             Debug.Log("Show Intrestitial  => " + result);
            });
            //ctrl.ga.SendEvent("Button Click", "New From Catalog", "");
        }

		public void ClickSkinPackCreator()
		{
			SuperStarAd.Instance.ShowInterstitialTimer((result) =>
			{
				DynamicSkinPackCreator.Instance.ResetData();
				daynamicTool.SetActive(true);
				base.scene.controller.sound.PlayClickSound();
				//base.scene.controller.newCharacter.CloseNewCharacterView();
				Debug.Log("Show Intrestitial  => " + result);
			});
		}


		public void ClickSteveButton()
		{
            SuperStarAd.Instance.ShowInterstitialTimer((result) => {
			character.LoadSteveSkin();
			ReloadBodyPartsData();
			base.scene.controller.sound.PlayClickSound();
			base.scene.controller.newCharacter.CloseNewCharacterView();
                Debug.Log("Show Intrestitial  => " + result);
            });
            //ctrl.ga.SendEvent("Button Click", "New From Steve", "");
        }

        public void ClickMCPEButton()
        {
            base.scene.controller.sound.PlayClickSound();
            base.scene.controller.newCharacter.CloseNewCharacterView();
            character.LoadMCPE();

            if (SuperStarAd.Instance.NoAds == 0)
            {
                SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
                {
                    Debug.Log("Show Interstitial => " + k);
                }, 3);
            }
            else
            {
                Debug.Log("NoAds is enabled, skipping interstitial");
            }

            // ctrl.ga.SendEvent("Button Click", "New From MCPE", "");
        }


        public void ClickGalleryButton()
		{
			base.scene.controller.sound.PlayClickSound();
			base.scene.controller.newCharacter.CloseNewCharacterView();
			character.LoadSkinGallery();
            //SuperStarAd.Instance.ShowForceInterstitialWithLoader((result) => {
            //    Debug.Log("Show Intrestitial  => " + result);
            //}, 1);
            //ctrl.ga.SendEvent("Button Click", "New From Gallery", "");
        }

		public void ClickSearchOnlineButton()
		{
            SuperStarAd.Instance.ShowInterstitialTimer((result) => {
			base.scene.controller.sound.PlayClickSound();
			base.scene.controller.newCharacter.CloseNewCharacterView();
			onlineNameCanvas.gameObject.SetActive(value: true);
                Debug.Log("Show Intrestitial  => " + result);
            });
            //	ctrl.ga.SendEvent("Button Click", "New From Online", "");
        }

		public void ClickSearchOnlineConfirm()
		{
            SuperStarAd.Instance.ShowInterstitialTimer((result) => {
	        TMP_InputField componentInChildren = onlineNameCanvas.GetComponentInChildren<TMP_InputField>();
			base.scene.view.waitingCanvas.ShowLoadingSkinOnlineWaiting();
			character.LoadSkinOnine(componentInChildren.text);
			base.scene.controller.sound.PlayClickSound();
			onlineNameCanvas.gameObject.SetActive(value: false);
                Debug.Log("Show Intrestitial  => " + result);
            });
        }

		public void ClickSearchOnlineCancel()
		{
			base.scene.controller.sound.PlayClickSound();
			onlineNameCanvas.gameObject.SetActive(value: false);
		}

		public void ClickRandomOnlineButton()
		{
            SuperStarAd.Instance.ShowInterstitialTimer((result) => {
			base.scene.view.waitingCanvas.ShowLoadingSkinOnlineWaiting();
			character.LoadRandomSkinOnine();
			base.scene.controller.sound.PlayClickSound();
			base.scene.controller.newCharacter.CloseNewCharacterView();
				Debug.Log("Show Intrestitial  => " + result);
			});
            //ctrl.ga.SendEvent("Button Click", "New From Random", "");
        }

		public void ClickButtonCancel()
		{
			base.scene.controller.sound.PlayClickSound();
			base.scene.controller.newCharacter.CloseNewCharacterView();
		}
	}
}
