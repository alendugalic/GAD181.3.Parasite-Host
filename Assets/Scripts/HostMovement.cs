
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class HostMovement : NetworkBehaviour
{
    private Rigidbody hostRb;
    private PlayerInput playerInput;

    private bool canJump = true;
    private bool isGrounded = true;
    private bool isSprinting = false;
    private bool isMoving = true;
    private bool canSprint = true;
    private bool isPaused = false;
    public static bool blockInput = false;

    private float sprintCooldown = 5f;
    private float lookSensitivity = 100f;

    [Header("Swords")]
    [SerializeField] private Transform leftSword;
    [SerializeField] private Transform rightSword;

    [Header("Attack Parameters")]
    public float attackRange = 2.0f;

    [Header("Player Input Power")]
    public float movementSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpPower = 5f;
    public float superJumpPower = 20f;
    public float attackPower = 5f;
    public float heavyAttackPower = 10f;
    public float heavyAttackCooldown = 2f;
    public float superJumpCooldown = 5f;

    [Header("Player Fields")]
    public GameObject pauseMenu;
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener listener;
    [SerializeField] private Animator animator;
    public float interactionRadius = 2f;
    //[SerializeField] private LayerMask whatIsGround;
    //[SerializeField] private AnimationCurve animCurve;
    //[SerializeField] private float time;
    public Transform playerCamera;

    [Header("Player stamina")]
    public float currentStamina = 50f;
    public float maxStamina = 50f;
    public float staminaRegenRate = 5f; 

    private const float sprintStaminaCost = 5f;
    private const float heavyAttackStaminaCost = 10f;
    private const float superJumpStaminaCost = 15f;
    public static HostMovement LocalInstance { get; private set; }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.enabled = true;
            vc.Priority = 1; 
            LocalInstance = this;
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
        animator = GetComponent<Animator>();
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
        RegenerateStamina();
        //SurfaceAlignment();
        
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
            Vector3 rightDirection = transform.right;

            float currentSpeed = isSprinting ? runSpeed : movementSpeed;

            Vector3 movement = (forwardDirection * inputVector.y + rightDirection * inputVector.x).normalized;

            // Apply force for movement
            hostRb.AddForce(movement * currentSpeed, ForceMode.Force);
            isMoving = true;
            animator.SetBool("isWalking", true);
            
        }
        else
        {
            hostRb.velocity = Vector3.zero;
            isMoving = false;
            isSprinting = false;
            animator.SetBool("isWalking", false);
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
            if (CanConsumeStamina(sprintStaminaCost))
            {
                ConsumeStamina(sprintStaminaCost);
                isSprinting = true;
                UpdateMovementSpeed();
                animator.SetBool("isRunning", true);
            }
          
        }
        else if (context.canceled)
        {
            isSprinting = false;
            UpdateMovementSpeed();
            animator.SetBool("isRunning", false);
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
        if (context.performed)
        {
            // Trigger the attack
            PerformAttack();
            animator.SetBool("isAttacking", true);
            Debug.Log("Im Normal Attacking");
        }
        else if (context.canceled)
        {
            animator.SetBool("isAttacking", false);
            Debug.Log("Im Not Normal Attacking");
        }
    }

    private void PerformAttack()
    {
        if (leftSword != null && rightSword != null)
        {
            CheckForEnemies(leftSword);
            CheckForEnemies(rightSword);  
        }
        else
        {
            Debug.LogWarning("LeftSword or RightSword not assigned!");
        }
    }

    private void CheckForEnemies(Transform sword)
    {
        Collider[] hitColliders = Physics.OverlapSphere(sword.position, attackRange);

        foreach (Collider collider in hitColliders)
        {
            // Check if the collider belongs to an enemy
            Health enemyHealth = collider.GetComponent<Health>();

            if (enemyHealth != null)
            {
                // Deal damage to the enemy
                enemyHealth.TakeDamage(attackPower);
            }
        }
    }
    public void HeavyAttack(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            if (CanConsumeStamina(heavyAttackStaminaCost))
            {
                ConsumeStamina(heavyAttackStaminaCost);
                PerformHeavyAttack();  
                //StartCoroutine(HeavyAttackCooldown());
                animator.SetBool("isHeavyAttack", true);
            }             
        }
        else if (context.canceled)
        {
            animator.SetBool("isHeavyAttack", false);
        }
    }

    private void PerformHeavyAttack()
    {
        CheckForEnemies(leftSword);
        CheckForEnemies(rightSword); 
    }

    private IEnumerator HeavyAttackCooldown()
    {        
        yield return new WaitForSeconds(heavyAttackCooldown); 
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
            animator.SetBool("isJumping", true);
            Debug.Log("Jumping");
            hostRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
        else if (context.canceled)
        {
            animator.SetBool("isJumping", false);
        }
        
    }

    public void SuperJump(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed && canJump && isGrounded)
        {
            if (CanConsumeStamina(superJumpStaminaCost))
            {
                ConsumeStamina(superJumpStaminaCost);
                Debug.Log("Super Jump");
                Vector3 jumpDirection = playerCamera.forward; // Use the forward direction of the player

                hostRb.AddForce(jumpDirection * superJumpPower, ForceMode.Impulse);
                animator.SetBool("isSuperJumping", true);
                canJump = false;
                //StartCoroutine(SuperJumpCooldown());
            }
        }
        else if (context.canceled)
        {
            animator.SetBool("isSuperJumping", false);
        }
    }
        private IEnumerator SuperJumpCooldown()
    {       
        yield return new WaitForSeconds(superJumpCooldown);
        canJump = true;
    }
    //checking for collision instead of tags for ability to jump
    //Using collision as we have different ground types in the game(want to learn to use layers but this works for now)
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

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.fixedDeltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }

    private bool CanConsumeStamina(float cost)
    {
        return currentStamina >= cost;
    }

    private void ConsumeStamina(float cost)
    {
        currentStamina = Mathf.Clamp(currentStamina - cost, 0f, maxStamina);
    }
    //private void SurfaceAlignment()
    //{
    //    Ray ray = new Ray(transform.position, -transform.up);
    //    RaycastHit info = new RaycastHit();
    //    Quaternion rotationRef = Quaternion.Euler(0, 0, 0);
    //    if (Physics.Raycast(ray, out info, whatIsGround))
    //    {
    //        rotationRef = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, info.normal), animCurve.Evaluate(time));
    //        transform.rotation = Quaternion.Euler(rotationRef.eulerAngles.x, transform.rotation.y, rotationRef.eulerAngles.z);
    //    }
    //}
}


