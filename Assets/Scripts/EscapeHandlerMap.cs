using System.Collections.Generic;
using UnityEngine;

public class EscapeHandlerMap : MonoBehaviour
{
    [SerializeField] private List<GameObject> mainPanels;

    [SerializeField] private List<GameObject> settingPanels;

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
                if (obj.CompareTag("MapSetting"))
                {
                    CloseSettingPanels(obj);
                }
                else if (obj.CompareTag("MapWallPaperSave"))
                {
                    obj.SetActive(false);
                    wallpaperPanel.gameObject.SetActive(true);
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
                Debug.LogWarning("CloseSettingPanels");
                anyActive = true;
                break;
            }
        }

        if (!anyActive)
        {
            panel.SetActive(false);
            Debug.LogWarning("Panel closed because all were inactive");
        }
    }
}
