using UnityEngine;

public class WeaponPickUp : MonoBehaviour, IPickUp
{
    [SerializeField] int weaponHealth = 3;
    [SerializeField] int weaponDamage = 15;
    [SerializeField] float weaponThrowForce = 100f;
    [SerializeField] float weaponDropForce = 15f;

    [Header("Trigger Limits")]
    [SerializeField] float triggerMaxLimit = 1f;
    [SerializeField] float triggerMinLimit = 1f;

    [Header("SFX/ VFX Setup")]
    [SerializeField] AudioRandomizer hitSFX;
    [SerializeField] GameObject hitVFX;

    private Rigidbody2D weaponRB2D;
    private SpriteRenderer weaponSprite;
    private Animator weaponAnimator;
    private float playerFeetLocation;
    private PlayerController player;
    private bool hasBeenPickedUp = false;
    public Transform PickUpTransform { get => transform; }

    void Start()
    {
        weaponRB2D = GetComponent<Rigidbody2D>();
        weaponSprite = GetComponent<SpriteRenderer>();
        weaponAnimator = GetComponent<Animator>();
        weaponRB2D.gravityScale = 0f;
    }

    void Update()
    {
        if (!player) return;

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

        if (hasBeenPickedUp)
        {
            transform.position = transform.parent.position;
            if (player.IsFacingRight && weaponSprite.flipX) weaponSprite.flipX = false;
            else if (!player.IsFacingRight && !weaponSprite.flipX) weaponSprite.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (weaponRB2D.position.y <= playerFeetLocation + 0.1f)
        {
            weaponRB2D.gravityScale = 0f;
            weaponRB2D.velocity = Vector2.zero;
            weaponRB2D.MovePosition(weaponRB2D.position);

            GetComponent<BoxCollider2D>().isTrigger = true;

            weaponAnimator.SetTrigger("hit");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent<PlayerController>(out PlayerController player)) { player.ChangeHealth(-weaponDamage); }
        weaponAnimator.SetTrigger("hit");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenPickedUp) return;

        if (other.TryGetComponent<PlayerController>(out player))
        {
            if (!player.IsHoldingObject)
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
        if (hasBeenPickedUp) return;

        if (other.TryGetComponent<PlayerController>(out PlayerController isPlayer))
        {
            GameSessionManager.instance.DisableButton(this.gameObject);
            if (!player.IsHoldingObject)
                player.SetUpObjectToInteract(null);
            player = null;
        }
    }

    public void PickUp(Transform player)
    {
        hasBeenPickedUp = true;
        GameSessionManager.instance.DisableButton(this.gameObject);

        transform.parent = player.GetComponentsInChildren<Transform>()[1];
    }

    public void Throw()
    {
        transform.parent = null;
        playerFeetLocation = player.gameObject.transform.position.y;
        weaponAnimator.SetTrigger("throw");

        weaponRB2D.gravityScale = 2f;
        if (player.IsFacingRight)
            weaponRB2D.AddForce(new Vector2(weaponThrowForce, 0f));
        else
            weaponRB2D.AddForce(new Vector2(-weaponThrowForce, 0f));

        GetComponent<BoxCollider2D>().isTrigger = false;
        hasBeenPickedUp = false;
        player = null;
    }

    public void Drop()
    {
        transform.parent = null;
        playerFeetLocation = player.gameObject.transform.position.y;

        weaponRB2D.gravityScale = 2f;
        if (player.IsFacingRight)
            weaponRB2D.AddForce(new Vector2(weaponDropForce, 0f));
        else
            weaponRB2D.AddForce(new Vector2(-weaponDropForce, 0f));
        hasBeenPickedUp = false;
        player = null;
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
