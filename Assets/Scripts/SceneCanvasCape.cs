using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCanvasCape : MonoBehaviour
{
    public void ChangeSceneToMain()
    {
        SoundControllerMain.instance.PlayClickSound();
        SceneManager.LoadSceneAsync(0);
    }
}
