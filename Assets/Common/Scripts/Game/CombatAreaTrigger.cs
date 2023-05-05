using System.Collections;
using UnityEngine;

public class CombatAreaTrigger : MonoBehaviour
{
    [Header("Spawning Variables")]
    [SerializeField] int numberOfEnemiesToSpawn = 6;
    [SerializeField] float minSpawnDelay = 3f;
    [SerializeField] float maxSpawnDelay = 6f;
    [SerializeField] int scoreOnCompletion = 1000;

    [SerializeField] GameObject[] enemiesToInstantiate;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float cameraPositionBuffer = 13f;

    private float boxCollider2DWidthBuffer = 2f;
    private PlayerController player;
    private int currentEnemiesSpawned = 0;

    private void Update()
    {
        if (currentEnemiesSpawned >= numberOfEnemiesToSpawn && transform.childCount <= 0)
        {
            player.FinishCombatArea();
            GameSessionManager.instance.AddToScore(scoreOnCompletion);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<PlayerController>();
            player.LockInCombatArea(this.transform.position.x - cameraPositionBuffer - boxCollider2DWidthBuffer,
            this.transform.position.x + cameraPositionBuffer - boxCollider2DWidthBuffer);
            if (enemiesToInstantiate.Length != 0)
            {
                StartCoroutine(SpawnEnemies());
            }
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (currentEnemiesSpawned < numberOfEnemiesToSpawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            GameObject enemy = enemiesToInstantiate[Random.Range(0, enemiesToInstantiate.Length)];

            GameObject newTrash = Instantiate(enemy, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            newTrash.transform.parent = transform;
            currentEnemiesSpawned++;
        }
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
}
