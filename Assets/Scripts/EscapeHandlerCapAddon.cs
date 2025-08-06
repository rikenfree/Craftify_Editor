using UnityEngine;

public class EscapeHandlerCapAddon : MonoBehaviour
{
    [SerializeField] private CapeAddonHandler1 capeAddon; // Drag your CapeAddonHandler1 here

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            HandleEscape();
        }
    }

    private void HandleEscape()
    {
        // Tutorial
        if (capeAddon.TutorialParent.activeSelf)
        {
            for (int i = 0; i < capeAddon.TutorialScreen.Length; i++)
            {
                if (capeAddon.TutorialScreen[i].activeSelf)
                {
                    if (i == 0)
                    {
                        capeAddon.BackScreen(0);
                    }
                    else
                    {
                        capeAddon.BackScreen(i);
                    }
                    return;
                }
            }
        }

        // HD Texture
        if (capeAddon.HDTextureScreen.activeSelf)
        {
            capeAddon.PreviewCloseButton();
            return;
        }

        // View Image
        if (capeAddon.ViewImageScreen.activeSelf)
        {
            capeAddon.TextureCloseButton();
            return;
        }

        // Pick Image  exit
        if (capeAddon.PickImageScreen.activeSelf)
        {
            capeAddon.MainScreenCloseButton();
            return;
        }

        Debug.Log("EscapeHandler: Nothing matched.");
    }
}