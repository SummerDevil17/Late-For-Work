using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.Events;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager instance;

    [Header("Game Displays Set-up")]
    [SerializeField] TextMeshProUGUI goalDisplay;
    [SerializeField] Slider goalSlider;
    [SerializeField] Image goalSliderImage;
    [SerializeField] Color goalImageReachedColor;
    [SerializeField] AudioClip goalReachedConfirmationSFX;
    [SerializeField] TextMeshProUGUI currencyDisplay;
    [SerializeField] TextMeshProUGUI batteryDisplay;


    [Header("End Screens References")]
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject lossScreen;

    [Header("Cameras Related To Conveyer By Index")]
    [SerializeField] CinemachineVirtualCamera[] cameraArray;

    [SerializeField] UnityEvent<int> onTrashProcessed;

    private int contractGoalAmount;
    private int numberOfConveyersInGame = 1, conveyersDisabled = 0;
    private int trashCompacted = 0, currentCurrencyAmount = 0, currentBatteriesAmount = 0;
    private bool hasReachedGoal = false, hasUnlockedEveryLevel = false;

    public int NumberOfConveyersInGame { get => numberOfConveyersInGame; }
    public int TrashCompacted { get => trashCompacted; }
    public int ContractGoalAmount { get => contractGoalAmount; }
    public int TrashCompacted1 { get => trashCompacted; set => trashCompacted = value; }
    public int CurrentCurrencyAmount { get => currentCurrencyAmount; }
    public int CurrentBatteriesAmount { get => currentBatteriesAmount; }
    public bool HasReachedGoal { get => hasReachedGoal; set => hasReachedGoal = value; }
    public bool HasUnlockedEveryLevel { get => hasUnlockedEveryLevel; set => hasUnlockedEveryLevel = value; }

    void Awake() { if (instance == null) instance = this; }

    void Start()
    {
        contractGoalAmount = LevelUnlockManager.instance.GetPlayerCurrentGoal();

        goalDisplay.text = string.Format("{0}/{1}", trashCompacted, contractGoalAmount);
        goalSlider.value = 0;
        currencyDisplay.text = currentCurrencyAmount.ToString();
        batteryDisplay.text = currentBatteriesAmount.ToString();

        PickCameraForConveyerAmount();
    }

    public void AddToTrashCompacted(int amount)
    {
        trashCompacted += amount;
        goalDisplay.text = string.Format("{0}/{1}", trashCompacted, contractGoalAmount);

        goalSlider.value = Mathf.Clamp(trashCompacted / (float)contractGoalAmount, 0f, 1f);
        if (!hasReachedGoal && trashCompacted >= contractGoalAmount)
        {
            goalSliderImage.color = goalImageReachedColor;
            SFXInstance.instance.PlayOnceAtVolume(goalReachedConfirmationSFX, 0.45f);
            hasReachedGoal = true;
            LevelUnlockManager.instance.AddNewLevelToRotation();
        }
        onTrashProcessed.Invoke(amount);
    }

    public void AddToCurrency(int amount)
    {
        currentCurrencyAmount = Mathf.Clamp(currentCurrencyAmount + amount, 0, 999999);
        currencyDisplay.text = currentCurrencyAmount.ToString();
    }

    public void SubtractToCurrency(int amount)
    {
        currentCurrencyAmount = Mathf.Clamp(currentCurrencyAmount - amount, 0, 999999);
        currencyDisplay.text = currentCurrencyAmount.ToString();
    }

    public void PickUpBattery()
    {
        currentBatteriesAmount = Mathf.Clamp(currentBatteriesAmount + 1, 0, 99);
        batteryDisplay.text = currentBatteriesAmount.ToString();
    }

    public void UseBattery()
    {
        currentBatteriesAmount = Mathf.Clamp(currentBatteriesAmount - 1, 0, 99);
        batteryDisplay.text = currentBatteriesAmount.ToString();
    }

    public void AddNewConveyer()
    {
        numberOfConveyersInGame = Mathf.Clamp(numberOfConveyersInGame + 1, 0, 4);
        PickCameraForConveyerAmount();
    }

    public void DisableConveyer()
    {
        conveyersDisabled++;

        if (conveyersDisabled >= numberOfConveyersInGame && trashCompacted >= contractGoalAmount)
        {
            //Protection against making triggering Win Screen also
            hasReachedGoal = false;
            winScreen.GetComponent<WinMenu>().TriggerMenu();
        }
        else if (conveyersDisabled >= numberOfConveyersInGame && trashCompacted < contractGoalAmount)
            lossScreen.GetComponent<LossMenu>().TriggerMenu();
    }

    public void EnableConveyer() { conveyersDisabled--; }

    private void PickCameraForConveyerAmount()
    {
        foreach (CinemachineVirtualCamera camera in cameraArray)
        {
            camera.Priority = 10;
        }
        cameraArray[numberOfConveyersInGame - 1].Priority = 20;
    }
}
