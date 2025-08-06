using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeCameraUpdater : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyThemeToCamera();
    }

    private void Start()
    {
        ApplyThemeToCamera(); // Also apply immediately if scene already loaded
    }

    void ApplyThemeToCamera()
    {
        if (ColorClass.instance != null && Camera.main != null)
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor; // 👈 important line
            Camera.main.backgroundColor = ColorClass.instance.colors[ColorClass.instance.currentThemeIndex].backGroundcolor;
        }
    }

}
