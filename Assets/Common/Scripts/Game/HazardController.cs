using UnityEngine;

public class HazardController : MonoBehaviour
{
    [SerializeField] int hazardDamage = 10;
    [SerializeField] float triggerMaxLimit = 1.15f;
    [SerializeField] float triggerMinLimit = 1f;

    [Header("SFX/ VFX Setup")]
    [SerializeField] AudioRandomizer hitSFX;
    [SerializeField] GameObject hitVFX;


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (player.IsDead) return;

            if (player.transform.position.y <= transform.position.y + triggerMaxLimit &&
                player.transform.position.y >= transform.position.y - triggerMinLimit)
            {
                if (!player.IsInvincible)
                {
                    player.ChangeHealth(-hazardDamage);
                    hitSFX.PlayClip();
                    Instantiate(hitVFX, player.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                }
            }
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
