using OneSignalSDK;
using UnityEngine;

public class OneSignalNotification2 : MonoBehaviour
{

    void Start()
    {
        // Replace 'YOUR_ONESIGNAL_APP_ID' with your OneSignal App ID from app.onesignal.com
        OneSignal.Initialize("ae1c1b8e-55b6-4b2f-be53-7552f9c0e3ce");
    }
}
