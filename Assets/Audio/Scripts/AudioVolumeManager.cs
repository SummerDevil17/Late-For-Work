using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioVolumeManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    public static AudioVolumeManager instance;

    void Awake()
    {
        if (instance == null) { instance = this; }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) { LoadVolumeValues(); }

    void Start()
    {
        LoadVolumeValues();
    }

    void LoadVolumeValues()
    {
        FindObjectOfType<OptionsMenu>().LoadSettings();
    }

    public void SetMasterVolume(float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}
