using Unity.Netcode;
using UnityEngine;

public class HostMovement : NetworkBehaviour
{
    public float movementSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
       
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        // Move the player
        transform.Translate(movement * movementSpeed * Time.deltaTime);
    }
}
