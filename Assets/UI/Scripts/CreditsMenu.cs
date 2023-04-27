using UnityEngine;

public class CreditsMenu : MonoBehaviour, ICanvasDisplayer
{
    [SerializeField] GameObject uiToControl;
    [SerializeField] GameObject[] otherUIToDisable = null;

    bool lastState = false;

    public GameObject UiToControl { get => uiToControl; }
    public GameObject[] OtherUIToDisable { get => otherUIToDisable; }


    void Start()
    {
        uiToControl.SetActive(false);
    }

    private void Update()
    {
        if (uiToControl.activeSelf == true)
        {
            SwitchUIS(false, 0);
            lastState = true;
            if (!Cursor.visible) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
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
}
