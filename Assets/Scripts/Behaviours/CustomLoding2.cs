using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using I2.Loc;

public class CustomLoding2 : MonoBehaviour
{
    public TextMeshProUGUI welcomePercentText;
    public TextMeshProUGUI tapToContinueText;
    public Image welcomeProgress;

    public GameObject ProgressBar;
    public GameObject eventSystem;

    [Header("Root object for entire loading overlay")]
    public GameObject loadingOverlayRoot;

    public float fillDuration = 6f; // Fake progress bar duration
    public float minDelayAfterTap = 0.75f;  // This delay starts only after tap

    private bool loadingDone = false;

    void Awake()
    {
        eventSystem.SetActive(false);
        // Load Main Scene immediately and fully activate it
        SceneManager.LoadSceneAsync(7, LoadSceneMode.Additive);
    }

    void Update()
    {
        if (!loadingDone)
        {
            float time = Time.time;
            if (time >= fillDuration)
            {
                welcomeProgress.fillAmount = 1f;

                welcomePercentText.text = "SUCCESS";
                string s = LocalizationManager.GetTranslation(welcomePercentText.text);
                welcomePercentText.text = s;

                tapToContinueText.gameObject.SetActive(true);
                loadingDone = true;
                ProgressBar.SetActive(false);
            }
            else
            {
                int num = (int)(time / fillDuration * 100f);
                welcomeProgress.fillAmount = time / fillDuration;
                welcomePercentText.text = num + " %";
                tapToContinueText.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickContinueButton()
    {
        if (loadingDone)
        {
            SoundControllerMain.instance.PlayClickSound();
            StartCoroutine(HideLoadingOverlayAfterDelay());
        }
    }

    IEnumerator HideLoadingOverlayAfterDelay()
    {
        yield return new WaitForSeconds(minDelayAfterTap);

        // Deactivate the whole loading overlay
        loadingOverlayRoot.SetActive(false);

        // Reactivate the Event System if needed
        eventSystem.SetActive(true);

        // Now fully unload this Loading Scene
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
