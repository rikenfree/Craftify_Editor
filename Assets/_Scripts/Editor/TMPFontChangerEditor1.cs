using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(TMPFontChanger))]
public class TMPFontChangerEditor1 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default variables

        TMPFontChanger fontChanger = (TMPFontChanger)target;

        if (GUILayout.Button("Change Fonts"))
        {
            TMP_Text[] texts = Resources.FindObjectsOfTypeAll<TMP_Text>();
            foreach (TMP_Text text in texts)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(text))
                    continue;

                text.font = fontChanger.newFont;
                EditorUtility.SetDirty(text); // Marks the text object as "dirty" to ensure changes are saved
            }
        }
    }
}
