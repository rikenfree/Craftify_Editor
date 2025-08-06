using I2.Loc;

namespace Main.View
{
	public class InfoCanvas : SceneElement
	{
        public Localize titleLocalize;
        public Localize infoLocalize;

        public void ShowAboutUsInfo()
        {
            titleLocalize.Term = "Skin Editor 3D Version 1.1.7";
            infoLocalize.Term = "Contact us: remoro.studios@gmail.com";
            gameObject.SetActive(true);
        }

        public void ShowMCPEExportSuccessInfo()
        {
            titleLocalize.Term = "Success!";
            infoLocalize.Term = "Please Restart MCPE";
            gameObject.SetActive(true);
        }

        public void ShowGalleryExportSuccessInfo()
        {
            titleLocalize.Term = "Success!";
            infoLocalize.Term = "Skin is saved to Gallery.";
            gameObject.SetActive(true);
        }

        public void Show(string titleKey, string infoKey)
        {
            titleLocalize.Term = titleKey;
            infoLocalize.Term = infoKey;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
