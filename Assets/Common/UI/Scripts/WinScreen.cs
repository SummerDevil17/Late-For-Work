using System.Collections;
using UnityEngine;
using TMPro;

public class WinScreen : MonoBehaviour, ICanvasDisplayer
{
    [SerializeField] GameObject uiToControl = null;
    [SerializeField] GameObject[] otherUIToDisable = null;
    [SerializeField] float timeToWaitBeforeTrigger = 2f;
    [SerializeField] TextMeshProUGUI finalScoreDisplay;

    [Header("Win Text Setup")]
    [SerializeField] TextMeshProUGUI winTextDisplay;
    [TextArea(1, 5)]
    [SerializeField] string winTextWithEveryLevel;

    [Header("Fanfare Audio Setup")]
    [SerializeField] AudioClip winFanfare;
    [SerializeField] float winFanfareVolume = 1f;


    [Header("Controller Support Fields")]
    bool lastState = false, isTringToQuit = false;

    public GameObject UiToControl { get => uiToControl; }
    public GameObject[] OtherUIToDisable { get => otherUIToDisable; }

    void Start()
    {
        uiToControl.SetActive(false);
    }

    void Update()
    {
        if (uiToControl.activeSelf == true)
        {
            SwitchUIS(false, 0);

            if (Input.anyKeyDown && !isTringToQuit)
            {
                GameSessionManager.instance.HasReachedGoal = false;
                Fader.instance.FadeToMainMenu();
            }
            else if (Input.anyKeyDown && isTringToQuit) Application.Quit();

            lastState = true;
            return;
        }

        if (uiToControl.activeSelf != lastState)
        {
            SwitchUIS(true, 1);
        }
        lastState = false;
    }

    public void TriggerMenu(bool quitMode)
    {
        isTringToQuit = quitMode;
        StartCoroutine(MenuTrigger());
    }

    private IEnumerator MenuTrigger()
    {
        yield return new WaitForSeconds(timeToWaitBeforeTrigger);

        uiToControl.SetActive(!uiToControl.activeSelf);

        finalScoreDisplay.text = string.Format("{0}/{1}", GameSessionManager.instance.TrashCompacted.ToString(),
        GameSessionManager.instance.ContractGoalAmount.ToString());
        if (GameSessionManager.instance.HasUnlockedEveryLevel)
            winTextDisplay.text = winTextWithEveryLevel;

        SFXInstance.instance.PlayOnceAtVolume(winFanfare, winFanfareVolume);
    }

    public void SwitchUIS(bool state, int timeScale)
    {
        Time.timeScale = timeScale;
        foreach (GameObject ui in otherUIToDisable)
        {
            ui.SetActive(state);
        }
    }

    public void ToggleUI(bool state)
    {
        uiToControl.SetActive(state);
    }
}