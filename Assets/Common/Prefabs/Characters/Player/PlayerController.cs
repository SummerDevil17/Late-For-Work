using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Follow Camera")]
    [SerializeField] CinemachineVirtualCamera playerFollowCam;

    [Header("Player Movement Values")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpForce = 250f;
    [SerializeField] float timeToKeepJumping = 1f;
    [SerializeField] float gravityScale = 15f;
    [SerializeField] float highestYValueInLevel = 1.5f;
    [SerializeField] float lowestYValueInLevel = -7.5f;

    [Header("Player Animation Speed Values")]
    [SerializeField] float pickUpAnimationHold = 0.35f;

    [Header("Player Stats Values")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float punchDamage = 10f;
    [SerializeField] float kickDamage = 20f;
    [SerializeField] float timeInvincible = 2f;

    [Header("Player VFX References")]
    [SerializeField] VFXTrigger hitVFX;

    [Header("Player SFX References")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip hurtSFX;


    //Player Variable Setup
    private Animator playerAnimator;
    private Rigidbody2D playerRB2D;
    private SpriteRenderer playerSprite;
    private float currentHealth, invincibilityTimer;

    //Player Movement private values
    private Vector2 currentInputVector2;
    private float jumpingTimer = 0f;
    private float startingJumpY;
    private float lowestXValueInLevel, highestXValueInLevel;

    //Player States Bool Checks
    private bool isDead = false, isInvincible = false;
    private bool isJumping = false, isGrounded = true, isFacingRight = true, isAnimating = false;
    private bool isInCombatArea = false, isHoldingObject = false, canHeal = false;

    //Player Interaction With Objects
    private GameObject objectToPickUp = null;

    private CinemachineFramingTransposer followTransposer;

    public bool CanHeal { get => canHeal; }
    public bool IsHoldingObject { get => isHoldingObject; set => isHoldingObject = value; }
    public bool IsFacingRight { get => isFacingRight; }
    public bool IsGrounded { get => isGrounded; }
    public float StartingJumpY { get => startingJumpY; }

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRB2D = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        followTransposer = playerFollowCam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineFramingTransposer;

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;

        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            if (invincibilityTimer <= 0) isInvincible = false;
        }
        if (isJumping)
        {
            jumpingTimer += Time.deltaTime;
            if (jumpingTimer >= timeToKeepJumping)
            {
                isJumping = false;
                playerRB2D.gravityScale = gravityScale;
                playerAnimator.SetTrigger("land");
            }
        }
        else if (!isGrounded && playerRB2D.position.y <= startingJumpY + 0.08f)
        {
            playerRB2D.gravityScale = 0f;
            playerRB2D.velocity = Vector2.zero;

            if (playerRB2D.position.y <= lowestYValueInLevel)
                playerRB2D.position += Vector2.up * 0.25f;
            else if (playerRB2D.position.y >= highestYValueInLevel)
                playerRB2D.position += -Vector2.up * 0.25f;
        }

        AnimatePlayer();
    }

    void FixedUpdate()
    {
        if (isAnimating) return;

        float forceInY = currentInputVector2.y;

        if (isJumping)
        {
            playerRB2D.AddForce(Vector2.up * jumpForce);
            forceInY = 0f;
        }
        else if (!isGrounded) forceInY = 0f;

        Vector2 position = playerRB2D.position;
        position += new Vector2(currentInputVector2.x, forceInY) * movementSpeed * Time.deltaTime;

        if (position.y <= lowestYValueInLevel || position.y >= highestYValueInLevel)
            position.y = playerRB2D.position.y;

        if (isInCombatArea && (position.x <= lowestXValueInLevel || position.x >= highestXValueInLevel))
            position.x = playerRB2D.position.x;

        playerRB2D.MovePosition(position);
    }


    public void OnMove(InputAction.CallbackContext inputValue)
    {
        if (isDead) return;

        currentInputVector2 = inputValue.ReadValue<Vector2>();
    }

    public void OnJump()
    {
        if (isDead || !isGrounded) return;

        if (isHoldingObject)
        {
            objectToPickUp.GetComponent<WeaponPickUp>().Drop();
            isHoldingObject = false;
            playerAnimator.SetBool("isHolding", false);

            GameSessionManager.instance.DisableButton(this.gameObject);
        }

        isJumping = true;
        isGrounded = false;
        jumpingTimer = 0f;

        playerAnimator.SetTrigger("jump");
        playerAnimator.SetBool("isGrounded", false);
        playerAnimator.SetLayerWeight(1, 1f);

        startingJumpY = playerRB2D.position.y;
    }

    public void OnPickUp() { if (isDead) return; StartCoroutine(PickUp()); }

    public void OnThrow()
    {
        if (isDead) return;

        if (isHoldingObject)
        {
            playerAnimator.SetTrigger("throw");
            objectToPickUp.GetComponent<WeaponPickUp>().Throw();

            GameSessionManager.instance.DisableButton(this.gameObject);
        }
    }

    public void OnPunch()
    {
        if (isDead || isAnimating) return;
        if (!isGrounded) playerAnimator.SetTrigger("jumpAttack");
        else playerAnimator.SetTrigger("punch");
    }
    public void OnKick()
    {
        if (isDead || isAnimating) return;
        if (!isGrounded) playerAnimator.SetTrigger("jumpAttack");
        else playerAnimator.SetTrigger("kick");
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return;

        if (amount < 0)
        {
            if (isInvincible) return;

            isInvincible = true;
            invincibilityTimer = timeInvincible;

            playerAnimator.SetTrigger("hit");
            sfxSource.PlayOneShot(hurtSFX);

            if (isHoldingObject)
            {
                objectToPickUp.GetComponent<WeaponPickUp>().Drop();
                isHoldingObject = false;
                playerAnimator.SetBool("isHolding", false);

                GameSessionManager.instance.DisableButton(this.gameObject);
            }
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        GameSessionManager.instance.UpdateHealthBar(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            playerAnimator.SetTrigger("die");
            currentInputVector2 = Vector2.zero;
            GameSessionManager.instance.LoseGame();
        }
        else if (currentHealth < maxHealth) { canHeal = true; }
        else if (currentHealth == maxHealth) { canHeal = false; }
    }

    public void LockInCombatArea(float lowestX, float highestX)
    {
        lowestXValueInLevel = lowestX;
        highestXValueInLevel = highestX;

        isInCombatArea = true;

        followTransposer.m_DeadZoneWidth = 1f;
        followTransposer.m_SoftZoneWidth = 2f;
    }

    public void FinishCombatArea()
    {
        isInCombatArea = false;

        followTransposer.m_DeadZoneWidth = 0f;
        followTransposer.m_SoftZoneWidth = 0.3f;
    }

    public void SetUpObjectToInteract(GameObject objectSent) { objectToPickUp = objectSent; }

    #region Animation Event Calls
    public void ResetJumpLayer() { playerAnimator.SetLayerWeight(1, 0f); isGrounded = true; }
    public void TriggerAnimating() { isAnimating = true; }
    public void CancelAnimating() { isAnimating = false; }
    public void StopHoldingObject() { isHoldingObject = false; }

    #endregion

    private void AnimatePlayer()
    {
        if (isAnimating) return;

        if (isGrounded) playerAnimator.SetBool("isGrounded", true);

        if (!isHoldingObject) playerAnimator.SetBool("isHolding", false);

        if (currentInputVector2.x > 0f) isFacingRight = true;
        else if (currentInputVector2.x < 0f) isFacingRight = false;

        if (isFacingRight && playerSprite.flipX) { playerSprite.flipX = false; }
        else if (!isFacingRight && !playerSprite.flipX) { playerSprite.flipX = true; }

        playerAnimator.SetFloat("movementInput", currentInputVector2.magnitude);
    }

    private IEnumerator PickUp()
    {
        if (isHoldingObject) yield return null;

        playerAnimator.SetTrigger("pickUp");
        isAnimating = true;

        yield return new WaitForSeconds(pickUpAnimationHold);

        isAnimating = false;
        if (objectToPickUp && objectToPickUp.TryGetComponent<IPickUp>(out IPickUp pickUp))
        {
            pickUp.PickUp(this.transform);

            if (objectToPickUp && objectToPickUp.TryGetComponent<WeaponPickUp>(out WeaponPickUp weapon))
            {
                isHoldingObject = true;
                playerAnimator.SetBool("isHolding", true);

                GameSessionManager.instance.EnableButton(this.gameObject);
            }
        }
    }
}
