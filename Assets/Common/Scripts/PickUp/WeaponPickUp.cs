using UnityEngine;

public class WeaponPickUp : MonoBehaviour, IPickUp
{
    [SerializeField] int weaponHealth = 3;
    [SerializeField] float weaponThrowForce = 6f;

    private Rigidbody2D weaponRB2D;
    private bool hasBeenPickedUp = false;
    public Transform PickUpTransform { get => transform; }

    void Start()
    {
        weaponRB2D = GetComponent<Rigidbody2D>();
        weaponRB2D.gravityScale = 0f;
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
            player.SetUpObjectToInteract(null);
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

        weaponRB2D.gravityScale = 1f;
        weaponRB2D.AddForce(new Vector2(weaponThrowForce, 0f));

        hasBeenPickedUp = false;
    }
}
