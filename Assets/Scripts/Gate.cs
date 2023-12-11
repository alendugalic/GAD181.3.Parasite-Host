using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Gate : NetworkBehaviour
{
    private Animator animator;
    private AudioSource audioSource; // Declare AudioSource variable

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Assign AudioSource component
    }

    public void OpenGate()
    {
        // Set the "Open" trigger to true
        animator.SetBool("isOpen", true);

        // Play the sound when the gate opens
        if (audioSource != null)
        {
            audioSource.Play();
        }

        Debug.Log("Gate Open");
    }
}
