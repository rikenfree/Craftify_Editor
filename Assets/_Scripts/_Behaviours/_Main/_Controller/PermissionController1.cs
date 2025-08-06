using UnityEngine;
using UnityEngine.Android;

namespace Main.Controller
{
	public class PermissionController1 : SceneElement1
	{
		public GameObject dialog;

		public void RequestWritePermission()
		{
			Permission.RequestUserPermission("android.permission.WRITE_EXTERNAL_STORAGE");
		}

		private void Start()
		{
			MonoBehaviour.print("Start asking for permission.");
			if (!Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE"))
			{
				RequestWritePermission();
			}
		}

		private void OnGUI()
		{
			if (!Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE"))
			{
				//dialog.SetActive(value: true);
			}
			else if (dialog != null)
			{
				//dialog.SetActive(value: false);
			}
		}
	}
}
