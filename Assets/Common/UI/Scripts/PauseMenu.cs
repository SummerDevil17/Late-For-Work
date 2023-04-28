using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour, ICanvasDisplayer
{
    [SerializeField] GameObject uiToControl = null;
    [SerializeField] GameObject[] otherUIToDisable = null;

    [Header("Screen to Show if has reached goal before quitting")]
    [SerializeField] WinScreen winScreenBeforeQuit;
    bool lastState = false;

    public GameObject UiToControl { get => uiToControl; }
    public GameObject[] OtherUIToDisable { get => otherUIToDisable; }


    void Start()
    {
        uiToControl.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUI(!uiToControl.activeSelf);
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

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        if (!GameSessionManager.instance || !GameSessionManager.instance.HasReachedGoal)
            Application.Quit();
        else if (winScreenBeforeQuit)
        {
            ToggleUI(false);
            winScreenBeforeQuit.TriggerMenu(true);
        }
    }
}
