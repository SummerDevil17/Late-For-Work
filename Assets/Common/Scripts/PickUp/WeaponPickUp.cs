using UnityEngine;

public class WeaponPickUp : MonoBehaviour, IPickUp
{
    [SerializeField] int weaponHealth = 3;
    [SerializeField] int weaponDamage = 15;
    [SerializeField] float weaponThrowForce = 100f;
    [SerializeField] float weaponDropForce = 15f;

    private Rigidbody2D weaponRB2D;
    private float playerFeetLocation;
    private bool hasBeenPickedUp = false;
    public Transform PickUpTransform { get => transform; }

    void Start()
    {
        weaponRB2D = GetComponent<Rigidbody2D>();
        weaponRB2D.gravityScale = 0f;
    }

    void Update()
    {
        if (hasBeenPickedUp) transform.position = transform.parent.position;
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
    }

    public void Throw(PlayerController player)
    {
        transform.parent = null;
        playerFeetLocation = player.gameObject.transform.position.y;

        weaponRB2D.gravityScale = 1f;
        if (player.IsFacingRight)
            weaponRB2D.AddForce(new Vector2(weaponThrowForce, 0f));
        else
            weaponRB2D.AddForce(new Vector2(-weaponThrowForce, 0f));

        GetComponent<BoxCollider2D>().isTrigger = false;
        hasBeenPickedUp = false;
    }

    public void Drop(PlayerController player)
    {
        transform.parent = null;
        playerFeetLocation = player.gameObject.transform.position.y;

        weaponRB2D.gravityScale = 1f;
        if (player.IsFacingRight)
            weaponRB2D.AddForce(new Vector2(weaponDropForce, 0f));
        else
            weaponRB2D.AddForce(new Vector2(-weaponDropForce, 0f));
        hasBeenPickedUp = false;
    }
}
