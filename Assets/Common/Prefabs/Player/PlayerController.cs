using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Values")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;

    [Header("Player Component References")]

    //Player Input Setup
    private Animator playerSpriteAnimator;
    private Rigidbody2D playerRB2D;
    private PlayerInput playerInput;
    private InputDeviceDescription currentInputDevice;

    //Player Movement private values
    private Vector2 currentInputVector2;

    void Start()
    {
        playerSpriteAnimator = GetComponent<Animator>();
        playerRB2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Time.timeScale == 0))
        { //Doesn't animate if timeScale is 0
            AnimatePlayer();
        }
        MovePlayer();
    }

    public void OnMove(InputAction.CallbackContext inputValue)
    {
        currentInputVector2 = inputValue.ReadValue<Vector2>();

        CheckIfDeviceChanged(inputValue);
        HandleDeviceSpecificInputs();
    }

    public void OnJump(InputAction.CallbackContext inputValue)
    {
        //playerCharController.Move(new Vector3(0, movementSpeed, 0));
    }

    private void MovePlayer()
    {
        playerRB2D.AddForce(new Vector2(currentInputVector2.x * Time.deltaTime * movementSpeed, 0));
    }

    private void AnimatePlayer()
    {

    }

    private void CheckIfDeviceChanged(InputAction.CallbackContext inputValue)
    {
        if (currentInputDevice != inputValue.control.device.description)
            currentInputDevice = inputValue.control.device.description;
    }

    private void HandleDeviceSpecificInputs()
    {
        if (currentInputDevice.deviceClass == "Gamepad")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
