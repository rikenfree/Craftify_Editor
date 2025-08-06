using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(FontChanger))]
public class FontChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default variables

        FontChanger fontChanger = (FontChanger)target;

        if (GUILayout.Button("Change Fonts"))
        {
            Text[] texts = Resources.FindObjectsOfTypeAll<Text>();
            foreach (Text text in texts)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(text))
                    continue;

                text.font = fontChanger.newFont;
                EditorUtility.SetDirty(text); // Marks the text object as "dirty" to ensure changes are saved
            }
        }
    }
}
