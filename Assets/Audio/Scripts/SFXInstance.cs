using UnityEngine;

public class SFXInstance : MonoBehaviour
{
    private AudioSource audioSource;

    public static SFXInstance instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayOnceAtVolume(AudioClip clipToPlay, float volume)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(clipToPlay);
    }
}
