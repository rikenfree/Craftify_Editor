using UnityEngine;

public class SoundManager2 : MonoBehaviour
{
	public static SoundManager2 instance;

	public AudioSource audioSource;

	public AudioClip buttonSound;

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

	public void PlayButtonSound()
	{
		audioSource.PlayOneShot(buttonSound, 0.5f);
	}
}
