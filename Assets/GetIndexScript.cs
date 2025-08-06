using UnityEngine;
using UnityEngine.UI;


public class GetIndexScript : MonoBehaviour
{
    void Start()
    {
        int savedIndex = PlayerPrefs.GetInt("selectedColorIndex", 0);
        HighlightSavedIndex(savedIndex);
    }

    public void GetIndex(int index, GameObject clickedButton)
    {
        Debug.Log("Selected Color Index: " + index);
        PlayerPrefs.SetInt("selectedColorIndex", index);

        // Deactivate ALL highlights
        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button btn in buttons)
        {
            Transform highlight = btn.transform.Find("Highlight");
            if (highlight != null)
            {
                highlight.gameObject.SetActive(false);
            }
        }

        // Activate clicked button's highlight
        Transform clickedHighlight = clickedButton.transform.Find("Highlight");
        if (clickedHighlight != null)
        {
            clickedHighlight.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No Highlight child found on clicked button: " + clickedButton.name);
        }

        // Refresh theme
        ThemeManagment[] themeElements = Resources.FindObjectsOfTypeAll<ThemeManagment>();
        foreach (var element in themeElements)
        {
            if (element.gameObject.scene.IsValid())
            {
                element.ApplyThemeFromOutside();
            }
        }
    }

    private void HighlightSavedIndex(int savedIndex)
    {
        ColorButton[] colorButtons = GetComponentsInChildren<ColorButton>(true);
        foreach (ColorButton cb in colorButtons)
        {
            Transform highlight = cb.transform.Find("Highlight");
            if (highlight != null)
            {
                highlight.gameObject.SetActive(cb.myIndex == savedIndex);
            }
        }
    }
}
