using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public struct Resolution { public int width; public int height; }

public class ScreenResolutionManager : MonoBehaviour
{
    [SerializeField] List<Resolution> availableResolutions;
    [SerializeField] TextMeshProUGUI resolutionLabel;
    [SerializeField] Toggle fullScreenToggle;

    int currentResolutionIndex;

    private void Start()
    {
        fullScreenToggle.isOn = Screen.fullScreen;

        bool hasFoundResolution = false;
        for (int i = 0; i < availableResolutions.Count; i++)
        {
            if (Screen.width == availableResolutions[i].width && Screen.height == availableResolutions[i].height)
            {
                hasFoundResolution = true;
                currentResolutionIndex = i;
                UpdateResLabel();
            }
        }

        if (!hasFoundResolution)
        {
            Resolution newRes = new Resolution();

            newRes.width = Screen.width;
            newRes.height = Screen.height;

            availableResolutions.Add(newRes);
            currentResolutionIndex = availableResolutions.Count - 1;

            UpdateResLabel();
        }
    }

    public void ChangeResDown()
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0)
        {
            currentResolutionIndex = 0;
        }
        UpdateResLabel();
    }

    public void ChangeResUp()
    {
        currentResolutionIndex++;
        if (currentResolutionIndex >= availableResolutions.Count)
        {
            currentResolutionIndex = availableResolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void ApplyGraphicalChanges()
    {
        Screen.SetResolution(availableResolutions[currentResolutionIndex].width,
        availableResolutions[currentResolutionIndex].height, fullScreenToggle.isOn);
    }

    private void UpdateResLabel()
    {
        resolutionLabel.text = string.Format("{0} X {1}", availableResolutions[currentResolutionIndex].width,
        availableResolutions[currentResolutionIndex].height);
    }

}
