using Main.Controller;
using I2.Loc;

namespace Main.View
{
	public class ErrorCanvas1 : SceneElement1
	{
        public Localize titleLocalize;
        public Localize infoLocalize;

        public void ShowCustomError(string titleKey, string infoKey)
        {
            titleLocalize.Term = titleKey;
            infoLocalize.Term = infoKey;
            gameObject.SetActive(true);
        }

        public void ShowLoadingSkinOnlineError(bool random)
        {
            titleLocalize.Term = "Can't Download Skin";

            if (random)
            {
                infoLocalize.Term = "Please Try Again";
            }
            else
            {
                infoLocalize.Term = "Error_CheckInternetOrName";
            }

            gameObject.SetActive(true);
        }

        public void ShowSkinWrongSizeError()
        {
            titleLocalize.Term = "Unsupported Skin Size";
            infoLocalize.Term = "We only Support 64x32 Skin.";
            gameObject.SetActive(true);
        }

        public void ShowCantFindMCPESkinError()
        {
            titleLocalize.Term = "Can't Locate MCPE";
            infoLocalize.Term = "Please Import from Gallery Instead.";
            gameObject.SetActive(true);
        }

        public void ShowCantSaveMCPESkinError()
        {
            titleLocalize.Term = "Can't Save MCPE";
            infoLocalize.Term = "Please Export to Gallery Instead.";
            gameObject.SetActive(true);
        }

        public void ShowCantSaveGallerySkinError()
        {
            titleLocalize.Term = "Error";
            infoLocalize.Term = "Can't Save Skin to Gallery.";
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            SoundController1.Instance?.PlayClickSound();
            gameObject.SetActive(false);
        }
    }
}
