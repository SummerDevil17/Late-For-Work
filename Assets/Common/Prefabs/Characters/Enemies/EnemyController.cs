using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Movement Values")]
    [SerializeField] float movementSpeed = 8f;

    [Header("Enemy Stats Values")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] int scoreValue = 165;
    [SerializeField] int attackDamage = 10;

    [Header("Enemy VFX References")]
    [SerializeField] VFXTrigger hitVFX;
    [SerializeField] GameObject landVFX;

    [Header("Enemy SFX References")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioRandomizer hitSFX;
    [SerializeField] AudioRandomizer hurtSFX;

    //Enemy Variable Setup
    private Animator enemyAnimator;
    private Rigidbody2D enemyRB2D;
    private SpriteRenderer enemySprite;
    private Transform targetTransform;
    private float currentHealth;

    //Enemy States Bool Checks
    private bool isDead = false;
    private bool isFacingRight = false;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyRB2D = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        targetTransform = FindObjectOfType<PlayerController>().transform;

        currentHealth = maxHealth;
    }

    void Update()
    {
        isFacingRight = transform.position.x < targetTransform.position.x ? true : false;

        AnimateEnemy();
    }

    void FixedUpdate()
    {

    }

    public void DamageWith(int amount)
    {
        if (isDead) return;

        enemyAnimator.SetTrigger("hit");
        hurtSFX.PlayClip();

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            enemyAnimator.SetTrigger("die");
            GameSessionManager.instance.AddToScore(scoreValue);
        }
    }

    private void AnimateEnemy() { }

    private void TryToHitPlayer(int damage)
    {
        RaycastHit2D hit = CastForHit();

        if (hit.collider.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.ChangeHealth(-damage);
            hitVFX.Trigger();
            hitSFX.PlayClip();
        }
    }

    private RaycastHit2D CastForHit()
    {
        Vector2 hitOrigin = new Vector2(hitVFX.transform.position.x, hitVFX.transform.position.y);

        if (isFacingRight)
            return Physics2D.Raycast(hitOrigin, Vector2.right, 1f);
        else
            return Physics2D.Raycast(hitOrigin, Vector2.left, 1f);
    }
}
