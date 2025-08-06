using UnityEngine;

public class EscapeHandlerMain : MonoBehaviour
{
    public GameObject exitPopup;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundControllerMain.instance.PlayClickSound();
            exitPopup.SetActive(true);
        }
    }
    public void OnClickexitYes()
    {
        SoundControllerMain.instance.PlayClickSound();
        Application.Quit();
    }

    public void OnClickExitNo()
    {
        SoundControllerMain.instance.PlayClickSound();
        exitPopup.SetActive(false);
    }
}
