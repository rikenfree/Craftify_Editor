using UnityEngine;
using SuperStarSdk;
using System;

namespace Main.View
{
	public class MenuCanvas : SceneElement
	{
        public GameObject tutorialPanel;
        public GameObject howToUseSkinPanel;

        public void ClickButtonRateThis()
		{
			base.scene.controller.sound.PlayClickSound();
			base.gameObject.SetActive(value: false);

			SuperStarSdkManager.Instance.Rate();
		}

		public void ClickButtonAbout()
		{
			base.scene.controller.sound.PlayClickSound();
			base.gameObject.SetActive(value: false);
			base.scene.view.infoCanvas.ShowAboutUsInfo();
		}

		
		public void ClickButtonPolicy()
		{
			base.scene.controller.sound.PlayClickSound();
			base.gameObject.SetActive(value: false);

			Application.OpenURL(SuperStarSdkManager.Instance.crossPromoAssetsRoot.PrivacyPolicy);
		}

		public void ClickButtonCancel()
		{
			base.scene.controller.sound.PlayClickSound();
			base.gameObject.SetActive(value: false);
		}

        public void ClickButtonHowToUse()
        {
            base.scene.controller.sound.PlayClickSound();
            //base.gameObject.SetActive(value: false);
            howToUseSkinPanel.SetActive(true);
        }

        public void ClickButtonTutorial()
        {
            base.scene.controller.sound.PlayClickSound();
            //base.gameObject.SetActive(value: false);
            tutorialPanel.SetActive(true);
        }

		public void ClickButtonShare()
		{
			base.scene.controller.sound.PlayClickSound();
			base.gameObject.SetActive(value: false);
			SuperStarSdkManager.Instance.Share();
		}
	}
}
