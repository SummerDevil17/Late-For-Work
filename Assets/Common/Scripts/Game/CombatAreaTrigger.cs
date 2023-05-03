using System.Collections;
using UnityEngine;

public class CombatAreaTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToInstantiate;
    [SerializeField] float cameraPositionBuffer = 13f;

    private float boxCollider2DWidthBuffer = 2f;
    private PlayerController player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<PlayerController>();
            player.LockInCombatArea(this.transform.position.x - cameraPositionBuffer - boxCollider2DWidthBuffer,
            this.transform.position.x + cameraPositionBuffer - boxCollider2DWidthBuffer);
        }

        StartCoroutine(WaitForDebug());
    }

    void OnDrawGizmos()
    {
        Vector3 center = this.transform.position;

        Gizmos.color = Color.red;
        center.x = this.transform.position.x - cameraPositionBuffer - boxCollider2DWidthBuffer;
        Gizmos.DrawWireSphere(center, 0.5f);

        Gizmos.color = Color.yellow;
        center.x = this.transform.position.x + cameraPositionBuffer - boxCollider2DWidthBuffer;
        Gizmos.DrawWireSphere(center, 0.5f);
    }

    private IEnumerator WaitForDebug()
    {
        yield return new WaitForSeconds(3f);
        player.FinishCombatArea();
        Destroy(this.gameObject);
    }
}
