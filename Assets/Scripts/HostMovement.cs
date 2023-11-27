
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class HostMovement : NetworkBehaviour
{
    private Rigidbody hostRb;
    private PlayerInput playerInput;
    private bool canJump = true;
    private bool isGrounded = true;
    private bool isSprinting = false;
    private bool isMoving = true;
    private bool canSprint = true;
    private float sprintCooldown = 5f;
    private float lookSensitivity = 100f;
    public float movementSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpPower = 5f;
    public float superJumpPower = 20f;
    public float attackPower = 5f;
    public float heavyAttackCooldown = 5f;
    public float superJumpCooldown = 5f;
    public GameObject pauseMenu;
    private bool isPaused = false;
    public static bool blockInput = false;
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener listener;
    public Transform playerCamera;



    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.enabled = true;
            vc.Priority = 1; 
        }
        else
        {
            vc.Priority = 0;
        }
    }
    private void Awake()
    {
        hostRb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void LateUpdate()
    {
        if (IsOwner)
        {
            HandleLookInput();
        } 
        
       
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
             HandleMovementInput();
        }
       
        
    }

    private void HandleLookInput()
    {
        if (!IsOwner) return;
        if (playerCamera == null) return;

        Vector2 lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        float horizontalRotation = lookInput.x * lookSensitivity * Time.fixedDeltaTime;
        float verticalRotation = lookInput.y * lookSensitivity * Time.fixedDeltaTime;

        transform.Rotate(Vector3.up, horizontalRotation);
        playerCamera.Rotate(Vector3.left, verticalRotation);
    }

    private void HandleMovementInput()
    {
        if (!IsOwner) return;
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();

        if (inputVector != Vector2.zero)
        {
            Vector3 forwardDirection = transform.forward;
            float currentSpeed = isSprinting ? runSpeed : movementSpeed;
            hostRb.AddForce(new Vector3(forwardDirection.x * inputVector.y, 0, forwardDirection.z * inputVector.y) * currentSpeed, ForceMode.Force);
            isMoving = true;
        }
        else
        {
            hostRb.velocity = Vector3.zero;
            isMoving = false;
            isSprinting = false;
        }
    }

    private void UpdateMovementSpeed()
    {
        float currentSpeed = isSprinting ? runSpeed : movementSpeed;
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();
        hostRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * currentSpeed, ForceMode.Force);
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        Debug.Log("Sprinting");
        if (context.performed)
        {
            isSprinting = true;
            UpdateMovementSpeed();
        }
        else if (context.canceled)
        {
            isSprinting = false;
            UpdateMovementSpeed();
        }

        canSprint = false;
        StartCoroutine(SprintCooldown());
    }

    private IEnumerator SprintCooldown()
    {
        yield return new WaitForSeconds(sprintCooldown);
        canSprint = false;
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        //player normal attack
    }
    public void HeavyAttack(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        //player heavy attack needs also cooldown
    }

    public void Block(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        //need to add the animation
        if (context.performed)
        {
            Debug.Log("BLOCKING");
            blockInput = true;
        }
        else 
        {
            Debug.Log("STOP BLOCKING");
            blockInput = false;
        }

       
    }
    public void AreaBlock(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        // no damage block that stops movement
    }
    public void TargetLock(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        //to lock on enemies
    }
    public void Eat(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        //eat dead bodies to regain hp
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!IsOwner)
        {
            return;
        }
        if (context.performed && isGrounded)
        {
            Debug.Log("Jumping");
            hostRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
        
    }

    public void SuperJump(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed && canJump && isGrounded)
        {
            Debug.Log("Super Jump");
            Vector3 jumpDirection = playerCamera.forward; // Use the forward direction of the player

            hostRb.AddForce(jumpDirection * superJumpPower, ForceMode.Impulse);

            canJump = false;
            StartCoroutine(SuperJumpCooldown());
        }
    }
        private IEnumerator SuperJumpCooldown()
    {       
        yield return new WaitForSeconds(superJumpCooldown);
        canJump = true;
    }
    // checking for collision instead of tags for ability to jump
    // Using collision as we have different ground types in the game (want to learn to use layers but this works for now)
    private void OnCollisionEnter(Collision collision) 
    {     
            isGrounded = true;   
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePause();
            Debug.Log("Paused " + context.phase);
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }

        // Switch action map
        playerInput.SwitchCurrentActionMap(isPaused ? "UI" : "Player-Host");
    }
}


