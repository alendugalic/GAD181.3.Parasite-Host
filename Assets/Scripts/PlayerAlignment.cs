using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAlignment : NetworkBehaviour
{

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SurfaceAlignment();
    }

    private void SurfaceAlignment()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit info = new RaycastHit();
        Quaternion rotationRef = Quaternion.Euler(0, 0, 0);
        if (Physics.Raycast(ray, out info, whatIsGround))
        {
            rotationRef = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, info.normal), animCurve.Evaluate(time));
            transform.rotation = Quaternion.Euler(rotationRef.eulerAngles.x, transform.rotation.y, rotationRef.eulerAngles.z);
        }
    }
}
