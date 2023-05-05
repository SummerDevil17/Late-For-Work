using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) GameSessionManager.instance.WinGame();
    }
}
