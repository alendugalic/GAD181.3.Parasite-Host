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
   
    private float lookSensitivity = 100f;
    public float movementSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpPower = 5f;
    public float superJumpPower = 20f;
    public float attackPower = 5f;
    public float heavyAttackCooldown = 5f;
    public float superJumpCooldown = 5f;
    public Transform playerCamera;
    public GameObject pauseMenu;
    private bool isPaused = false;
    public static bool blockInput = false;



    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void LateUpdate()
    {
        if (!IsOwner)
        HandleLookInput();
      
    }

    private void FixedUpdate()
    {

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

    private void HandleMovementInput()
    {
       
    }

    private void UpdateMovementSpeed()
    {
    }

    public void Sprint(InputAction.CallbackContext context)
    {
       
    }

 
    public void Attack(InputAction.CallbackContext context)
    {
        //player normal attack
    }
    public void HeavyAttack(InputAction.CallbackContext context)
    {
        //player heavy attack needs also cooldown
    }

    public void Block(InputAction.CallbackContext context)
    {
       


    }
    public void AreaBlock(InputAction.CallbackContext context)
    {
        // no damage block that stops movement
    }
    public void TargetLock(InputAction.CallbackContext context)
    {
        //to lock on enemies
    }
    public void Eat(InputAction.CallbackContext context)
    {
        //eat dead bodies to regain hp
    }
    public void Jump(InputAction.CallbackContext context)
    {
       

    }

    public void SuperJump(InputAction.CallbackContext context)
    {
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


