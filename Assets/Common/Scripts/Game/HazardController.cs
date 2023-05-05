using UnityEngine;

public class HazardController : MonoBehaviour
{
    [SerializeField] int hazardDamage = 10;
    [SerializeField] float triggerMaxLimit = 1.15f;
    [SerializeField] float triggerMinLimit = 1f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (player.transform.position.y <= transform.position.y + triggerMaxLimit &&
                player.transform.position.y >= transform.position.y - triggerMinLimit)
                player.ChangeHealth(-hazardDamage);
        }
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
