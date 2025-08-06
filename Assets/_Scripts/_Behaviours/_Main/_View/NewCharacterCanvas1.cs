using Main.Controller;
using UnityEngine;
using UnityEngine.UI;
using SuperStarSdk;
using System.Collections;
using TMPro;

namespace Main.View
{
	public class NewCharacterCanvas1 : SceneElement1
	{
		public RectTransform onlineNameCanvas;

		public CatalogCanvas1 catalogCanvas;

		private Character1 character;

		private BodyPart1[] bodyParts;

		private RootController1 ctrl;

		private void Awake()
		{
			ctrl = base.scene.controller;
			InitBodyParts();
		}

		private void InitBodyParts()
		{
			character = base.scene.view.character;
			Component[] componentsInChildren = character.GetComponentsInChildren(typeof(BodyPart1), includeInactive: true);
			bodyParts = new BodyPart1[componentsInChildren.Length];
			for (int i = 0; i < bodyParts.Length; i++)
			{
				bodyParts[i] = componentsInChildren[i].GetComponent<BodyPart1>();
			}
		}

		public void ReloadBodyPartsData()
		{
			BodyPart1[] array = bodyParts;
			foreach (BodyPart1 bodyPart in array)
			{
				if (bodyPart.HasPixelsData())
				{
					bodyPart.RefreshSkinData();
				}
			}
		}

		public void ClickCatalogButton()
		{
			catalogCanvas.Show();
            SoundController1.Instance.PlayClickSound();
            base.scene.controller.newCharacter.CloseNewCharacterView();
            SuperStarAd.Instance.ShowInterstitialTimer(null);
			//ctrl.ga.SendEvent("Button Click", "New From Catalog", "");
		}

		public void ClickSteveButton()
		{
			character.LoadSteveSkin1();
			ReloadBodyPartsData();
            SoundController1.Instance.PlayClickSound();
            base.scene.controller.newCharacter.CloseNewCharacterView();
            SuperStarAd.Instance.ShowInterstitialTimer(null);
            //ctrl.ga.SendEvent("Button Click", "New From Steve", "");
        }

		public void ClickMCPEButton()
		{
            SoundController1.Instance.PlayClickSound();
            base.scene.controller.newCharacter.CloseNewCharacterView();
			character.LoadMCPE();
            SuperStarAd.Instance.ShowInterstitialTimer(null); 
            //ctrl.ga.SendEvent("Button Click", "New From MCPE", "");
        }

		public void ClickGalleryButton()
		{
            SoundController1.Instance.PlayClickSound();
            base.scene.controller.newCharacter.CloseNewCharacterView();
			CapeController.Instance.maincharacter.GetComponent<CharacterMain>().LoadSkinGallery();
            SuperStarAd.Instance.ShowInterstitialTimer(null);
            //ctrl.ga.SendEvent("Button Click", "New From Gallery", "");
        }

		public void ClickSearchOnlineButton()
		{
            SoundController1.Instance.PlayClickSound();
            base.scene.controller.newCharacter.CloseNewCharacterView();
			onlineNameCanvas.gameObject.SetActive(value: true);
            SuperStarAd.Instance.ShowInterstitialTimer(null);
            //	ctrl.ga.SendEvent("Button Click", "New From Online", "");
        }

		public void ClickSearchOnlineConfirm()
		{
			TMP_InputField componentInChildren = onlineNameCanvas.GetComponentInChildren<TMP_InputField>();
			base.scene.view.waitingCanvas.ShowLoadingSkinOnlineWaiting();
			CapeController.Instance.maincharacter.GetComponent<CharacterMain>().LoadSkinOnine(componentInChildren.text);
            SoundController1.Instance.PlayClickSound();
            onlineNameCanvas.gameObject.SetActive(value: false);
            SuperStarAd.Instance.ShowInterstitialTimer(null); 
        }

		public void ClickSearchOnlineCancel()
		{
            SoundController1.Instance.PlayClickSound();
            onlineNameCanvas.gameObject.SetActive(value: false);
		}

		public void ClickRandomOnlineButton()
		{
			base.scene.view.waitingCanvas.ShowLoadingSkinOnlineWaiting();
			CapeController.Instance.maincharacter.GetComponent<CharacterMain>().LoadRandomSkinOnine();
            //character.LoadRandomSkinOnine();
            SoundController1.Instance.PlayClickSound();
            base.scene.controller.newCharacter.CloseNewCharacterView();
            SuperStarAd.Instance.ShowInterstitialTimer(null);
            //ctrl.ga.SendEvent("Button Click", "New From Random", "");
        }

		public void ClickButtonCancel()
		{
            SoundController1.Instance.PlayClickSound();
            base.scene.controller.newCharacter.CloseNewCharacterView();
		}
		
	}
}
