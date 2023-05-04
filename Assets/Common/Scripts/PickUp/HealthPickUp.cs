using UnityEngine;

public class HealthPickUp : MonoBehaviour, IPickUp
{
    [SerializeField] int healthToRestore = 10;

    public Transform PickUpTransform { get => transform; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.ChangeHealth(-10);

            if (player.CanHeal && !player.IsHoldingObject)
            {
                GameSessionManager.instance.EnableButton(this.gameObject);
                player.SetUpObjectToInteract(this.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            GameSessionManager.instance.DisableButton(this.gameObject);
            if (!player.IsHoldingObject)
                player.SetUpObjectToInteract(null);
        }
    }

    public void PickUp(Transform player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.ChangeHealth(healthToRestore);
        playerController.IsHoldingObject = false;

        GameSessionManager.instance.DisableButton(this.gameObject);
        Destroy(this.gameObject);
    }
}
