using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class HostMovement : NetworkBehaviour
{
    private Rigidbody hostRb;
    private PlayerInput playerInput;

    [Header("HOST PRIMARY STATS")]

    [SerializeField]
    [Tooltip("On ground Movespeed")]
    [Range(0f, 100f)]
    public float movementSpeed = 5f;

    [SerializeField]
    [Tooltip("Normal jump power (heavy jump is X2")]
    [Range(0f, 100f)]
    private float jumpPower = 5f;

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
        if (context.performed)
        {
            hostRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            Debug.Log("I jumped " + context.phase);
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
        if(context.performed)
        {
            hostRb.AddForce(Vector3.up * jumpPower * 4, ForceMode.Impulse);
        }
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
