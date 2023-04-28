using UnityEngine;

public class IntroductionMenu : MonoBehaviour, ICanvasDisplayer
{
    [SerializeField] private GameObject uiToControl = null;
    [SerializeField] TutorialSkipper tutorialCanvas = null;
    [SerializeField] GameObject[] otherUIToDisable = null;

    bool lastState = false;
    bool hasDisplayedControls = false;

    public GameObject UiToControl { get => uiToControl; }
    public GameObject[] OtherUIToDisable { get => otherUIToDisable; }

    void OnEnable()
    {
        if (!IntroTextSkipper.instance.HasDisplayed)
        {
            uiToControl.SetActive(true);
        }
        else
        {
            hasDisplayedControls = true;
            lastState = true;
            uiToControl.SetActive(false);
        }
    }

    private void Update()
    {
        if (!hasDisplayedControls && Input.anyKeyDown)
        {
            IntroTextSkipper.instance.HasDisplayed = true;
            PlayerPrefsController.SetTutorialWatchedTo(true);
            ToggleUI(false);
            tutorialCanvas.ShowTutorial();
        }
        if (uiToControl.activeSelf == true)
        {
            SwitchUIS(false, 0);
            lastState = true;
            return;
        }
        if (uiToControl.activeSelf != lastState)
        {
            SwitchUIS(true, 1);
        }
        lastState = false;
    }

    public void SwitchUIS(bool flag, int timeScale)
    {
        Time.timeScale = timeScale;
        foreach (GameObject ui in otherUIToDisable)
        {
            ui.SetActive(flag);
        }
    }

    public void ToggleUI(bool state)
    {
        uiToControl.SetActive(state);
        hasDisplayedControls = !state;
    }
}
