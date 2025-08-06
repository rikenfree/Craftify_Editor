using UnityEngine;
using SuperStarSdk;

namespace Main.Controller
{
	public class RatingController1 : SceneElement1
	{
		public static RatingController1 instance;

        public GameObject succesfullPooup;

        public static readonly string KEY_RATED = "KEY_BOOL_RATED";

		public static readonly string KEY_GAME_PLAYED = "KEY_LONG_GAME_PLAYED";

        public int Rate
        {
            get
            {
                return (PlayerPrefs.GetInt("Rate", 0));
            }
            set
            {
                PlayerPrefs.SetInt("Rate", value);
            }
        }
        public int TimePlay
        {
            get
            {
                return (PlayerPrefs.GetInt("TimePlay", 0));
            }
            set
            {
                PlayerPrefs.SetInt("TimePlay", value);
            }
        }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public RectTransform rateCanvas;

		private void Start()
		{
			long num = TimesPlayed() + 1;
            TimePlay += 1;
            
            //Debug.Log("PLAYED: " + num + ", RATED: " + flag.ToString());
            //base.scene.SaveLong(KEY_GAME_PLAYED, num);
            if (MustShowRatePanel(TimePlay))
            {
                ShowRateCanvas();
            }

        }

		public void ShowRateCanvas()
		{
            rateCanvas.gameObject.SetActive(value: true);
		}

		public void HideRateCanvas()
		{
            SoundController1.Instance.PlayClickSound();
            rateCanvas.gameObject.SetActive(value: false);
		}

		public void ConfirmRate()
		{
            SoundController1.Instance.PlayClickSound();
            //base.scene.SaveBool(KEY_RATED, value: true);
            HideRateCanvas();
            SuperStarSdkManager.Instance.Rate();
            Rate = 1;
		}

		
		private long TimesPlayed()
		{
			return 0;// base.scene.LoadLong(KEY_GAME_PLAYED);
		}

		public bool MustShowRatePanel(long played)
		{
			if (Rate ==0)
			{
				return played % 3 == 0;
			}
			return false;
		}
	}
}
