using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class Fader : MonoBehaviour
{
    public static Fader instance;

    [SerializeField] UnityEvent onFadingToLevel;
    [SerializeField] UnityEvent onFadingToMainMenu;
    [SerializeField] VideoPlayer BGVideoPlayer;
    [SerializeField] WinScreen winScreen = null;
    private Animator fadingAnimator;

    void Awake()
    {
        if (instance == null) instance = this;

        fadingAnimator = GetComponent<Animator>();

        if (BGVideoPlayer != null)
        {
            fadingAnimator.enabled = false;
            BGVideoPlayer.started += StartAnimating;
        }
    }

    private void StartAnimating(VideoPlayer source)
    {
        fadingAnimator.enabled = true;
    }

    public void FadeToBlack()
    {
        fadingAnimator.SetTrigger("fadeToBlack");
    }

    public void FadeToClear()
    {
        fadingAnimator.SetTrigger("fadeToClear");
    }
    public void FadeToMainMenu()
    {
        //if doesn't exist or if exists but has not reached goal
        if (!GameSessionManager.instance || !GameSessionManager.instance.HasReachedGoal)
            fadingAnimator.SetTrigger("fadeToMain");
        else if (winScreen)
            winScreen.TriggerMenu(false);
    }

    public void FadeToBlackImmediately()
    {
        fadingAnimator.SetTrigger("fadeToBlackImmediately");
    }

    public void OnFinishToLevel()
    {
        onFadingToLevel.Invoke();
    }

    public void OnFinishToMainMenu()
    {
        onFadingToMainMenu.Invoke();
    }
}