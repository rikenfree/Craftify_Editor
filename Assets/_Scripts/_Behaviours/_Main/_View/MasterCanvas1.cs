using Main.Controller;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SuperStarSdk;
using TMPro;
using I2.Loc;

namespace Main.View
{
    public class MasterCanvas1 : SceneElement1
    {
        public RectTransform welcomePanel;

        public RectTransform gamesdeedeePanel;

        public CircleButton1 buttonNew;

        public CircleButton1 buttonSave;

        public CircleButton1 buttonBody;

        public CircleButton1 buttonClothing;

        public CircleButton1 buttonPencil;

        public CircleButton1 buttonBucket;

        public CircleButton1 buttonColor;

        public CircleButton1 buttonDropper;

        public CircleButton1 buttonUndo;

        public CircleButton1 buttonGamesDeeDee;

        public CircleButton1 buttonEraser;

        public CircleButton1 buttonRotate;

        public CircleButton1 buttonZoomIn;

        public CircleButton1 buttonZoomOut;

        public CircleButton1 buttonRedo;

        public CircleButton1 buttonTriggeredAd;

        private CircleButton1[] toggleGroup;

        private RootController1 ctrl;

        public GameObject internetconnectionPopup;
        public GameObject GridPanel;

        public GameObject tutorialPanel;
        public GameObject howToUseSkinPanel;
        public GameObject selectThemePanel;

        public GameObject mainCharecter;
        public GameObject mainCap;
        public GameObject leftPanel;
        public GameObject rightPanel;
        public GameObject charecterCapskin;
        public GameObject mainCapskin;
        public GameObject button_new_for_cape;
        public GameObject button_new;
        public GameObject button_save;
        public GameObject button_cap;
        public GameObject button_theme;
        public GameObject button_BodyGrid;
        public GameObject selectLanguagePopup;
        public GameObject dropDownMenuButton;


        public TextMeshProUGUI HeaderText;
        public Localize HeaderLocalize;

        public GameObject[] EditorScreens;
        public GameObject[] CollectionScreens;

        public CapeEltraView capeEltraView;

        public RectTransform dropdownContainer;
        public float dropdownAnimationDuration = 0.3f;
        private bool isDropdownOpen = true;
        private Coroutine dropdownAnimationCoroutine;

        public Image DownImage;
        public Image UpImage;

        private void Awake()
        {
            ctrl = base.scene.controller;
        }

        private void Start()
        {
            InitButtonGroup();
            InitFirstMode();
            InvokeRepeating("CheckInterNetConnection", 5, 5);
        }
        public void InitButtonGroup()
        {
            toggleGroup = new CircleButton1[5];
            toggleGroup[0] = buttonPencil;
            toggleGroup[1] = buttonBucket;
            toggleGroup[2] = buttonDropper;
            toggleGroup[3] = buttonEraser;
            toggleGroup[4] = buttonRotate;
        }

        public void UpdateButtonColor(Color color)
        {
            buttonColor.GetComponent<Image>().color = color;
        }

        private void DeselectAll()
        {
            CircleButton1[] array = toggleGroup;
            for (int i = 0; i < array.Length; i++)
            {
                //array[i].MarkDeselected();
            }
        }

        private void DisableGroupControllers()
        {
            //	ctrl.camera.gameObject.SetActive(value: false);
            ctrl.pencil.gameObject.SetActive(value: false);
            ctrl.bucket.gameObject.SetActive(value: false);
            ctrl.eraser.gameObject.SetActive(value: false);
            ctrl.dropper.gameObject.SetActive(value: false);

            buttonPencil.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            buttonBucket.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            buttonDropper.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            buttonEraser.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

        public void OnCloseLanguagePanel()
        {
            SoundController1.Instance.PlayClickSound();
            selectLanguagePopup.SetActive(false);
        }
        public void OnClickSelectLanguageButton()
        {
            SoundController1.Instance.PlayClickSound();
            selectLanguagePopup.SetActive(true);
        }

        public void OnClickTutorialButton()
        {
            SoundController1.Instance.PlayClickSound();
            tutorialPanel.SetActive(true);
        }

        public void OnClickNoAdsButton()
        {
            SoundController1.Instance.PlayClickSound();
            IAPManager.Instance.BuyNonConsumable();
        }

        public void OnClickSelectThemeButton()
        {
            SoundController1.Instance.PlayClickSound();
            selectThemePanel.SetActive(true);
            SuperStarAd.Instance.ShowInterstitialTimer(null);
        }

        public void ClickButtonMenu()
        {
            SoundController1.Instance.PlayClickSound();
            SuperStarAd.Instance.ShowInterstitialTimer(ClickButtonMenuDelegate);
        }
        public void HowToSetPlayerClose()
        {
            SoundController1.Instance.PlayClickSound();
            howToUseSkinPanel.SetActive(false);
        }

        public GameObject SucessfullyPopup;
        public void CloseSucessfullyPopup()
        {
            SoundController1.Instance.PlayClickSound();
            SucessfullyPopup.SetActive(false);
        }

        public void ClickButtonMenuDelegate(bool result)
        {
            SoundController1.Instance.PlayClickSound();
            base.scene.view.menuCanvas.gameObject.SetActive(value: true);
            //	ctrl.ga.SendEvent("Button Click", "Menu", "");
        }

        public void ClickButtonNew()
        {
            SoundController1.Instance.PlayClickSound();
            SuperStarAd.Instance.ShowInterstitialTimer(ClickButtonNewDelegate);
        }

        public void SubscribeButtonClick()
        {
            SoundController1.Instance.PlayClickSound();

            //if (!string.IsNullOrEmpty(SuperStarSdkManager.Instance.crossPromoAssetsRoot.subscribeUrl))
            //{
            //    Application.OpenURL(SuperStarSdkManager.Instance.crossPromoAssetsRoot.subscribeUrl);
            //}
        }

        public void ClickButtonNewDelegate(bool result)
        {
            SoundController1.Instance.PlayClickSound();
            ctrl.newCharacter.OpenNewCharacterView();
            //ctrl.ga.SendEvent("Button Click", "New", "");
        }

        public void ClickButtonCapePick()
        {
            SoundController1.Instance.PlayClickSound();
            SuperStarAd.Instance.ShowInterstitialTimer(ClickButtonCapePickDelegate);
        }

        public void ClickButtonCapePickDelegate(bool result)
        {
            SoundController1.Instance.PlayClickSound();
            capePickFromGalleryPopup.SetActive(true);
        }

        public void ClickButtonSave()
        {
            SoundController1.Instance.PlayClickSound();
            SuperStarAd.Instance.ShowInterstitialTimer(ClickButtonSaveDelegate);
        }

        public void ClickButtonSaveDelegate(bool result)
        {
            SoundController1.Instance.PlayClickSound();
            base.scene.view.saveCharacter.gameObject.SetActive(value: true);
            //ctrl.ga.SendEvent("Button Click", "Save", "");
        }

        int cnt = 0;
        public void ClickButtonBody()
        {
            SoundController1.Instance.PlayClickSound();
            // SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonBodyDelegate);
            ClickButtonBodyDelegate();
            var leftPanelRect = leftPanel.GetComponent<RectTransform>();
            if (cnt == 0)
            {
                //buttonBody.MarkSelected();
                CapeController.Instance.currentcap.capeObject.SetActive(false);
                CapeController.Instance.currentcap.elytraObject.SetActive(false);
                dropDownMenuButton.SetActive(false);
                //leftPanelRect.anchoredPosition = new Vector2(-122f, leftPanelRect.anchoredPosition.y);
                cnt = 1;
                DisableGroupControllers();
            }
            else
            {
                //buttonBody.MarkDeselected();
                if (CapeController.Instance.currentcap.LastMode == 0 || CapeController.Instance.currentcap.LastMode == 1)
                {
                    CapeController.Instance.currentcap.capeObject.SetActive(true);
                    //CapeController.Instance.currentcap.elytraObject.SetActive(false);
                }
                else
                {
                    CapeController.Instance.currentcap.elytraObject.SetActive(true);
                    //CapeController.Instance.currentcap.capeObject.SetActive(false);
                }
                //leftPanelRect.anchoredPosition = new Vector2(-105f, leftPanelRect.anchoredPosition.y);
                dropDownMenuButton.SetActive(true);
                DisableGroupControllers();
                ctrl.pencil.gameObject.SetActive(value: true);
                buttonPencil.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                cnt = 0;
            }
            SuperStarAd.Instance.ShowInterstitialTimer(null);
        }

        public void ClickButtonBodyDelegate()
        {
            CapeController.Instance.currentcap.AutoSave();
            //SoundController1.Instance.PlayClickSound();
            mainCharecter.SetActive(!mainCharecter.activeSelf);
            //mainCap.SetActive(!mainCap.activeSelf);
            //leftPanel.SetActive(!leftPanel.activeSelf);
            rightPanel.SetActive(!rightPanel.activeSelf);
            button_new_for_cape.SetActive(!button_new_for_cape.activeSelf);
            button_new.SetActive(!button_new.activeSelf);
            button_cap.SetActive(!button_cap.activeSelf);
            button_BodyGrid.SetActive(!button_BodyGrid.activeSelf);
            //button_save.SetActive(!button_save.activeSelf);
            //charecterCapskin.GetComponent<Cape>().skin = Resources.Load<Texture2D>("Skins/disk_tmp");
            //charecterCapskin.GetComponent<Cape>().tryAutoLoad = false;
            //ClickButtonRotate();
        }

        public void ClickButtonClothing()
        {
            //SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonClothingDelegate);
            SoundController1.Instance.PlayClickSound();
            ClickButtonClothingDelegate();
        }

        public void ClickButtonGridPanel()
        {
            SoundController1.Instance.PlayClickSound();
        }

        public void ClickButtonClothingDelegate()
        {
            SoundController1.Instance.PlayClickSound();
            ctrl.clothing.ToggleClothing();
            //ctrl.ga.SendEvent("Button Click", "Cloth", "");
        }

        public void ClickButtonGridDelegate()
        {
            SoundController1.Instance.PlayClickSound();
            ctrl.clothing.ToggleClothing();
            //ctrl.ga.SendEvent("Button Click", "Cloth", "");
        }

        public void ClickButtonPencil()
        {
            // SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonPencilDelegate);
            SoundController1.Instance.PlayClickSound();
            ClickButtonPencilDelegate();
        }

        public void ClickButtonPencilDelegate()
        {
            DeselectAll();
            //buttonPencil.MarkSelected();
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
            SoundController1.Instance.PlayClickSound();
            DeselectAll();
            //buttonBucket.MarkSelected();
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
            SoundController1.Instance.PlayClickSound();
            base.scene.view.colorOptionCanvas.gameObject.SetActive(value: true);
            //ctrl.ga.SendEvent("Button Click", "Color", "");
        }

        public void ClickButtonDropper()
        {
            // SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonDropperDelegate);
            SoundController1.Instance.PlayClickSound();
            ClickButtonDropperDelegate();
        }

        public void ClickButtonDropperDelegate()
        {
            DeselectAll();
            //buttonDropper.MarkSelected();
            DisableGroupControllers();
            ctrl.dropper.gameObject.SetActive(value: true);
            buttonDropper.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //ctrl.ga.SendEvent("Button Click", "Dropper", "");
        }

        public void ClickButtonEraser()
        {
            //SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonEraserDelegate);
            SoundController1.Instance.PlayClickSound();
            ClickButtonEraserDelegate();
        }

        public void ClickButtonEraserDelegate()
        {
            DeselectAll();
            //buttonEraser.MarkSelected();
            DisableGroupControllers();
            ctrl.eraser.gameObject.SetActive(value: true);
            buttonEraser.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //ctrl.ga.SendEvent("Button Click", "Eraser", "");
        }

        public void ClickButtonRotate()
        {
            //SuperStarAd.Instance.ShowInterstitialIfReady(ClickButtonRotateDelegate);
            SoundController1.Instance.PlayClickSound();
            ClickButtonRotateDelegate();
        }

        public void ClickButtonRotateDelegate()
        {
            DeselectAll();
            //buttonRotate.MarkSelected();
            DisableGroupControllers();
            //ctrl.camera.gameObject.SetActive(value: true);
            //ctrl.ga.SendEvent("Button Click", "Rotate", "");
        }

        public void ClickButtonZoomIn()
        {
            SoundController1.Instance.PlayClickSound();
            ctrl.camera.ZoomIn();
            //ctrl.ga.SendEvent("Button Click", "Zoom In", "");
        }

        public void ClickButtonZoomOut()
        {
            SoundController1.Instance.PlayClickSound();
            ctrl.camera.ZoomOut();
            //ctrl.ga.SendEvent("Button Click", "Zoom Out", "");
        }

        public void ClickButtonUndo()
        {
            SoundController1.Instance.PlayClickSound();
            ctrl.undoRedo.Undo();
            //ctrl.ga.SendEvent("Button Click", "Undo", "");
        }

        public void ClickButtonRedo()
        {
            SoundController1.Instance.PlayClickSound();
            ctrl.undoRedo.Redo();
            //	ctrl.ga.SendEvent("Button Click", "Redo", "");
        }

        public void CloseThemePopUp()
        {
            SoundController1.Instance.PlayClickSound();
            selectThemePanel.SetActive(false);
        }

        public void CloseTutorial()
        {
            SoundController1.Instance.PlayClickSound();
            tutorialPanel.SetActive(false);
        }

        public void ClickButtonTriggeredAd()
        {
            SoundController1.Instance.PlayClickSound();
            //SuperStarAd.Instance.adCount = 0;
            SuperStarAd.Instance.ShowForceInterstitial((result) =>
            {

                if (result)
                {
                    ToastManager.Instance.ShowToast("Thank You For Your Support");
                }
                else
                {
                    ToastManager.Instance.ShowToast("Ad is not Loaded");
                }
            });
            //ctrl.ga.SendEvent("Button Click", "Triggered Ad", "");
        }

        //public SSBannerCollection GetSSBannerCollection;
        public void ClickButtonGamesDeeDee()
        {
            SoundController1.Instance.PlayClickSound();
            SuperStarSdkManager.Instance.MoreApps();

        }

        public void ClickButtonCloseGamesDeeDee()
        {
            SoundController1.Instance.PlayClickSound();
            gamesdeedeePanel.gameObject.SetActive(value: false);
        }

        public void OnClickCapSelectionButton()
        {
            SoundController1.Instance.PlayClickSound();
            CapeController.Instance.pageNum = 1;
            CapeController.Instance.onlineCapCatalogue.SetActive(true);
            CapeController.Instance.onlineCapCatalogue.GetComponent<CapeCatalogue>().OnStartSetUp();
            //CapeController.Instance.ChangeCapTexture();
        }



        private void InitFirstMode()
        {
            DeselectAll();
            //buttonPencil.MarkSelected();
            DisableGroupControllers();
            ctrl.pencil.gameObject.SetActive(value: true);
            buttonPencil.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        public void GridsOnOff(CircleButton1 CB)
        {
            SoundController1.Instance.PlayClickSound();

            if (CB.isMarked)
            {
                CapeController.Instance.currentcap.OnClickGridOnOff(false);
                CB.MarkDeselected();
                button_BodyGrid.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                CapeController.Instance.currentcap.OnClickGridOnOff(true);
                CB.MarkSelected();
                button_BodyGrid.transform.GetChild(1).gameObject.SetActive(true);
            }
            SuperStarAd.Instance.ShowInterstitialTimer(null);
        }

        public void CheckInterNetConnection()
        {
            //SoundController.Instance.PlayClickSound();

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                internetconnectionPopup.SetActive(true);
            }
        }

        #region Cape Selection From Gallery.......
        public GameObject capePickFromGalleryPopup;
        public GameObject capePickFromGalleryNotSupported;

        public void OnclickCapePickFromGalleryButton(int index)
        {
            if (index == 0)
            {
                SoundController1.Instance.PlayClickSound();
                capePickFromGalleryPopup.SetActive(false);
            }
            else
            {
                SoundController1.Instance.PlayClickSound();
                capePickFromGalleryNotSupported.SetActive(false);
            }

            NativeGallery.GetImageFromGallery((path) =>
            {
                Debug.Log("Image path: " + path);
                if (!string.IsNullOrEmpty(path))
                {
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, 64, false);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    // Supported cape sizes
                    if ((texture.width == 22 && texture.height == 17) ||
                        (texture.width == 64 && texture.height == 32) ||
                        (texture.width == 512 && texture.height == 256))
                    {
                        Debug.Log("Cape texture format supported");
                        CapeController.Instance.ChangePickCapeTexture(texture);
                    }
                    else
                    {
                        Debug.Log("Cape texture format not supported");
                        capePickFromGalleryNotSupported.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogError("Path is null or empty");
                }
            }, "Select a PNG image", "image/png");
        }



        public void OnclickElytraPickFromGalleryButton()
        {
            SoundController1.Instance.PlayClickSound();
            capePickFromGalleryPopup.SetActive(false);
            capePickFromGalleryNotSupported.SetActive(false);

            NativeGallery.GetImageFromGallery((path) =>
            {
                Debug.Log("Image path: " + path);
                if (!string.IsNullOrEmpty(path))
                {
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, 64, false);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    // Check if it's a valid elytra texture size
                    if (texture.width == 64 && texture.height == 32)
                    {
                        Debug.Log("Elytra texture format supported");
                        CapeController.Instance.ChangePickElytraTexture(texture);
                    }
                    else
                    {
                        Debug.Log("Elytra texture format not supported");
                        capePickFromGalleryNotSupported.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogError("Path is null or empty");
                }
            }, "Select a PNG image", "image/png");
        }


        public void OnclickCapePickFromGalleryCancelButton()
        {
            capePickFromGalleryPopup.SetActive(false);
        }

        public void OnClickCapePickFromGalleryNotSupportedCancelButton()
        {
            SoundController1.Instance.PlayClickSound();
            capePickFromGalleryNotSupported.SetActive(false);
        }


        public void CreateNewCape2217()
        {
            SoundController1.Instance.PlayClickSound();
            SuperStarAd.Instance.ShowForceInterstitial((o) =>
            {

                CapeController.Instance.currentcap.NewSetup(CapeResolution.C2217);
                OnclickCapePickFromGalleryCancelButton();

            });
        }

        public void CreateNewCape6432()
        {
            SoundController1.Instance.PlayClickSound();
            SuperStarAd.Instance.ShowForceInterstitial((o) =>
            {
                CapeController.Instance.currentcap.NewSetup(CapeResolution.C6432);
                OnclickCapePickFromGalleryCancelButton();
            });
        }
        public void CreateNewCape512256()
        {
            SoundController1.Instance.PlayClickSound();
            SuperStarAd.Instance.ShowForceInterstitial((o) =>
            {
                CapeController.Instance.currentcap.NewSetup(CapeResolution.C512256);
                OnclickCapePickFromGalleryCancelButton();
            });


        }

        public void CreateNewElytra6432()
        {
            SoundController1.Instance.PlayClickSound();
            SuperStarAd.Instance.ShowForceInterstitial((o) =>
            {
                CapeController.Instance.currentcap.NewSetup(CapeResolution.Elytra6432);
                OnclickCapePickFromGalleryCancelButton();
            });

        }

        public void OnClickCollectionView()
        {
            isEditorShowing = false;

            for (int i = 0; i < EditorScreens.Length; i++)
            {
                EditorScreens[i].SetActive(false);

            }
            for (int i = 0; i < CollectionScreens.Length; i++)
            {
                CollectionScreens[i].SetActive(true);

                string s = LocalizationManager.GetTranslation("Page");
                capeEltraView.PageTxt.text = s + " " + (capeEltraView.currentPageIndex + 1);

            }
            SoundController1.Instance.PlayClickSound();
            SuperStarAd.Instance.ShowInterstitialTimer(null);
        }

        public void OnClickBackFromCollectionView()
        {
            Debug.Log("OnClickBackFromCollectionView");
            SoundController1.Instance.PlayClickSound();
            isEditorShowing = true;
            for (int i = 0; i < EditorScreens.Length; i++)
            {
                EditorScreens[i].SetActive(true);

            }
            for (int i = 0; i < CollectionScreens.Length; i++)
            {
                CollectionScreens[i].SetActive(false);

            }

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

        #endregion
    }


}