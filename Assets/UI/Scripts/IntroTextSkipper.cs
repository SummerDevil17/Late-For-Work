using UnityEngine;

public class IntroTextSkipper : MonoBehaviour
{
    public static IntroTextSkipper instance;

    private bool hasDisplayed = false;

    public bool HasDisplayed { get => hasDisplayed; set => hasDisplayed = value; }

    void Awake()
    {
        if (instance == null) { instance = this; }

        hasDisplayed = PlayerPrefsController.GetIfTutorialWatched();
    }
}
