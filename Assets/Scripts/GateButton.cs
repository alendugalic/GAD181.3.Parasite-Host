using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class GateButton : MonoBehaviour
{
    public Gate gate;
    private AudioSource audioSource; // Declare AudioSource variable

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Assign AudioSource component
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player touched button");
            gate.OpenGate();

            // Play the sound when the player enters the trigger
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
