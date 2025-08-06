using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThemeManagment : MonoBehaviour
{
    public Type type;

    Image image;
    TextMeshProUGUI textMeshPro;
    Text uiText;

    private void OnEnable()
    {
        StartCoroutine(ApplyThemeWhenReady()); // Always apply on enable
    }

    private IEnumerator ApplyThemeWhenReady()
    {
        yield return new WaitUntil(() => ColorClass.instance != null);

        // Lazy load components
        if (image == null) image = GetComponent<Image>();
        if (textMeshPro == null) textMeshPro = GetComponent<TextMeshProUGUI>();
        if (uiText == null) uiText = GetComponent<Text>();

        ApplyTheme();
    }

    private void ApplyTheme()
    {
        int selectedIndex = PlayerPrefs.GetInt("selectedColorIndex", 0);

        switch (type)
        {
            case Type.topBar:
                if (image != null)
                    image.color = ColorClass.instance.colors[selectedIndex].topBarcolor;
                break;

            case Type.button:
                if (image != null)
                    image.color = ColorClass.instance.colors[selectedIndex].buttoncolor;
                break;

            case Type.headertext:
                if (textMeshPro != null)
                    textMeshPro.color = ColorClass.instance.colors[selectedIndex].headerTextcolor;
                else if (uiText != null)
                    uiText.color = ColorClass.instance.colors[selectedIndex].headerTextcolor;
                break;

            case Type.buttontext:
                if (textMeshPro != null)
                    textMeshPro.color = ColorClass.instance.colors[selectedIndex].buttonTextcolor;
                else if (uiText != null)
                    uiText.color = ColorClass.instance.colors[selectedIndex].buttonTextcolor;
                break;

            case Type.normaltext:
                if (textMeshPro != null)
                    textMeshPro.color = ColorClass.instance.colors[selectedIndex].normalTextcolor;
                else if (uiText != null)
                    uiText.color = ColorClass.instance.colors[selectedIndex].normalTextcolor;
                break;

            case Type.background:
                if (image != null)
                    image.color = ColorClass.instance.colors[selectedIndex].backGroundcolor;
                break;
        }
    }

    public void ApplyThemeFromOutside()
    {
        StartCoroutine(ApplyThemeWhenReady());
    }

    public enum Type
    {
        none,
        button,
        topBar,
        headertext,
        buttontext,
        normaltext,
        background
    }
}
