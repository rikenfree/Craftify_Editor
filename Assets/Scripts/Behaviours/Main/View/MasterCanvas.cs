using Main.Controller;
using UnityEngine;
using UnityEngine.UI;
using SuperStarSdk;
using UnityToolbag;
using System.Collections;

namespace Main.View
{
    public class MasterCanvas : SceneElement
    {
        public RectTransform welcomePanel;

        public RectTransform gamesdeedeePanel;

        public CircleButton buttonNew;

        public CircleButton buttonSave;

        public CircleButton buttonBody;

        public CircleButton buttonClothing;

        public CircleButton buttonPencil;

        public CircleButton buttonBucket;

        public CircleButton buttonColor;

        public CircleButton buttonDropper;

        public CircleButton buttonUndo;

        //public CircleButton buttonGamesDeeDee;

        public CircleButton buttonEraser;

        public CircleButton buttonRotate;

        public CircleButton buttonZoomIn;

        public CircleButton buttonZoomOut;

        public CircleButton buttonRedo;

        public CircleButton buttonTriggeredAd;

        private CircleButton[] toggleGroup;

        private RootController ctrl;

        public GameObject internetconnectionPopup;
        public GameObject gridButton;

        public GameObject tutorialPanel;
        public GameObject howToUseSkinPanel;
        public GameObject selectThemePanel;
        public GameObject selectLanguagePopup;
        public GameObject HowToSetSkin;

        public GameObject SucssesPopUp;
        public GameObject SucssesPopUpMCPE;

        public RectTransform dropdownContainer;
        public float dropdownAnimationDuration = 0.3f;
        private bool isDropdownOpen = true;
        private Coroutine dropdownAnimationCoroutine;

        public Image DownImage;
        public Image UpImage;

        private void Awake()
        {
            ctrl = base.scene.controller;
            InitButtonGroup();
        }

        private void Start()
        {
            InitButtonGroup();
            ClickButtonPencilDelegate();
            InvokeRepeating("CheckInterNetConnection", 5, 5);
        }

        public void InitButtonGroup()
        {
            toggleGroup = new CircleButton[5] {
                buttonPencil,
                buttonBucket,
                buttonDropper,
                buttonEraser,
                buttonRotate
            };
            foreach (var btn in toggleGroup)
            {
                if (btn == null)
                    Debug.LogError("A button in toggleGroup is not assigned in the Inspector!");
            }
        }

        public void UpdateButtonColor(Color color)
        {
            buttonColor.GetComponent<Image>().color = color;
        }

        public void OnClickSelectLanguageButton()
        {
            base.scene.controller.sound.PlayClickSound();
            selectLanguagePopup.SetActive(true);
        }
        public void SucssesFullOkButtonClick()
        {
            base.scene.controller.sound.PlayClickSound();
            SucssesPopUp.SetActive(false);
        }
        public void SucssesMCPEFullOkButtonClick()
        {
            base.scene.controller.sound.PlayClickSound();
            SucssesPopUpMCPE.SetActive(false);
        }

        public void OnCloseTutorialPanel()
        {
            base.scene.controller.sound.PlayClickSound();
            tutorialPanel.SetActive(false);
        }

        public void OnCloseHowToSetSkin()
        {
            base.scene.controller.sound.PlayClickSound();
            HowToSetSkin.SetActive(false);
        }

        public void OnCloseLanguagePanel()
        {
            base.scene.controller.sound.PlayClickSound();
            selectLanguagePopup.SetActive(false);
        }

        private void DeselectAll()
        {
            CircleButton[] array = toggleGroup;
            for (int i = 0; i < array.Length; i++)
            {
                // array[i].MarkDeselected();
            }
        }

        private void DisableGroupControllers()
        {
            //ctrl.camera.gameObject.SetActive(value: false);
            ctrl.pencil.gameObject.SetActive(value: false);
            ctrl.bucket.gameObject.SetActive(value: false);
            ctrl.eraser.gameObject.SetActive(value: false);
            ctrl.dropper.gameObject.SetActive(value: false);

            buttonPencil.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            buttonBucket.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            buttonDropper.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            buttonEraser.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        public void OnClickTutorialButton()
        {
            tutorialPanel.SetActive(true);

        }
        public void OnClickNoAdsButton()
        {

            IAPManager.Instance.BuyNonConsumable();
        }
        public void OnClickSelectThemeButton()
        {
            base.scene.controller.sound.PlayClickSound();
            selectThemePanel.SetActive(true);
        }

        public void ClickButtonMenu()
        {
            SuperStarAd.Instance.ShowInterstitialTimer(ClickButtonMenuDelegate);
        }

        public void ClickButtonMenuDelegate(bool isloaded)
        {
            Debug.Log(isloaded ? "Ad Worked" : "Ad not Worked");

            base.scene.view.menuCanvas.gameObject.SetActive(true);
            ctrl.sound.PlayClickSound();
        }



        public void ClickButtonNew()
        {
            SuperStarAd.Instance.ShowInterstitialTimer(ClickButtonNewDelegate);
            //ctrl.sound.PlayClickSound();
            //ctrl.newCharacter.OpenNewCharacterView();
        }

        public void ClickButtonNewDelegate(bool isloaded)
        {
            ctrl.sound.PlayClickSound();
            ctrl.newCharacter.OpenNewCharacterView();
            //ctrl.ga.SendEvent("Button Click", "New", "");
        }

        public void ClickButtonSave()
        {
            SuperStarAd.Instance.ShowForceInterstitial(ClickButtonSaveDelegate);
        }

        public void ClickButtonSaveDelegate(bool isloaded)
        {

            if (isloaded)
            {
                Debug.Log("Ad Worked");
            }
            else
            {
                Debug.Log("Ad not Worked");
            }
            ctrl.sound.PlayClickSound();
            base.scene.view.saveCharacter.gameObject.SetActive(value: true);

            //ctrl.ga.SendEvent("Button Click", "Save", "");
        }

        public void ClickButtonBody()
        {
            // SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonBodyDelegate);
            ClickButtonBodyDelegate();

        }

        public void ClickButtonBodyDelegate()
        {
            ctrl.sound.PlayClickSound();
            ctrl.bodyParts.OpenBodyPartsViewer();
            //	ctrl.ga.SendEvent("Button Click", "Body", "");
        }

        public void ClickButtonClothing()
        {
            //SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonClothingDelegate);
            ClickButtonClothingDelegate();
            if (ctrl.clothing.clothingOn)
            {
                gridButton.transform.GetChild(1).gameObject.SetActive(true);
                DisableGroupControllers();
            }
            else if (!ctrl.clothing.clothingOn)
            {
                gridButton.transform.GetChild(1).gameObject.SetActive(false);
                DisableGroupControllers();
                ctrl.pencil.gameObject.SetActive(value: true);
                buttonPencil.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        public void ClickButtonClothingDelegate()
        {
            ctrl.sound.PlayClickSound();
            ctrl.clothing.ToggleClothing();
            //ctrl.ga.SendEvent("Button Click", "Cloth", "");
        }

        public void ClickButtonGridDelegate()
        {
            ctrl.sound.PlayClickSound();
            ctrl.clothing.ToggleClothing();
            //ctrl.ga.SendEvent("Button Click", "Cloth", "");
        }

        public void ClickButtonPencil()
        {
            // SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonPencilDelegate);
            ClickButtonPencilDelegate();

        }

        public void ClickButtonPencilDelegate()
        {
            ctrl.sound.PlayClickSound();
            DeselectAll();
            // buttonPencil.MarkSelected();
            DisableGroupControllers();
            ctrl.pencil.gameObject.SetActive(value: true);
            buttonPencil.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //ctrl.ga.SendEvent("Button Click", "Pencil", "");
        }

        public void ClickButtonBucket()
        {
            //  SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonBucketDelegate);
            ClickButtonBucketDelegate();

        }

        public void ClickButtonBucketDelegate()
        {
            ctrl.sound.PlayClickSound();
            DeselectAll();
            // buttonBucket.MarkSelected();
            DisableGroupControllers();
            ctrl.bucket.gameObject.SetActive(value: true);
            buttonBucket.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //ctrl.ga.SendEvent("Button Click", "Bucket", "");
        }

        public void ClickButtonColor()
        {
            //  SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonColorDelegate);
            ClickButtonColorDelegate();

        }

        public void ClickButtonColorDelegate()
        {
            ctrl.sound.PlayClickSound();
            base.scene.view.colorOptionCanvas.gameObject.SetActive(value: true);
            //ctrl.ga.SendEvent("Button Click", "Color", "");
        }

        public void ClickButtonDropper()
        {
            // SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonDropperDelegate);
            ClickButtonDropperDelegate();

        }

        public void ClickButtonDropperDelegate()
        {
            ctrl.sound.PlayClickSound();
            DeselectAll();
            // buttonDropper.MarkSelected();
            DisableGroupControllers();
            ctrl.dropper.gameObject.SetActive(value: true);
            buttonDropper.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //ctrl.ga.SendEvent("Button Click", "Dropper", "");
        }

        public void ClickButtonEraser()
        {
            //SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonEraserDelegate);
            ClickButtonEraserDelegate();

        }

        public void ClickButtonEraserDelegate()
        {
            ctrl.sound.PlayClickSound();
            DeselectAll();
            // buttonEraser.MarkSelected();
            DisableGroupControllers();
            ctrl.eraser.gameObject.SetActive(value: true);
            buttonEraser.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //ctrl.ga.SendEvent("Button Click", "Eraser", "");
        }

        public void ClickButtonRotate()
        {
            //SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonRotateDelegate);
            ClickButtonRotateDelegate();

        }

        public void ClickButtonRotateDelegate()
        {
            ctrl.sound.PlayClickSound();
            DeselectAll();
            // buttonRotate.MarkSelected();
            DisableGroupControllers();
            ctrl.camera.gameObject.SetActive(value: true);
            //ctrl.ga.SendEvent("Button Click", "Rotate", "");
        }

        public void ClickButtonZoomIn()
        {
            ctrl.sound.PlayClickSound();
            ctrl.camera.ZoomIn();
            //ctrl.ga.SendEvent("Button Click", "Zoom In", "");
        }

        public void ClickButtonZoomOut()
        {
            ctrl.sound.PlayClickSound();
            ctrl.camera.ZoomOut();
            //ctrl.ga.SendEvent("Button Click", "Zoom Out", "");
        }

        public void ClickButtonUndo()
        {
            ctrl.sound.PlayClickSound();
            ctrl.undoRedo.Undo();
            //ctrl.ga.SendEvent("Button Click", "Undo", "");
        }

        public void ClickButtonRedo()
        {
            ctrl.sound.PlayClickSound();
            ctrl.undoRedo.Redo();
            //	ctrl.ga.SendEvent("Button Click", "Redo", "");
        }

        public void ClickButtonTriggeredAd()
        {
            ctrl.sound.PlayClickSound();
            //SuperStarAd.Instance.adCount = 0;
            SuperStarAd.Instance.ShowForceInterstitial(null);
            //ctrl.ga.SendEvent("Button Click", "Triggered Ad", "");
        }
        public SSBannerCollection GetSSBannerCollection;
        //      public void ClickButtonGamesDeeDee()
        //{
        //	ctrl.sound.PlayClickSound();
        //          if (GetSSBannerCollection.isDataLaoded)
        //          {
        //              GetSSBannerCollection.BannerParent.gameObject.SetActive(true);
        //		GetSSBannerCollection.CommingSoonParent.gameObject.SetActive(false);

        //	}
        //	else
        //          {
        //		GetSSBannerCollection.BannerParent.gameObject.SetActive(false);
        //		GetSSBannerCollection.CommingSoonParent.gameObject.SetActive(true);
        //		Debug.Log("data is not loded");
        //          }
        //          //	ctrl.ga.SendEvent("Button Click", "Games Dee Dee", "");
        //      }


        public void ClickButtonCloseGamesDeeDee()
        {
            ctrl.sound.PlayClickSound();
            gamesdeedeePanel.gameObject.SetActive(value: false);
        }


        public void MoreAppsPressed()
        {

            SuperStarSdkManager.Instance.MoreApps();
        }


        private void InitFirstMode()
        {
            DeselectAll();
            // buttonRotate.MarkSelected();
            DisableGroupControllers();
            ctrl.camera.gameObject.SetActive(value: true);
        }

        public void CheckInterNetConnection()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                internetconnectionPopup.SetActive(true);
            }
        }

        public void ClickSubscribeButton()
        {
            base.scene.controller.sound.PlayClickSound();
            //Application.OpenURL(SuperStarSdkManager.Instance.crossPromoAssetsRoot.subscribeUrl);
        }

        public void ClickThemeCancelButton()
        {
            base.scene.controller.sound.PlayClickSound();
            selectThemePanel.SetActive(false);
        }

        public bool isEditorShowing = true;

        public void ToggleDropdown()
        {
            if (dropdownAnimationCoroutine != null)
            {
                StopCoroutine(dropdownAnimationCoroutine);
            }
            dropdownAnimationCoroutine = StartCoroutine(AnimateDropdown(!isDropdownOpen));
        }

        private IEnumerator AnimateDropdown(bool open)
        {
            isDropdownOpen = open;
            float startScale = open ? 0f : 1f;
            float endScale = open ? 1f : 0f;
            CanvasGroup canvasGroup = dropdownContainer.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = dropdownContainer.gameObject.AddComponent<CanvasGroup>();
            }

            if (!open)
            {
                StartCoroutine(FadeImage(UpImage, false, 0.2f));
                StartCoroutine(FadeImage(DownImage, true, 0.2f));
            }
            else
            {
                dropdownContainer.gameObject.SetActive(true);
                StartCoroutine(FadeImage(DownImage, false, 0.2f));
                StartCoroutine(FadeImage(UpImage, true, 0.2f));
            }

            // Set the pivot to top center
            //dropdownContainer.pivot = new Vector2(0.5f, 1f);

            float timer = 0f;
            while (timer < dropdownAnimationDuration)
            {
                timer += Time.deltaTime;
                float progress = timer / dropdownAnimationDuration;
                float currentScale = Mathf.Lerp(startScale, endScale, progress);
                dropdownContainer.localScale = new Vector3(currentScale, currentScale, currentScale);
                canvasGroup.alpha = Mathf.Lerp(startScale, endScale, progress);
                yield return null;
            }

            dropdownContainer.localScale = new Vector3(endScale, endScale, endScale);
            canvasGroup.alpha = endScale;

            if (!open)
            {
                dropdownContainer.gameObject.SetActive(false);
            }
        }

        private IEnumerator FadeImage(Image img, bool fadeIn, float duration)
        {
            float startAlpha = fadeIn ? 0f : 1f;
            float endAlpha = fadeIn ? 1f : 0f;
            Color color = img.color;
            float timer = 0f;
            img.gameObject.SetActive(true);

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;
                color.a = Mathf.Lerp(startAlpha, endAlpha, progress);
                img.color = color;
                yield return null;
            }
            color.a = endAlpha;
            img.color = color;
            if (!fadeIn)
                img.gameObject.SetActive(false);
        }
    }
}
