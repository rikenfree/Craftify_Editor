using TMPro;
using UnityEngine.UI;

namespace Main.View
{
	public class WaitingCanvas1 : SceneElement1
	{
		public TextMeshProUGUI title;

		public TextMeshProUGUI info;

		public void ShowCustomInformation(string titleString, string infoString)
		{
			title.text = titleString;
			info.text = infoString;
			base.gameObject.SetActive(value: true);
		}

		public void ShowLoadingSkinOnlineWaiting()
		{
			title.text = "Downloading Skin";
			info.text = "Please wait . . .";
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
