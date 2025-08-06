using UnityEngine;
using UnityEngine.UI;
namespace I2.Loc
{
    [AddComponentMenu("I2/Localization/SetLanguage Button")]
    public class SetLanguage : MonoBehaviour
    {
        public string _Language;

#if UNITY_EDITOR
        public LanguageSource mSource;
#endif
        private void Start()
        {
            string currentLang = PlayerPrefs.GetString("language", LocalizationManager.CurrentLanguage);

            // Find all SetLanguage buttons
            var allButtons = FindObjectsByType<SetLanguage>(FindObjectsSortMode.None);

            foreach (var btn in allButtons)
            {
                // Disable all tickmarks first
                btn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }

            // If this button matches the current language, enable its tickmark
            if (currentLang == _Language)
            {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        public void ApplyLanguage()
        {
            PlayerPrefs.SetString("language", _Language);
            PlayerPrefs.Save();

            if (LocalizationManager.HasLanguage(_Language))
            {
                LocalizationManager.CurrentLanguage = _Language;
                // Show tickmark for this button
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);

                // Hide tickmark for all other SetLanguage buttons
                foreach (var btn in FindObjectsByType<SetLanguage>(FindObjectsSortMode.None))
                {
                    if (btn != this)
                        btn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        public void ResetLanguage()
        {
            if (LocalizationManager.HasLanguage(_Language))
            {
                LocalizationManager.CurrentLanguage = _Language;

                // Hide tickmark for all other SetLanguage buttons
                foreach (var btn in FindObjectsByType<SetLanguage>(FindObjectsSortMode.None))
                {
                    if (btn != this)
                        btn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
}