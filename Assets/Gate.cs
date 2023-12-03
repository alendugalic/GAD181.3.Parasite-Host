using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Gate : NetworkBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenGate()
    {
        // Set the "Open" trigger to true
        animator.SetBool("Open", true);
    }
}
