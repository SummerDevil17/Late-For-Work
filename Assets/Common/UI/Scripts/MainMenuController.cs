using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Fader fadeController = null;
    AudioSource audioSource = null;
    [Header("Audio Clip to Play")]
    [SerializeField] AudioClip audioClip;

    private void Awake()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }
    private void Start()
    {
        audioSource.Play();
    }

    public void StartFadeToGame()
    {
        fadeController.FadeToBlack();
    }

    public void TryLoadLevel()
    {
        var levelUnlocker = FindObjectOfType<LevelUnlockManager>();

        if (levelUnlocker) levelUnlocker.LoadLevel();
        else SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
