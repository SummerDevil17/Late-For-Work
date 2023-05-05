using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager instance;

    [Header("Game Displays Set-up")]
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] TextMeshProUGUI timeElapsedDisplay;

    [Header("Pick-Up Button References")]
    [SerializeField] Button weaponButton;
    [SerializeField] Button healingButton;
    [SerializeField] Button throwButton;

    [Header("End Screens References")]
    [SerializeField] WinMenu winScreen;
    [SerializeField] LossMenu lossScreen;

    private int scorePoints = 0;
    private float timeElapsed = 0f;
    private bool hasUnlockedEveryLevel = false;

    public int ScorePoints { get => scorePoints; }
    public float TimeElapsed { get => timeElapsed; }

    public bool HasUnlockedEveryLevel { get => hasUnlockedEveryLevel; set => hasUnlockedEveryLevel = value; }

    private void Awake() { if (instance == null) instance = this; }

    private void Start()
    {
        scoreDisplay.text = scorePoints.ToString();
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            timeElapsed += Time.deltaTime;
            timeElapsedDisplay.text = CalculateTime();
        }
    }

    public void UpdateHealthBar(float amount) { healthBar.value = amount; }

    public void AddToScore(int amount)
    {
        scorePoints = Mathf.Clamp(scorePoints + amount, 0, 99999999);
        scoreDisplay.text = scorePoints.ToString();
    }

    public string CalculateTime()
    {
        float hours = Mathf.FloorToInt((timeElapsed / 60) / 60);
        float minutes = Mathf.FloorToInt((timeElapsed / 60) % 60);
        float seconds = Mathf.FloorToInt(timeElapsed % 60);

        if (hours > 0) { return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds); }
        else
            return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void WinGame() { winScreen.TriggerMenu(); }
    public void LoseGame() { lossScreen.TriggerMenu(); }

    public void EnableButton(GameObject caller)
    {
        if (caller.TryGetComponent<WeaponPickUp>(out WeaponPickUp weapon)) { weaponButton.gameObject.SetActive(true); }
        else if (caller.TryGetComponent<HealthPickUp>(out HealthPickUp healing)) { healingButton.gameObject.SetActive(true); }
        else if (caller.TryGetComponent<PlayerController>(out PlayerController player))
        { throwButton.gameObject.SetActive(true); }
    }

    public void DisableButton(GameObject caller)
    {
        if (weaponButton.gameObject.activeSelf == true && caller.TryGetComponent<WeaponPickUp>(out WeaponPickUp weapon))
        { weaponButton.gameObject.SetActive(false); }
        else if (healingButton.gameObject.activeSelf == true && caller.TryGetComponent<HealthPickUp>(out HealthPickUp healing))
        { healingButton.gameObject.SetActive(false); }
        else if (throwButton.gameObject.activeSelf == true && caller.TryGetComponent<PlayerController>(out PlayerController player))
        { throwButton.gameObject.SetActive(false); }
    }
}
