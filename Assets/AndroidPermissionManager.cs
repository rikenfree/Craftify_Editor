using UnityEngine;
using UnityEngine.Android;

public class AndroidPermissionManager : MonoBehaviour
{
    public static AndroidPermissionManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CheckAndRequestPermission();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckAndRequestPermission()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        int sdkInt = GetSdkInt();

        if (sdkInt <= 29) // Android 10 or below
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Debug.Log("🔐 Requesting READ_EXTERNAL_STORAGE");
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Debug.Log("🔐 Requesting WRITE_EXTERNAL_STORAGE");
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }
        }
        else
        {
            Debug.Log("ℹ️ No runtime storage permissions needed on Android 11+ (use /Download only)");
        }
#endif
    }

    private int GetSdkInt()
    {
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT");
        }
    }
}
