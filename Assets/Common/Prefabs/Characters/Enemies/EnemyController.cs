using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Movement Values")]
    [SerializeField] float movementSpeed = 8f;

    [SerializeField] float stopDistance = 1.5f;

    [Header("Enemy Feet Reference")]
    [SerializeField] GameObject feetCollider;

    [Header("Enemy Stats Values")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] int scoreValue = 165;
    [SerializeField] int attackDamage = 10;

    [Header("Enemy VFX References")]
    [SerializeField] VFXTrigger hitVFX;

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
    private bool isAnimating = false, isFacingRight = false;

    public bool IsDead { get => isDead; }

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
        if (isDead) Destroy(this.gameObject, 4f);

        isFacingRight = transform.position.x < targetTransform.position.x ? true : false;

        AnimateEnemy();
    }

    void FixedUpdate()
    {
        if (isDead || isAnimating) return;

        Vector2 targetDistance = targetTransform.position - transform.position;

        float hForce = targetDistance.x / Mathf.Abs(targetDistance.x);
        float yForce = targetDistance.x / Mathf.Abs(targetDistance.x);

        if (Mathf.Abs(targetDistance.x) < stopDistance) { hForce = 0; }
        if (Mathf.Abs(targetDistance.y) < stopDistance) { yForce = 0; }

        enemyRB2D.velocity = new Vector2(hForce * movementSpeed, yForce * movementSpeed);

        PlayerController player = targetTransform.GetComponent<PlayerController>();
        enemyRB2D.position = new Vector2(enemyRB2D.position.x, Mathf.Clamp(enemyRB2D.position.y,
        player.lowestYValueInLevel, player.highestYValueInLevel));
    }

    public void DamageWith(int amount)
    {
        if (isDead || isAnimating) return;

        enemyAnimator.SetTrigger("hurt");
        hurtSFX.PlayClip();

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            enemyAnimator.SetTrigger("die");
            feetCollider.SetActive(false);
            GameSessionManager.instance.AddToScore(scoreValue);
        }
    }

    public void TriggerAnimating() { isAnimating = true; }
    public void CancelAnimating() { isAnimating = false; }

    private void AnimateEnemy()
    {
        if (isAnimating) return;

        if (isFacingRight && transform.localScale.x > 0) { transform.localScale = new Vector3(-1f, 1f, 1f); }
        else if (!isFacingRight && transform.localScale.x < 0) { transform.localScale = new Vector3(1f, 1f, 1f); }

        enemyAnimator.SetFloat("movementInput", enemyRB2D.velocity.magnitude);
    }

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
