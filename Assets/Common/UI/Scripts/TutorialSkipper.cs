using UnityEngine;

public class TutorialSkipper : MonoBehaviour
{
    [SerializeField] private GameObject uiToControl = null;
    [SerializeField] GameObject[] otherUIToDisable = null;

    bool lastState = false;

    public GameObject UiToControl { get => uiToControl; }
    public GameObject[] OtherUIToDisable { get => otherUIToDisable; }

    void Start()
    {
        uiToControl.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            uiToControl.SetActive(false);
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

    public void ShowTutorial()
    {
        uiToControl.SetActive(true);
    }
}
