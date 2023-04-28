using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUnlockManager : MonoBehaviour
{
    public static LevelUnlockManager instance;

    [SerializeField] int[] goalTillNextLevel = { 0, 1500, 3500, 800, 15000 };

    private int levelsUnlocked;

    void Awake()
    {
        if (instance == null) instance = this;

        levelsUnlocked = PlayerPrefsController.GetLevelsUnlocked();
    }

    public int GetPlayerCurrentGoal()
    {
        return goalTillNextLevel[levelsUnlocked];
    }

    public void AddNewLevelToRotation()
    {
        if (levelsUnlocked < goalTillNextLevel.Length - 1 && levelsUnlocked < SceneManager.sceneCountInBuildSettings - 1)
            levelsUnlocked++;
        else if (levelsUnlocked == SceneManager.sceneCountInBuildSettings - 1)
            GameSessionManager.instance.HasUnlockedEveryLevel = true;

        PlayerPrefsController.SetLevelsUnlocked(levelsUnlocked);
    }

    public void RefreshLevelsUnlocked()
    {
        levelsUnlocked = PlayerPrefsController.GetLevelsUnlocked();
        GameSessionManager isInGame = FindObjectOfType<GameSessionManager>();

        if (isInGame) Fader.instance.FadeToBlack();
    }

    public void LoadLevel()
    {
        //Set to levels Unlocked +1 to deal with Random.Range being exclusive at max value
        SceneManager.LoadScene(Random.Range(1, levelsUnlocked + 1));

        Debug.Log("Currently Unlocked levels: " + levelsUnlocked);
    }
}
