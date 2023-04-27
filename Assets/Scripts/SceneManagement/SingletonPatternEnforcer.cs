using UnityEngine;

public class SingletonPatternEnforcer : MonoBehaviour
{
    private void Awake()
    {
        int persistentObject = FindObjectsOfType<SingletonPatternEnforcer>().Length;

        if (persistentObject > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
