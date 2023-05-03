using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
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

    [Header("Player Stats Values")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float punchDamage = 10f;
    [SerializeField] float kickDamage = 20f;
    [SerializeField] float timeInvincible = 2f;

    [Header("Player Touch Joystick References")]
    [SerializeField] RectTransform baseTouchJoystick;
    [SerializeField] RectTransform midTouchJoystick;
    [SerializeField] RectTransform tipTouchJoystick;

    //Player Variable Setup
    private Animator playerAnimator;
    private Rigidbody2D playerRB2D;
    private float currentHealth, invincibilityTimer;

    //Player Movement private values
    private Vector2 currentInputVector2;
    private float jumpingTimer = 0f;
    private float startingJumpY;
    private float lowestXValueInLevel, highestXValueInLevel;

    //Player Touch private variables
    private Finger movementFinger;

    //Player States Bool Checks
    private bool isDead = false, isInvincible = false;
    private bool isJumping = false, isGrounded = true, isFacingRight = true;
    private bool isInCombatArea = false;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRB2D = GetComponent<Rigidbody2D>();

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
            if (jumpingTimer >= timeToKeepJumping) { isJumping = false; playerRB2D.gravityScale = gravityScale; }
        }
        else if (!isGrounded && playerRB2D.position.y <= startingJumpY + 0.08f)
        {
            isGrounded = true; playerRB2D.gravityScale = 0f;

            if (playerRB2D.position.y <= lowestYValueInLevel)
                playerRB2D.position += Vector2.up * 0.25f;
            else if (playerRB2D.position.y >= highestYValueInLevel)
                playerRB2D.position += -Vector2.up * 0.25f;
        }

        AnimatePlayer();
    }

    void FixedUpdate()
    {
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

        isJumping = true;
        isGrounded = false;
        jumpingTimer = 0f;

        startingJumpY = transform.position.y;
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible) return;

            isInvincible = true;
            invincibilityTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        GameSessionManager.instance.UpdateHealthBar(currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            GameSessionManager.instance.LoseGame();
        }
    }

    public void LockInCombatArea(float lowestX, float highestX)
    {
        lowestXValueInLevel = lowestX;
        highestXValueInLevel = highestX;

        isInCombatArea = true;
        playerFollowCam.Follow = null;
    }

    public void FinishCombatArea()
    {
        isInCombatArea = false;
        playerFollowCam.Follow = this.transform;
    }

    private void AnimatePlayer()
    {

    }


    #region Handle Touch Input With the Input System
    private void HandleFingerDown(Finger fingerUsed)
    {
        if (movementFinger == null) return;
    }

    private void HandleFingerMove(Finger fingerDragged)
    {

    }

    private void HandleFingerUp(Finger fingerLift)
    {

    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerMove += HandleFingerMove;
        ETouch.Touch.onFingerUp += HandleFingerUp;
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
    }

    #endregion
}
