using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioRandomizer : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips = null;
    [SerializeField] bool changeOnUpdate = false;
    AudioSource audioSource = null;
    float counterTillChange = 0f;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        counterTillChange += Time.deltaTime;
        if (changeOnUpdate)
        {
            if (counterTillChange > audioSource.clip.length)
            {
                audioSource.Stop();
                ChangeClip();
                audioSource.Play();
                counterTillChange = 0f;
            }
        }
    }

    private void ChangeClip()
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
    }

    public void PlayClip()
    {
        if (!audioSource.isPlaying)
        {
            ChangeClip();
            audioSource.Play();
        }
    }
}
