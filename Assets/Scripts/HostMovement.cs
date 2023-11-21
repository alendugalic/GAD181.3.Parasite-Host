
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class HostMovement : NetworkBehaviour
{
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 55,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone/*, NetworkVariableWritePermission.Owner*/);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }
    private Rigidbody hostRb;
    private PlayerInput playerInput;


    private bool canJump = true;

    public GameObject pauseMenu;
    private bool isGrounded = true;
    private bool isPaused = false;
    private bool isSprinting = false;
    private bool isMoving = false;
    private bool canSprint = true;
    private float sprintCooldown = 5f;
    public float lookSensitivity = 10f;

    [Header("HOST PRIMARY STATS")]

    [SerializeField]
    [Tooltip("On ground Movespeed")]
    [Range(0f, 100f)]
    public float movementSpeed = 5f;

    [SerializeField]
    [Tooltip("Host Run speed")]
    [Range(0f, 100f)]
    public float runSpeed = 10f;


    [SerializeField]
    [Tooltip("Normal jump power")]
    [Range(0f, 100f)]
    private float jumpPower = 5f;

    [SerializeField]
    [Tooltip("Super jump power")]
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

    //the transform lis used to spawn objects for both hold and client
    [SerializeField]
    private Transform hostDirections;
    private Vector2 lookInput = Vector2.zero;
    public Transform playerCamera;

    private void Awake()
    {
        hostRb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + newValue._bool);
        };
    }

    // Update is called once per frame
    void Update()
    {


        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            //how to spawn on both Host and client
            Transform hostDirectionsTransform = Instantiate(hostDirections);
            hostDirectionsTransform.GetComponent<NetworkObject>().Spawn(true);
            //use a normal destroy to despawn the object or use .Despawn


            randomNumber.Value = new MyCustomData
            {
                _bool = false,
                _int = 10,
                message = "forward",

            };
        }


    }

    private void LateUpdate()
    {
        lookInput = playerInput.actions["Look"].ReadValue<Vector2>();

        if (lookInput != Vector2.zero)
        {
            float horizontalRotation = lookInput.x * lookSensitivity * Time.fixedDeltaTime;
            float verticalRotation = lookInput.y * lookSensitivity * Time.fixedDeltaTime;

            transform.Rotate(Vector3.up, horizontalRotation);
            playerCamera.Rotate(Vector3.left, verticalRotation);
        }
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();

        if (inputVector != Vector2.zero)
        {


            Vector3 forwardDirection = transform.forward;

            hostRb.AddForce(new Vector3(forwardDirection.x * inputVector.y, 0, forwardDirection.z * inputVector.y) * movementSpeed, ForceMode.Force);
            isMoving = true;
        }
        else
        {
            hostRb.velocity = Vector3.zero;
            isMoving = false;
            isSprinting = false;
        }

    }

        public void Move(InputAction.CallbackContext context)
    {

        Debug.Log("I moved " + context.phase);
        Vector2 inputVector = context.ReadValue<Vector2>();
        Vector3 forwardDirection = transform.forward;
        hostRb.AddForce(new Vector3(forwardDirection.x * inputVector.y, 0, forwardDirection.z * inputVector.y)* movementSpeed, ForceMode.Force);
        isMoving = true;

    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Im running" + context.phase);
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

    private void UpdateMovementSpeed()
    {
        float currentSpeed = isSprinting ? runSpeed : movementSpeed;
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();
        hostRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * currentSpeed, ForceMode.Force);
    }
    private IEnumerator SprintCooldown()
    {
        Debug.Log("Sprint on Cooldown");
        yield return new WaitForSeconds(sprintCooldown);
        canSprint = false;
    }

   //The controls for the Host facing direction
    public void Look(InputAction.CallbackContext context)
    {
        if (playerCamera == null)
        {
            return;
        }

        Vector2 lookInput = context.ReadValue<Vector2>();
        float horizontalRotation = lookInput.x * lookSensitivity * Time.fixedDeltaTime;
        float verticalRotation = lookInput.y * lookSensitivity * Time.fixedDeltaTime;

        transform.Rotate(Vector3.up, horizontalRotation);
        playerCamera.Rotate(Vector3.left, verticalRotation);


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

            if (context.performed)
            {
                hostRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
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
        if (context.performed)
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
