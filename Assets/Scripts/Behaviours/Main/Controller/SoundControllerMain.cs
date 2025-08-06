using UnityEngine;

public class SoundControllerMain : MonoBehaviour
{
    [Header("Assign your click sound here")]
    public AudioClip clickSound;

    private AudioSource audioSource;

    public static SoundControllerMain instance;

    private void Awake()
    {
        // Singleton pattern with safety
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: make it survive scene loads
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogError("SoundControllerMain: No AudioSource found on this GameObject. Adding one.");
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else if (instance != this)
        {
            // Destroy duplicates
            Destroy(gameObject);
        }
    }

    public void PlayClickSound()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("SoundControllerMain: AudioSource is null. Cannot play click sound.");
            return;
        }

        if (clickSound == null)
        {
            Debug.LogWarning("SoundControllerMain: Click sound is not assigned.");
            return;
        }

        audioSource.PlayOneShot(clickSound);
    }
}
