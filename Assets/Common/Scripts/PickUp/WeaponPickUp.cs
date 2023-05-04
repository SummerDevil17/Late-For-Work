using UnityEngine;

public class WeaponPickUp : MonoBehaviour, IPickUp
{
    [SerializeField] int weaponHealth = 3;
    [SerializeField] int weaponDamage = 15;
    [SerializeField] float weaponThrowForce = 100f;
    [SerializeField] float weaponDropForce = 15f;

    private Rigidbody2D weaponRB2D;
    private SpriteRenderer weaponSprite;
    private Animator weaponAnimator;
    private float playerFeetLocation;
    private PlayerController playerController;
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
        if (hasBeenPickedUp)
        {
            transform.position = transform.parent.position;
            if (playerController.IsFacingRight && !weaponSprite.flipX) weaponSprite.flipX = false;
            else if (!playerController.IsFacingRight && weaponSprite.flipX) weaponSprite.flipX = true;
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
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent<PlayerController>(out PlayerController player)) { player.ChangeHealth(-weaponDamage); }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenPickedUp) return;

        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (!player.IsHoldingObject)
            {
                GameSessionManager.instance.EnableButton(this.gameObject);
                player.SetUpObjectToInteract(this.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (hasBeenPickedUp) return;

        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            GameSessionManager.instance.DisableButton(this.gameObject);
            if (!player.IsHoldingObject)
                player.SetUpObjectToInteract(null);
        }
    }

    public void PickUp(Transform player)
    {
        hasBeenPickedUp = true;
        GameSessionManager.instance.DisableButton(this.gameObject);

        transform.parent = player.GetComponentsInChildren<Transform>()[1];
        playerController = player.GetComponent<PlayerController>();
    }

    public void Throw()
    {
        transform.parent = null;
        playerFeetLocation = playerController.gameObject.transform.position.y;

        weaponRB2D.gravityScale = 2f;
        if (playerController.IsFacingRight)
            weaponRB2D.AddForce(new Vector2(weaponThrowForce, 0f));
        else
            weaponRB2D.AddForce(new Vector2(-weaponThrowForce, 0f));

        GetComponent<BoxCollider2D>().isTrigger = false;
        hasBeenPickedUp = false;
        playerController = null;
    }

    public void Drop()
    {
        transform.parent = null;
        playerFeetLocation = playerController.gameObject.transform.position.y;

        weaponRB2D.gravityScale = 2f;
        if (playerController.IsFacingRight)
            weaponRB2D.AddForce(new Vector2(weaponDropForce, 0f));
        else
            weaponRB2D.AddForce(new Vector2(-weaponDropForce, 0f));
        hasBeenPickedUp = false;
        playerController = null;
    }
}
