using I2.Loc;
using Main.Controller;
using SuperStarSdk;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
	public class SaveCharacterCanvas1 : SceneElement1
	{
		private Cape1 character;

		private RootController1 ctrl;

		public InfoCanvas1 infoCanvas;

		public TextMeshProUGUI tileText;
		public GameObject mainAvatarPreviewObj;

		public GameObject Cape2217;
		public GameObject Cape6432;
		public GameObject Cape512256;
		public GameObject Elytra6432;
		public GameObject CharacterSkin;

		public GameObject MainPanel;

		private void Awake()
		{
			ctrl = base.scene.controller;
		}

		private void Start()
		{
			character = base.scene.view.cape;
			
		}

		private void OnEnable()
        {
			SetupPopup();
		}

		void SetupPopup()
        {
            if (mainAvatarPreviewObj.gameObject.activeSelf)
            {
				tileText.text = "Export Character";
				string s = LocalizationManager.GetTranslation(tileText.text);
				tileText.text = s;
				MainPanel.GetComponent<AspectRatioFitter>().aspectRatio = 1.4f;
				Cape2217.SetActive(false);
				Cape6432.SetActive(false);
				Cape512256.SetActive(false);
				CharacterSkin.SetActive(true);
				Elytra6432.SetActive(false);
			}
            else
            {
				tileText.text = "Export Cape";
                string s = LocalizationManager.GetTranslation(tileText.text);
                tileText.text = s;
                //Cape2217.SetActive(true);
                //Cape6432.SetActive(true);
                //Cape512256.SetActive(true);
                CharacterSkin.SetActive(false);

				if (CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C2217 || CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C6432)
				{
                    MainPanel.GetComponent<AspectRatioFitter>().aspectRatio = 1.1f;
                    Cape2217.SetActive(true);
					Cape6432.SetActive(true);
					Cape512256.SetActive(false);
					Elytra6432.SetActive(false);
				}
				else if (CapeController.Instance.currentcap.CurrentResolution == CapeResolution.C512256)
				{
                    MainPanel.GetComponent<AspectRatioFitter>().aspectRatio = 1.4f;
                    Cape2217.SetActive(false);
					Cape6432.SetActive(false);
					Cape512256.SetActive(true);
					Elytra6432.SetActive(false);

				}
                else if (CapeController.Instance.currentcap.CurrentResolution == CapeResolution.Elytra6432)
                {
                    MainPanel.GetComponent<AspectRatioFitter>().aspectRatio = 1.4f;
                    Cape2217.SetActive(false);
					Cape6432.SetActive(false);
					Cape512256.SetActive(false);
					Elytra6432.SetActive(true);
				}
				else {
                    MainPanel.GetComponent<AspectRatioFitter>().aspectRatio = 1.1f;
                    Cape2217.SetActive(true);
					Cape6432.SetActive(true);
					Cape512256.SetActive(false);
					Elytra6432.SetActive(false);

				}
			}
        }

		public void ClickMCPEButton()
		{
            SoundController1.Instance.PlayClickSound();
            Hide();
			//ctrl.ga.SendEvent("Button Click", "Save to MCPE", "");
				SuperStarAd.Instance.ShowForceInterstitial(CharacterSaveMCPE);
#if UNITY_ANDROID
			CharacterSaveGallery(0);
#elif UNITY_IOS
       CharacterSaveGallery(0);
#endif
			
		}

		private void CharacterSaveMCPE(bool result)
		{
			SuperStarAd.Instance.ShowForceInterstitial((result) => {

				if (result)
				{
					character.SaveMCPE();
				}
				else
				{
					ToastManager.Instance.ShowToast("Ad is not Loaded");
				}
			});
		}

		public void ClickGalleryButton(int index)
		{
            SoundController1.Instance.PlayClickSound();
            Hide();
			//ctrl.ga.SendEvent("Button Click", "Save to Gallery", "");

			if (!mainAvatarPreviewObj.gameObject.activeSelf)//Export cape.......
			{
				Debug.Log("Export 1");
					SuperStarAd.Instance.ShowForceInterstitial((o) => {
						if (o)
						{
							Debug.LogError("Export 1111");
							CharacterSaveGallery(index);
						}
						else
						{
							Debug.LogError("Export 000");
							ToastManager.Instance.ShowToast("Ad is not Loaded");
						}
                    });
            }
            else//Export Character.......
            {
				Debug.LogError("Save Main character skin");
					SuperStarAd.Instance.ShowInterstitialTimer((o)=> {
						MainCharacterSaveGallery();
					});
					
			}
		}

		private void CharacterSaveGallery(int index)
		{
            Debug.LogError("CharacterSaveGallery => " + index);
            character.SaveGallery(index);
		}

		private void MainCharacterSaveGallery()
        {
			mainAvatarPreviewObj.GetComponent<CharacterMain>().SaveGallery();
		}

		public void ClickEndlessHillsButton()
		{
            SoundController1.Instance.PlayClickSound();
            Hide();
			//ctrl.ga.SendEvent("Button Click", "Save to Endless Hills", "");
			//AndroidNativeUtility.ExternalStoragePathLoaded += CheckEndlessHillsFolderExist;
			//SA_Singleton<AndroidNativeUtility>.Instance.GetExternalStoragePath();
		}

		public void ClickDiamondMasterButton()
		{
            SoundController1.Instance.PlayClickSound();
            Hide();
			//	ctrl.ga.SendEvent("Button Click", "Save to Diamond Master", "");
			//AndroidNativeUtility.ExternalStoragePathLoaded += CheckDiamondMasterFolderExist;
			//SA_Singleton<AndroidNativeUtility>.Instance.GetExternalStoragePath();
		}

		public void ClickCloseButton()
		{
            SoundController1.Instance.PlayClickSound();
            Hide();
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}