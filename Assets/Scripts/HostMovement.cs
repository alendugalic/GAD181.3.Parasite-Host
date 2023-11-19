
using System;

using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class HostMovement : NetworkBehaviour
{
    private Rigidbody hostRb;
    private PlayerInput playerInput;

    private bool canJump = true;

    public GameObject pauseMenu;
    private bool isGrounded = true;
    private bool isPaused = false;


    [Header("HOST PRIMARY STATS")]

    [SerializeField]
    [Tooltip("On ground Movespeed")]
    [Range(0f, 100f)]
    public float movementSpeed = 5f;

    [SerializeField]
    [Tooltip("Normal jump power")]
    [Range(0f, 100f)]
    private float jumpPower = 5f;

    [SerializeField]
    [Tooltip("Super jump power (4x jump power)")]
    [Range(0f, 100f)]
    private float superJumpPower = 20f;

    [SerializeField]
    [Tooltip("Host attack power")]
    [Range(0f, 100f)]
    private float attackPower = 5f;

    [SerializeField]
    [Tooltip("Heavy attack cooldown")]
    [Range(0f, 100f)]
    private float heavyAttackCooldown = 5f;

    [SerializeField]
    [Tooltip("Super jump cooldown")]
    [Range(0f, 100f)]
    private float superJumpCooldown = 5f;

    private void Awake()
    {
        hostRb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;



    }

    private void FixedUpdate()
    {



        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();
        hostRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * movementSpeed, ForceMode.Force);

       

    }

    public void Move(InputAction.CallbackContext context)
    {

        Debug.Log("I moved " + context.phase);
        Vector2 inputVector = context.ReadValue<Vector2>();
        hostRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * movementSpeed, ForceMode.Force);

    }
    public void Sprint(InputAction.CallbackContext context)
    {

    }
    //The controls for the Host facing direction
    public void Facing(InputAction.CallbackContext context)
    {

    }
    public void Jump(InputAction.CallbackContext context)
    {

        if (context.performed && canJump)
        {
            hostRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            canJump = false;

            if (context.performed && isGrounded)
            {
                hostRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                isGrounded = false;

                Debug.Log("I jumped " + context.phase);
            }

        }
       
    }
    public void Attack(InputAction.CallbackContext context)
    {

    }
    public void HeavyAttack(InputAction.CallbackContext context)
    {

    }
    public void SuperJump(InputAction.CallbackContext context)
    {

        if (context.performed && canJump)

            if (context.performed && isGrounded)

            {
                Vector3 mousePosition = Mouse.current.position.ReadValue();
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 jumpDirection = (hit.point - transform.position).normalized;
                    hostRb.AddForce(jumpDirection * superJumpPower, ForceMode.Impulse);


                    canJump = false;
                    StartCoroutine(SuperJumpCooldown());

                    isGrounded = false;
                    StartCoroutine(SuperJumpCooldown());

                }
            }

    }

    private IEnumerator SuperJumpCooldown()
    {
        yield return new WaitForSeconds(superJumpCooldown);

        canJump = true;
    }

    // You may need to set canJump to true when the player touches the ground.
    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }

    public void Block(InputAction.CallbackContext context)
    {

    }
    public void AreaBlock(InputAction.CallbackContext context)
    {

    }
    public void TargetLock(InputAction.CallbackContext context)
    {

    }
    public void Eat(InputAction.CallbackContext context)
    {

    }
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isPaused)
            {
                // Resume the game
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
            }
            else
            {
                // Pause the game
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
            }

            isPaused = !isPaused;

            // Switch action map
            if (playerInput.currentActionMap.name == "Player-Host")
            {
                playerInput.SwitchCurrentActionMap("UI");
            }
            else if (playerInput.currentActionMap.name == "UI")
            {
                playerInput.SwitchCurrentActionMap("Player-Host");
            }

            Debug.Log("Paused " + context.phase);
        }


    }
}
