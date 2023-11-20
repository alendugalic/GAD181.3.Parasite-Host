using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParasiteInstruct : NetworkBehaviour
{
    

    //need to attatch networkObject to the inspector
    //also add to network prefabs
    // instantiate it in the script

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            _int = 55,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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
    private PlayerInput playerInput;

    public GameObject pauseMenu;

    private bool isPaused = false;

    [SerializeField]
    private Transform hostDirections;


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

    private void Awake()
    {
       
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
            Instantiate(hostDirections);
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
    public void Movement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           
        }
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
        

    }
    public void Attack(InputAction.CallbackContext context)
    {

    }
    public void HeavyAttack(InputAction.CallbackContext context)
    {

    }
    public void SuperJump(InputAction.CallbackContext context)
    {

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
       

    }
}


