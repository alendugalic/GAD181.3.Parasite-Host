using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParasiteInstruct : NetworkBehaviour
{
   
 
    private PlayerInput playerInput;

    public GameObject pauseMenu;

    private bool isPaused = false;

   

    // need to change all of these names to new inputs
    [Header("Parasite PRIMARY STATS")]

    [SerializeField]
    [Tooltip("On ground Movespeed")]
    [Range(0f, 100f)]
    public float movementSpeed = 5f;

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

    private Vector2 lookInput = Vector2.zero;
    public Transform playerCamera;
    public float lookSensitivity = 10f;

    private void Awake()
    {
       
        playerInput = GetComponent<PlayerInput>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        
      

        if (Input.GetKeyDown(KeyCode.P))
        {
            //Instantiate(hostDirections);
            //randomNumber.Value = new MyCustomData
            //{
            //    _bool = false,
            //    _int = 10,
            //    message = "ROARRRRR!!!"
               
            //};
        }


    }

    private void FixedUpdate()
    {
        
    }
    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        HandleLookInput();
    }
    public void MoveInstructions(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           // movement instructions for the host
        }
    }
    public void TargetMark(InputAction.CallbackContext context)
    {
        //mark target for host to see
    }

    private void HandleLookInput()
    {
        if (playerCamera == null) return;

        Vector2 lookInput = playerInput.actions["LookParasite"].ReadValue<Vector2>();
        float horizontalRotation = lookInput.x * lookSensitivity * Time.fixedDeltaTime;
        float verticalRotation = lookInput.y * lookSensitivity * Time.fixedDeltaTime;

        transform.Rotate(Vector3.up, horizontalRotation);
        playerCamera.Rotate(Vector3.left, verticalRotation);
    }
    
    public void SlowDown(InputAction.CallbackContext context)
    {
        //tell host to slow down
    }
    public void Run(InputAction.CallbackContext context)
    {
        //tell host to run
    }
    public void VisionChangeF(InputAction.CallbackContext context)
    {
        //different vision type
    }
    public void VisionChangeB(InputAction.CallbackContext context)
    {
        //Different vision type
    }

    public void MarkerOne(InputAction.CallbackContext context)
    {
        //save current location on map Different color
    }
    public void MarkerTwo(InputAction.CallbackContext context)
    {
        // save current location on map Different color
    }
    public void Pause(InputAction.CallbackContext context)
    {
       

    }
}


