using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip matchSound;
    public AudioClip mismatchSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMatchSound()
    {
        audioSource.PlayOneShot(matchSound);
    }

    public void PlayMismatchSound()
    {
        audioSource.PlayOneShot(mismatchSound);
    }

    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }

    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }
}
