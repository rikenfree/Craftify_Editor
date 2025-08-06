using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SuperStarSdk;
namespace SuperStarSdk
{
    public class CustomLoding : MonoBehaviour
    {
        public Text welcomePercentText;

        public Text welcomeDoneText;

        public Image welcomeProgress;

        private float welcomeWaitTime = 8f;

        private bool welcomeDone = false;

        private void Start()
        {
#if UNITY_ANDROID
            welcomeWaitTime = SuperStarSdkManager.Instance.LoadingTime;
#elif UNITY_IOS
        welcomeWaitTime=0;
#endif
          //  LoadSceneAsync();
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
                    welcomeDoneText.gameObject.SetActive(true);
                    welcomeDone = true;
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
                //SceneManager.LoadScene(1);

                //SuperStarAd.Instance.ShowInterstitial();
               // asyncLoad.allowSceneActivation = true;
                Destroy(this.gameObject);

            }
        }

        AsyncOperation asyncLoad;
        public void LoadSceneAsync()
        {

            asyncLoad = SceneManager.LoadSceneAsync(1);
            asyncLoad.allowSceneActivation = false;

        }
    }
}