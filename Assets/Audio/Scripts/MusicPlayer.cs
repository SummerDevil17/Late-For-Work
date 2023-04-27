using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip ambienceClip, inInterrogationClip, onLosingClip;
    [SerializeField] AudioSource track1, track2;
    [SerializeField] float fadingTime = 1f;
    float track1MaxVolume, track2MaxVolume;
    bool isTrack1Playing = false, isInOverrideZone = false;
    AudioClip currentClipPlaying;

    //PlayerController playerController;
    //Fighter playerFighterComponent;
    //bool isPlayerDying = false;

    public static MusicPlayer instance;

    public bool IsInOverrideZone { get => isInOverrideZone; set => isInOverrideZone = value; }

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        track1.volume = PlayerPrefsController.GetMusicVolume();
        track2.volume = PlayerPrefsController.GetMusicVolume();
        //playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //playerFighterComponent = player.GetComponent<Fighter>();

        UpdateMaxVolume();

        ChangeMusic(ambienceClip);
    }

    private void Update()
    {
        //if (!isInOverrideZone && !isPlayerDying /*&& !isOverridden*/)
        /*{ 
            if (currentClipPlaying != inCombatClip && (playerFighterComponent.IsFighting ||
            (player.IsBeingChased && !playerFighterComponent.IsFighting)))
            { ChangeMusic(inCombatClip); }
            else if (currentClipPlaying != ambienceClip && !playerFighterComponent.IsFighting && !player.IsBeingChased)
            { RevertToAmbience(); }
        }*/
    }

    public void SetVolume(float volume)
    {
        track1.volume = volume;
        track2.volume = volume;

        UpdateMaxVolume();
    }

    public void ChangeMusic(AudioClip clipToPlay)
    {
        if (currentClipPlaying == clipToPlay) return;

        StopAllCoroutines();

        StartCoroutine(FadeTracks(clipToPlay));

        isTrack1Playing = !isTrack1Playing;
        currentClipPlaying = clipToPlay;
    }

    public void RevertToAmbience()
    {
        ChangeMusic(ambienceClip);
    }

    public void CutOutToLosing()
    {
        //isPlayerDying = true;

        StopAllCoroutines();
        StartCoroutine(FadeTracks(onLosingClip));

        /*if (isTrack1Playing)
        {
            track2.clip = OnDeathClip;
            track1.Stop();
            track2.Play();
        }
        else
        {
            track1.clip = OnDeathClip;
            track2.Stop();
            track1.Play();
        }
        isTrack1Playing = !isTrack1Playing;
        currentClipPlaying = OnDeathClip;*/
    }

    public void OverridePlayState()
    {
        track1.Stop();
        track2.Stop();
    }

    private IEnumerator FadeTracks(AudioClip clipToPlay)
    {
        float timeElapsed = 0;

        if (isTrack1Playing)
        {
            track2.clip = clipToPlay;
            track2.Play();

            while (timeElapsed < fadingTime)
            {
                track2.volume = Mathf.Lerp(0, track2MaxVolume, timeElapsed / fadingTime);
                track1.volume = Mathf.Lerp(track1MaxVolume, 0, timeElapsed / fadingTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track1.Stop();
        }
        else
        {
            track1.clip = clipToPlay;
            track1.Play();

            while (timeElapsed < fadingTime)
            {
                track1.volume = Mathf.Lerp(0, track1MaxVolume, timeElapsed / fadingTime);
                track2.volume = Mathf.Lerp(track2MaxVolume, 0, timeElapsed / fadingTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track2.Stop();
        }
    }

    private void UpdateMaxVolume()
    {
        track1MaxVolume = track1.volume;
        track2MaxVolume = track2.volume;
    }
}
