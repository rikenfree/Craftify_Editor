using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperStarSdk
{ 
public class PermissionManager : MonoBehaviour
{
    public static PermissionManager Instance;

    private void Awake()
    {
        if (Instance==null)
        {
          Instance = this;
        }
    }

        private void Start()
        {

            if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image) == NativeGallery.Permission.Denied)
            {
                NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
            }
            else if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image) == NativeGallery.Permission.ShouldAsk)
            {
                NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);

            }

            GetExternalStoragePermission();
            GetCameraPermission();
        }

        public void GetExternalStoragePermission()
        {
            AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.WRITE_EXTERNAL_STORAGE");

            if (result == AndroidRuntimePermissions.Permission.Granted)
            {
                Debug.Log("We have permission to access external storage!");
            }
            else
            {
                Debug.Log("Permission state: " + result);
            }
        }

        public void GetCameraPermission()
        {
            AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.CAMERA");

            if (result == AndroidRuntimePermissions.Permission.Granted)
            {
                Debug.Log("We have permission to access external storage!");
            }
            else
            {
                Debug.Log("Permission state: " + result);
            }
        }

        public void GetContactsPermission()
        {
            AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.READ_CONTACTS");

            if (result == AndroidRuntimePermissions.Permission.Granted)
            {
                Debug.Log("We have permission to access external storage!");
            }
            else
            {
                Debug.Log("Permission state: " + result);
            }
        }
    }
}