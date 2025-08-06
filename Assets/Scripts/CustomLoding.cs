using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SuperStarSdk;
using TMPro;
using I2.Loc;

public class CustomLoding : MonoBehaviour
{
    public TextMeshProUGUI welcomePercentText;

    public TextMeshProUGUI welcomeDoneText;

    public Image welcomeProgress;

    public GameObject ProgressBar;

    public float welcomeWaitTime = 0f;

    private bool welcomeDone = false;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IOS
welcomeWaitTime=6;
#else
        welcomeWaitTime = 6;
#endif

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

                welcomePercentText.text = "SUCCESS";
                string s = LocalizationManager.GetTranslation(welcomePercentText.text);
                welcomePercentText.text = s;

                welcomeDoneText.gameObject.SetActive(true);
                welcomeDone = true;
                ProgressBar.SetActive(false);
            }
            else
            {
                int num = (int)(time / welcomeWaitTime * 100f);
                welcomeProgress.fillAmount = time / welcomeWaitTime;
                welcomePercentText.text = num + " %";
                welcomeDoneText.gameObject.SetActive(false);
            }
        }
    }
    public void OnClickContinueButton()
    {
        if (welcomeDone)
        {
            SceneManager.LoadSceneAsync(2);

            //SuperStarAd.Instance.ShowInterstitialTimer((result) => {
            //    Debug.Log("Show Intrestitial  => " + result);
            //});

        }
    }
}
