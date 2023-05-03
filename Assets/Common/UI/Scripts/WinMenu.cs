using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinMenu : MonoBehaviour, ICanvasDisplayer
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
    bool lastState = false;

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

            lastState = true;
            return;
        }

        if (uiToControl.activeSelf != lastState)
        {
            SwitchUIS(true, 1);
        }
        lastState = false;
    }

    public void TriggerMenu()
    {
        StartCoroutine(MenuTrigger());
    }

    private IEnumerator MenuTrigger()
    {
        yield return new WaitForSeconds(timeToWaitBeforeTrigger);

        uiToControl.SetActive(!uiToControl.activeSelf);

        finalScoreDisplay.text = string.Format("{0}\n{1}", GameSessionManager.instance.ScorePoints.ToString(),
        GameSessionManager.instance.CalculateTime());
        if (GameSessionManager.instance.HasUnlockedEveryLevel)
            winTextDisplay.text = winTextWithEveryLevel;

        SFXInstance.instance.PlayOnceAtVolume(winFanfare, winFanfareVolume);
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

    public void Quit()
    {
        Application.Quit();
    }
}