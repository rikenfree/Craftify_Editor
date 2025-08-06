using TMPro;
using UnityEngine;
using Unity.Collections;
using System;

namespace SuperStarSdk
{
    public class SSFeedbackManager : MonoBehaviour
{
    public TextMeshProUGUI supportEmailtxt;

    private void OnEnable()
     {
       if (string.IsNullOrEmpty( SuperStarSdkManager.Instance.crossPromoAssetsRoot.feedbackmail )) {

        Debug.LogError("Feedback URL is not available");
                supportEmailtxt.text ="keshavgamehead@gmail.com";
       }
            else{

                supportEmailtxt.text = SuperStarSdkManager.Instance.crossPromoAssetsRoot.feedbackmail;
       }
    }


    public void OnButtonSendMail()
    {
            string email = supportEmailtxt.text;
          

            string message = "";
            message += "\nApp: " + Application.productName;
#if UNITY_ANDROID
            message += "\nOS: " + getSDKInt();

#elif UNITY_IOS
        message += "\nOS: " + Device.systemVersion;
#endif
          
            message += "\nDEV: " + SystemInfo.deviceName + " " + SystemInfo.deviceModel;
            message += "\n\n\n\n ";
            string subject = Application.productName + " - " + Application.platform + " - Support (v" + Application.version + ")";

            Debug.Log("SendingMail..."+ message);

            SendEmail(email,"Feedback From USER " + Application.productName , message);
    }

    public void OnButtonNoThanks(){

        gameObject.SetActive(false);
    }

        public static void SendEmail(string toEmail, string emailSubject, string emailBody)
        {
            emailSubject = System.Uri.EscapeUriString(emailSubject);
            emailBody = System.Uri.EscapeUriString(emailBody);

            
#if UNITY_IOS
        string message = "mailto:" + toEmail + "?subject=" + emailSubject + "&body=" + emailBody;
#else
            string message = "mailto:" + toEmail + "?subject=" + emailSubject + "&body=" + emailBody;
#endif
            Debug.Log("message " + message);
            try
            {

                Application.OpenURL(message);
            }
            catch (Exception ex)
            {
#if UNITY_IOS
            try
            {

                Application.OpenURL("mailto:" + toEmail + "?subject=" + emailSubject + "&body=" + emailBody);
            }
            catch (Exception e)
            {
                //print("ex " + ex.Message);
            }
#endif
            }
        }

       

        public static int getSDKInt()
        {
#if !UNITY_IOS && !UNITY_EDITOR
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT");
        }
#else
            return 25;
#endif
        }
    }

}