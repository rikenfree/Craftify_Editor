using UnityEngine;
using System.Collections.Generic;
using Main.View;

public class EscapeHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> mainPanels;

    [SerializeField] private List<GameObject> settingPanels;
    [SerializeField] private GameObject wallpaperPanel;

    [SerializeField] private GameObject capviewCanvas;
    [SerializeField] private GameObject elytraviewCanvas;
    [SerializeField] private GameObject elytraParent;
    [SerializeField] private GameObject elytraParentBig;
    [SerializeField] private GameObject canvasBig;
    [SerializeField] private GameObject canvasCollections;
    [SerializeField] private GameObject saveToGallery;
    [SerializeField] private GameObject chanracterSplash;

    [SerializeField] private MasterCanvas1 masterCanvas1;
    [SerializeField] private WallPaperManeger wallPaperManager;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundControllerMain.instance.PlayClickSound();
            CloseMainPanels();
            Debug.LogWarning("Key is pressed");
        }
    }

    private void CloseMainPanels()
    {
        bool anyClosed = false;

        foreach (GameObject obj in mainPanels)
        {
            if (obj.activeSelf)
            {
                if (obj.CompareTag("CapSetting"))
                {
                    CloseSettingPanels(obj);
                }
                else if (obj.CompareTag("CapWallPaperSave"))
                {
                    obj.SetActive(false);
                    wallPaperManager.OnClickWallPaper();
                }
                else if (obj.CompareTag("CapWallPaper"))
                {
                    wallPaperManager.OnClickBackBtn();
                }
                else
                {
                    obj.SetActive(false);
                    Debug.LogWarning($"Closed panel: {obj.name}");
                }

                anyClosed = true;
                break; 
            }
        }

        if (elytraviewCanvas.activeSelf && saveToGallery.activeSelf)
        {
            saveToGallery.SetActive(false);
            return;
        }

        if (elytraviewCanvas.activeSelf && elytraParentBig.activeSelf && canvasBig.activeSelf)
        {
            elytraParentBig.SetActive(false);
            canvasBig.SetActive(false);
            elytraParent.SetActive(true);
            canvasCollections.SetActive(true);
            return;
        }

        if (elytraviewCanvas.activeSelf && elytraParent.activeSelf && canvasCollections.activeSelf)
        {
            masterCanvas1.OnClickBackFromCollectionView();
            return;
        }

        if (chanracterSplash.activeSelf)
        {
            masterCanvas1.ClickButtonBody();
            return;
        }

        if (!anyClosed)
        {
            Debug.LogWarning("No active panels left — Changing to scene 3");
            ManageScene.instance.ChangeToScene(3);
        }
    }

    private void CloseSettingPanels(GameObject panel)
    {
        bool anyActive = false;

        foreach (GameObject obj in settingPanels)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                Debug.LogWarning("Closed sub-setting panel");
                anyActive = true;
                break;
            }
        }

        if (!anyActive)
        {
            panel.SetActive(false);
            Debug.LogWarning("Closed main setting panel — no subpanels were active");
        }
    }
}
