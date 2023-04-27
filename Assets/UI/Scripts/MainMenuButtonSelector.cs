using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButtonSelector : MonoBehaviour
{
    [SerializeField] GameObject mainMenuFirstButton;
    [SerializeField] GameObject optionsMenuFirstButton;
    [SerializeField] GameObject creditsExitButton;

    public void UpdateButtonToShow(GameObject passingObject)
    {
        if (passingObject.GetComponent<MainMenuController>() == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
        }
        else if (passingObject.GetComponent<OptionsMenu>() == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(optionsMenuFirstButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(creditsExitButton);
        }
    }

    public void ResetToMain()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
    }
}
