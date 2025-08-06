using SuperStarSdk;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class WelcomeManager2 : MonoBehaviour
{
	public static WelcomeManager2 instance;

	public GameObject welcomePanel;

	public Text welcomeText;

	private bool loading;

	private float startTime;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
		{

		}
		else
		{

			Permission.RequestUserPermission(Permission.ExternalStorageWrite);
		}
		if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
		{

		}
		else
		{

			Permission.RequestUserPermission(Permission.ExternalStorageRead);
		}
		startTime = Time.time;
		loading = true;

	}

	private void Update()
	{
		if (loading)
		{
			/*float num = Time.time - startTime;
			welcomeText.text = "Loading ... " + (int)(num * 100f / 7f) + "%";
			if (num >=7f)
			{
				welcomeText.text = "Success!\nTap to Continue.";
				loading = false;
			}*/
		}
	}

	public void Tap()
	{
		if (!loading)
		{
            SuperStarAd.Instance.ShowInterstitialTimer(null);
            if (GuiManager2.instance.Login == 0)
            {
				//GuiManager.instance.LoginScreen.SetActive(true);
				GuiManager2.instance.LoginScreen.SetActive(false);

            }
            else
            {
				GuiManager2.instance.LoginScreen.SetActive(false);
			}
            welcomePanel.SetActive(false);
            //AdMobAdManager.Instance.ShowInterstitialThenSwitchToMainScene();
        }

       
    }
}
