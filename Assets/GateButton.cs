using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButton : MonoBehaviour
{
    // Reference to the gate script
    public Gate gate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // OnTriggerEnter is called when another collider enters the trigger zone
    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is a player
        if (other.CompareTag("Player"))
        {
            // Call the OpenGate method on the Gate script
            gate.OpenGate();
        }
    }
}
