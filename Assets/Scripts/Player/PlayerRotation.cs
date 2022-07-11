using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField]
    private InputProvider input;

    [SerializeField]
    private Rigidbody rb;

    [Min(0)]
    [SerializeField]
    private float rotationVelocity;

    private Quaternion targetRotation;

    private void Awake()
    {
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        targetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        var inputDir = input.GetMovementInput();
        if (inputDir.sqrMagnitude > 0.001)
            targetRotation = Quaternion.LookRotation(inputDir, Vector3.up);
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, 
            rotationVelocity * Time.fixedDeltaTime));
        rb.angularVelocity = Vector3.zero;
    }
}
