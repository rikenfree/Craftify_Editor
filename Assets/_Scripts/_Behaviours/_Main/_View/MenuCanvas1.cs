	using UnityEngine;
using SuperStarSdk;
using System;
using Main.Controller;

namespace Main.View
{
	public class MenuCanvas1 : SceneElement1
	{
        public GameObject tutorialPanel;
        public GameObject howToUseSkinPanel;

        public void ClickButtonRateThis()
		{
            SoundController1.Instance.PlayClickSound();
            base.gameObject.SetActive(value: false);

			SuperStarSdkManager.Instance.Rate();
		}

		public void ClickButtonAbout()
		{
            SoundController1.Instance.PlayClickSound();
            base.gameObject.SetActive(value: false);
			base.scene.view.infoCanvas.ShowAboutUsInfo();
		}

		public void ClickHowToUseSkin()
		{
            SoundController1.Instance.PlayClickSound();
            howToUseSkinPanel.SetActive(true);
        }
		
		public void ClickButtonPolicy()
		{
            SoundController1.Instance.PlayClickSound();
            base.gameObject.SetActive(value: false);

#if UNITY_ANDROID
          Application.OpenURL(SuperStarSdkManager.Instance.crossPromoAssetsRoot.PrivacyPolicy);
#elif UNITY_IPHONE
			Application.OpenURL(SuperStarSdkManager.Instance.crossPromoAssetsRoot.PrivacyPolicy);
#else
       Application.OpenURL(SuperStarSdkManager.Instance.crossPromoAssetsRoot.PrivacyPolicy);
#endif


		}

		public void ClickButtonCancel()
		{
            SoundController1.Instance.PlayClickSound();
            base.gameObject.SetActive(value: false);
		}
        public void ClickButtonHowToUse()
        {
            SoundController1.Instance.PlayClickSound();
            //Application.OpenURL(SuperStarSdkManager.Instance.crossPromoAssetsRoot.tutorial_url);

        }
        public void ClickButtonTutorial()
        {
            SoundController1.Instance.PlayClickSound();
            //base.gameObject.SetActive(value: false);
            tutorialPanel.SetActive(true);

        }
		public void ClickButtonShare()
		{
            SoundController1.Instance.PlayClickSound();
            base.gameObject.SetActive(value: false);
			SuperStarSdkManager.Instance.Share();

		}
		
	}
}
