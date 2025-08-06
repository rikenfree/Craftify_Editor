using UnityEngine;

namespace Main.Controller
{
    public class SoundController : MonoBehaviour
    {
        public AudioClip clickSound;

        private AudioSource audioSource;

        public static SoundController instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
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
