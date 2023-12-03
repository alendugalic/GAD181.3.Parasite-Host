using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class GateButton : MonoBehaviour
{
    public Gate associatedGate;
    private bool isActivated = false;

    public bool CanInteract()
    {
        return !isActivated;
    }

    public void OpenDoor()
    {
        if (associatedGate != null)
        {
            associatedGate.OpenGate();
            isActivated = true;
        }
    }
}
