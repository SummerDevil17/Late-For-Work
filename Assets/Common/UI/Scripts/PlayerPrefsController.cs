using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    const string MOUSE_SENSITIVITY_KEY = "Mouse Sensitivity";
    const string ANALOG_SENSITIVITY_KEY = "Analog Sensitivity";
    const string MASTER_VOLUME_KEY = "Master Volume";
    const string MUSIC_VOLUME_KEY = "Music Volume";
    const string SFX_VOLUME_KEY = "Sound Effects Volume";
    const string HAS_SEEN_TUTORIAL = "Tutorial Watched";
    const string LEVELS_UNLOCKED = "Levels Unlocked";

    const float MIN_SENSITIVITY = 0.1f;
    const float MAX_SENSITIVITY = 10f;

    const float MIN_VOL = 0f;
    const float MAX_VOL = 1f;


    public static void SetMouseSensitivity(float amount)
    {
        if (amount >= MIN_SENSITIVITY && amount <= MAX_SENSITIVITY)
        {
            Debug.Log("Mouse Sensitivity set to the value of " + amount);
            PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_KEY, amount);
        }
        else
        {
            Debug.LogError("Mouse Sensitivity is Out of Range");
        }
    }

    public static float GetMouseSensitivity()
    {
        return PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_KEY, 4f);
    }

    public static void SetAnalogSensitivity(float amount)
    {
        if (amount >= MIN_SENSITIVITY && amount <= MAX_SENSITIVITY)
        {
            Debug.Log("Analog Sensitivity set to the value of " + amount);
            PlayerPrefs.SetFloat(ANALOG_SENSITIVITY_KEY, amount);
        }
        else
        {
            Debug.LogError("Analog Sensitivity is Out of Range");
        }
    }

    public static float GetAnalogSensitivity()
    {
        return PlayerPrefs.GetFloat(ANALOG_SENSITIVITY_KEY, 3f);
    }


    public static void SetMasterVolume(float amount)
    {
        if (amount >= MIN_VOL && amount <= MAX_VOL)
        {
            Debug.Log("Master Volume set to the value of " + amount);
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, amount);
        }
        else
        {
            Debug.LogError("Master Volume is Out of Range");
        }
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 0.9f);
    }

    public static void SetMusicVolume(float amount)
    {
        if (amount >= MIN_VOL && amount <= MAX_VOL)
        {
            Debug.Log("Music Volume set to the value of " + amount);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, amount);
        }
        else
        {
            Debug.LogError("Music Volume is Out of Range");
        }
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
    }

    public static void SetSFXVolume(float amount)
    {
        if (amount >= MIN_VOL && amount <= MAX_VOL)
        {
            Debug.Log("Sound Effects set to the value of " + amount);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, amount);
        }
        else
        {
            Debug.LogError("Sound Effects is Out of Range");
        }
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
    }

    public static void SetTutorialWatchedTo(bool state)
    {
        if (state == true)
            PlayerPrefs.SetInt(HAS_SEEN_TUTORIAL, 1);
        else
            PlayerPrefs.SetInt(HAS_SEEN_TUTORIAL, 0);

        Debug.Log("Set tutorial watched to " + state);
    }

    public static bool GetIfTutorialWatched()
    {
        int hasWatched = PlayerPrefs.GetInt(HAS_SEEN_TUTORIAL, 0);

        if (hasWatched == 1)
            return true;
        else
            return false;
    }

    public static void SetLevelsUnlocked(int amount)
    {
        PlayerPrefs.SetInt(LEVELS_UNLOCKED, amount);

        Debug.Log("Set levels Unlocked to " + amount);
    }

    public static void ResetLevelsUnlocked()
    {
        SetLevelsUnlocked(1);
    }

    public static int GetLevelsUnlocked()
    {
        return PlayerPrefs.GetInt(LEVELS_UNLOCKED, 1);
    }
}
