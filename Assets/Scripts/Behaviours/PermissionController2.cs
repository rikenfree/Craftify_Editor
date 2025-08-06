using UnityEngine;
using UnityEngine.Android;

public class PermissionController2 : MonoBehaviour
{
	public GameObject dialog;

    //private void Awake()
    //{
    //    Debug.LogError("IsStoragePermitGranted" + !IsStoragePermitGranted());
    //    if (!IsStoragePermitGranted())
    //    {
    //        AndroidRuntimePermissions.Permission permit = AndroidRuntimePermissions.RequestPermission("android.permission.WRITE_EXTERNAL_STORAGE");
    //    }     
    //}
    //public bool IsStoragePermitGranted()
    //{
    //    if (Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE"))
    //    {
    //        return true;
    //    }
    //    return true;
    //}

    //public void RequestWritePermission()
    //{
    //    Permission.RequestUserPermission("android.permission.WRITE_EXTERNAL_STORAGE");
    //}
 //   private void Start()
	//{
	//	Debug.Log("Start asking for permission.");
	//	if (!Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE"))
	//	{
	//		RequestWritePermission();
	//	}
	//}

	//private void OnGUI()
	//{
 //       Debug.LogError("OnGui:" + !Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE"));
	//	if (!Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE"))
	//	{
	//		dialog.SetActive(true);
	//	}
	//	else if (dialog != null)
	//	{
	//		dialog.SetActive(false);
	//	}
	//}
}
