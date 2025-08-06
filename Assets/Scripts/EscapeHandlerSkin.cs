using System.Collections.Generic;
using UnityEngine;

public class EscapeHandlerSkin : MonoBehaviour
{
    [SerializeField] private List<GameObject> mainPanels;

    [SerializeField] private List<GameObject> settingPanels;
    [SerializeField] private List<GameObject> exportPanels;
    [SerializeField] private List<GameObject> newCharacterPanels;

    [SerializeField] private GameObject wallpaperPanel;

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
                if (obj.CompareTag("SkinSetting"))
                {
                    CloseSettingPanels(obj);
                }
                else if (obj.CompareTag("SkinExport"))
                {
                    CloseExportPanels(obj);
                }
                else if (obj.CompareTag("SkinCharacter"))
                {
                    CloseNewCharacterPanels(obj);
                }
                else if (obj.CompareTag("SkinWallPaperSave"))
                {
                    obj.SetActive(false);
                    wallpaperPanel.SetActive(true);
                }
                else
                {
                    obj.SetActive(false);
                    Debug.LogWarning($"Closed panel: {obj.name}");
                }

                anyClosed = true;
                break; // Only close one per key press
            }
        }

        if (!anyClosed)
        {
            Debug.Log("No Active Panel - Changing Scene");
            ManageScene.instance.ChangeToScene(0);
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
                Debug.LogWarning("Closed Setting Panel");
                anyActive = true;
                break;
            }
        }

        if (!anyActive)
        {
            panel.SetActive(false);
            Debug.LogWarning("Closed parent Setting Panel because all were inactive");
        }
    }

    private void CloseExportPanels(GameObject panel)
    {
        bool anyActive = false;

        foreach (GameObject obj in exportPanels)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                Debug.LogWarning("Closed Export Panel");
                anyActive = true;
                break;
            }
        }

        if (!anyActive)
        {
            panel.SetActive(false);
            Debug.LogWarning("Closed parent Export Panel because all were inactive");
        }
    }

    private void CloseNewCharacterPanels(GameObject panel)
    {
        bool anyActive = false;

        foreach (GameObject obj in newCharacterPanels)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                Debug.LogWarning("Closed Character Panel");
                anyActive = true;
                break;
            }
        }

        if (!anyActive)
        {
            panel.SetActive(false);
            Debug.LogWarning("Closed parent Character Panel because all were inactive");
        }
    }
}
