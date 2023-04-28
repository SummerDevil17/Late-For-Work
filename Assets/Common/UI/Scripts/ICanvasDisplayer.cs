using UnityEngine;

public interface ICanvasDisplayer
{
    GameObject UiToControl { get; }
    GameObject[] OtherUIToDisable { get; }

    void SwitchUIS(bool state, int timeScale);
    void ToggleUI(bool state);
}
