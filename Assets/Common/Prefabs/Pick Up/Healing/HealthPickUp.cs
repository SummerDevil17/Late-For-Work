using UnityEngine;

public class HealthPickUp : MonoBehaviour, IPickUp
{
    [SerializeField] int healthToRestore = 10;

    [Header("Trigger Limits")]
    [SerializeField] float triggerMaxLimit = 1f;
    [SerializeField] float triggerMinLimit = 1f;
    [SerializeField] GameObject healVFX;

    private PlayerController player;

    public Transform PickUpTransform { get => transform; }

    void Update()
    {
        if (!player || player.IsHoldingObject || !player.CanHeal) return;

        if (IsWithingLimits())
        {
            GameSessionManager.instance.EnableButton(this.gameObject);
            player.SetUpObjectToInteract(this.gameObject);
        }
        else
        {
            GameSessionManager.instance.DisableButton(this.gameObject);
            if (!player.IsHoldingObject)
            {
                player.SetUpObjectToInteract(null);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out player))
        {
            if (player.CanHeal && !player.IsHoldingObject)
            {
                if (IsWithingLimits())
                {
                    GameSessionManager.instance.EnableButton(this.gameObject);
                    player.SetUpObjectToInteract(this.gameObject);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController isPlayer))
        {
            GameSessionManager.instance.DisableButton(this.gameObject);
            if (!player.IsHoldingObject)
            {
                player.SetUpObjectToInteract(null);
            }
            player = null;
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

    private bool IsWithingLimits()
    {
        return player.transform.position.y <= transform.position.y + triggerMaxLimit &&
               player.transform.position.y >= transform.position.y - triggerMinLimit;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Max line
        Gizmos.DrawLine(new Vector3(transform.position.x + 0.8f, transform.position.y + triggerMaxLimit, 0f),
                        new Vector3(transform.position.x - 0.8f, transform.position.y + triggerMaxLimit, 0f));

        Gizmos.color = Color.blue;
        //Min line
        Gizmos.DrawLine(new Vector3(transform.position.x + 0.8f, transform.position.y - triggerMinLimit, 0f),
                        new Vector3(transform.position.x - 0.8f, transform.position.y - triggerMinLimit, 0f));
    }
}
