using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuFirstButton;
    [SerializeField] GameObject optionsMenuFirstButton;
    [SerializeField] GameObject dialogueMenuFirstButton;
    [SerializeField] GameObject interrogationFirstSelectable;
    [SerializeField] GameObject lossScreenFirstButton;

    public GameObject DialogueMenuFirstButton { get => dialogueMenuFirstButton; set => dialogueMenuFirstButton = value; }

    public void UpdateButtonToShow(GameObject passingObject)
    {
        if (passingObject.GetComponent<PauseMenu>() == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pauseMenuFirstButton);
        }
        else if (passingObject.GetComponent<OptionsMenu>() == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(optionsMenuFirstButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(lossScreenFirstButton);
        }
    }

    public void ExitToPause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseMenuFirstButton);
    }
}
