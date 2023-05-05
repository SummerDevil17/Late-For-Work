using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LossMenu : MonoBehaviour, ICanvasDisplayer
{
    [SerializeField] GameObject uiToControl = null;
    [SerializeField] GameObject[] otherUIToDisable = null;
    [SerializeField] float timeToWaitBeforeTrigger = 2f;
    [SerializeField] TextMeshProUGUI finalScoreDisplay;

    [Header("Fanfare Audio Setup")]
    [SerializeField] AudioClip loseFanfare;
    [SerializeField] float loseFanfareVolume = 1f;

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
        SFXInstance.instance.PlayOnceAtVolume(loseFanfare, loseFanfareVolume);
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

    public void TryLoadLevel()
    {
        var levelUnlocker = FindObjectOfType<LevelUnlockManager>();

        //if (levelUnlocker) levelUnlocker.LoadLevel();
        /*else*/
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        Application.Quit();
    }
}