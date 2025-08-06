using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Main.Controller;
using I2.Loc;

public class CustomLoding1 : MonoBehaviour
{
    public TextMeshProUGUI welcomePercentText;

    public TextMeshProUGUI SuccessText;

    public GameObject CapeEditorButton;
    public GameObject CustomCapeAddonButton;
    public GameObject backButton;
    public GameObject ProgressBar;

    public UnityEngine.UI.Image welcomeProgress;

    private float welcomeWaitTime = 0f;

    private bool welcomeDone = false;

    // Start is called before the first frame update
    private void Awake()
    {
#if UNITY_ANDROID
        welcomeWaitTime = 6;
#elif UNITY_IOS
        welcomeWaitTime=6;
#endif
    }

    public void Start()
    {
        ProgressBar.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!welcomeDone)
        {
            float time = Time.time;
            if (time >= welcomeWaitTime)
            {
                welcomeProgress.fillAmount = 1f;
                SuccessText.gameObject.SetActive(true);
                welcomePercentText.gameObject.SetActive(false);

                StartCoroutine(SetSuceessText());
            }
            else
            {
                welcomePercentText.gameObject.SetActive(true);
                int num = (int)(time / welcomeWaitTime * 100f);
                welcomeProgress.fillAmount = time / welcomeWaitTime;
                welcomePercentText.text = num + " %";

                SuccessText.gameObject.SetActive(false);

                CapeEditorButton.SetActive(false);
                CustomCapeAddonButton.SetActive(false);
                backButton.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ManageScene.instance.ChangeToScene(0);
        }
    }

    public IEnumerator SetSuceessText()
    {
        ProgressBar.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        welcomeDone = true;

        CapeEditorButton.SetActive(true);
        //string text1 = "Custom Cape Addon";
        //CapeEditorButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationManager.GetTranslation(text1);

        CustomCapeAddonButton.SetActive(true);
        //string text = "Cape Editor";
        //CustomCapeAddonButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationManager.GetTranslation(text);

        backButton.SetActive(true);
    }
    public void OnClickCapeEditorButton()
    {

        SoundController1.Instance.PlayClickSound();
        SceneManager.LoadSceneAsync(4);
        Debug.Log("Try_1");
        //SuperStarAd.Instance.ShowInterstitial();

    }

    public void OnClickCustomCapeAddonButton()
    {

        SoundController1.Instance.PlayClickSound();
        SceneManager.LoadSceneAsync(5);
        Debug.Log("Try_2");
        //SuperStarAd.Instance.ShowInterstitial();

    }
}
