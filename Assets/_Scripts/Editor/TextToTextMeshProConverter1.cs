using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TextToTextMeshProConverter1 : EditorWindow
{
    // Add a field to set a default TextMeshPro font in the Editor
    public static TMP_FontAsset defaultTMPFont;
    public static Text referenceText;

    [MenuItem("Tools/Convert Text to TextMeshPro")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TextToTextMeshProConverter1), true, "Convert Text to TextMeshPro");
    }

    void OnGUI()
    {
        // Allow users to set a default TMP Font Asset through the editor window
        defaultTMPFont = EditorGUILayout.ObjectField("Default TMP Font", defaultTMPFont, typeof(TMP_FontAsset), false) as TMP_FontAsset;
        referenceText = EditorGUILayout.ObjectField("referenceText", referenceText, typeof(Text), true) as Text;

        if (GUILayout.Button("Convert All Text to TextMeshPro"))
        {
            ConvertAllTextComponents();
        }
    }

    private static void ConvertAllTextComponents()
    {
        // Find all Text components in the scene
        Text[] texts = Resources.FindObjectsOfTypeAll<Text>();

        foreach (Text text in texts)
        {

            if (text.transform.name!="Riken")
            {

            // Create a new TextMeshProUGUI component on the same GameObject
            GameObject go = text.gameObject;

            referenceText.text = text.text;
            referenceText.fontSize = text.fontSize;
            referenceText.alignment = text.alignment;
            referenceText.color = text.color;
            referenceText.rectTransform.sizeDelta = text.rectTransform.sizeDelta;

            // Remove the original Text component
                Debug.Log(text.transform.name);
            DestroyImmediate(text);

            TMP_Text tmp = go.AddComponent<TextMeshProUGUI>();

            // Copy properties from Text to TextMeshProUGUI
            tmp.text = referenceText.text;
            tmp.fontSize = referenceText.fontSize;
            tmp.alignment = GetTMPAlignment(referenceText.alignment);
            tmp.color = referenceText.color;

            // Assign the TMP_FontAsset, if available
            tmp.font = defaultTMPFont;

            // Optionally, copy RectTransform properties if they differ
            tmp.rectTransform.sizeDelta = referenceText.rectTransform.sizeDelta;
            }

          
        }

        Debug.Log("Conversion completed. " + texts.Length + " Text elements converted to TextMeshProUGUI.");
    }

    private static TMP_FontAsset GetTMPFontAsset(Font oldFont)
    {
        // Placeholder for a method to map Unity fonts to TMP fonts
        // Implement your own mapping logic here depending on your project's fonts
        // For example, you could have a dictionary mapping Unity font names to TMP_FontAsset paths
        Dictionary<string, TMP_FontAsset> fontMap = new Dictionary<string, TMP_FontAsset>
        {
            { "Arial", Resources.Load<TMP_FontAsset>("Fonts & Materials/Arial_SDF") },
            // Add more mappings as needed
        };

        TMP_FontAsset tmpFontAsset;
        if (fontMap.TryGetValue(oldFont.name, out tmpFontAsset))
        {
            return tmpFontAsset;
        }

        return null;
    }

    private static TextAlignmentOptions GetTMPAlignment(TextAnchor anchor)
    {
        switch (anchor)
        {
            case TextAnchor.UpperLeft: return TextAlignmentOptions.TopLeft;
            case TextAnchor.UpperCenter: return TextAlignmentOptions.Top;
            case TextAnchor.UpperRight: return TextAlignmentOptions.TopRight;
            case TextAnchor.MiddleLeft: return TextAlignmentOptions.MidlineLeft;
            case TextAnchor.MiddleCenter: return TextAlignmentOptions.Center;
            case TextAnchor.MiddleRight: return TextAlignmentOptions.MidlineRight;
            case TextAnchor.LowerLeft: return TextAlignmentOptions.BottomLeft;
            case TextAnchor.LowerCenter: return TextAlignmentOptions.Bottom;
            case TextAnchor.LowerRight: return TextAlignmentOptions.BottomRight;
            default: return TextAlignmentOptions.Center;
        }
    }
}
