using UnityEngine;

namespace Main.Controller
{
	public class SoundController1 : MonoBehaviour
	{
		public static SoundController1 Instance;

		public AudioClip clickSound;
		private AudioSource audioSource;

        public void Awake()
        {
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
        }

        private void Start()
		{
			audioSource = GetComponent<AudioSource>();
		}

		public void PlayClickSound()
		{
			audioSource.PlayOneShot(clickSound);
		}
	}
}
