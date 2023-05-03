using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour, ICanvasDisplayer
{
    [SerializeField] GameObject uiToControl;
    [SerializeField] GameObject[] otherUIToDisable = null;
    public Slider masterVolumeSlider, musicVolumeSlider, SFXVolumeSlider;

    bool lastState = false;

    public GameObject UiToControl { get => uiToControl; }
    public GameObject[] OtherUIToDisable { get => otherUIToDisable; }

    void Start()
    {
        uiToControl.SetActive(false);

        masterVolumeSlider.onValueChanged.AddListener(AudioVolumeManager.instance.SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(AudioVolumeManager.instance.SetMusicVolume);
        SFXVolumeSlider.onValueChanged.AddListener(AudioVolumeManager.instance.SetSFXVolume);
    }

    private void Update()
    {
        if (uiToControl.activeSelf == true)
        {
            SwitchUIS(false, 0);
            lastState = true;
            if (!Cursor.visible) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleUI(false);
            }
            return;
        }

        if (uiToControl.activeSelf != lastState)
        {
            SwitchUIS(true, 0);
            SaveSettings();
        }
        lastState = false;
    }

    public void SwitchUIS(bool state, int timeScale)
    {
        Time.timeScale = timeScale;
        foreach (GameObject ui in otherUIToDisable)
        {
            if (ui.activeSelf == !state)
                ui.SetActive(state);
        }
    }

    public void ToggleUI(bool state)
    {
        uiToControl.SetActive(state);
    }

    public void LoadSettings()
    {
        masterVolumeSlider.value = PlayerPrefsController.GetMasterVolume();
        musicVolumeSlider.value = PlayerPrefsController.GetMusicVolume();
        SFXVolumeSlider.value = PlayerPrefsController.GetSFXVolume();

        AudioVolumeManager.instance.SetMasterVolume(masterVolumeSlider.value);
        AudioVolumeManager.instance.SetMusicVolume(musicVolumeSlider.value);
        AudioVolumeManager.instance.SetSFXVolume(SFXVolumeSlider.value);
    }

    public void ResetPlayerProgression()
    {
        PlayerPrefsController.ResetLevelsUnlocked();
        FindObjectOfType<LevelUnlockManager>().RefreshLevelsUnlocked();
    }

    private void SaveSettings()
    {
        //Audio Saving to PlayerPrefs
        PlayerPrefsController.SetMasterVolume(masterVolumeSlider.value);
        PlayerPrefsController.SetMusicVolume(musicVolumeSlider.value);
        PlayerPrefsController.SetSFXVolume(SFXVolumeSlider.value);
    }
}
