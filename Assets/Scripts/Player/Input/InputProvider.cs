using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputProvider : ScriptableObject
{
    [SerializeField]
    protected Vector3 rightDirection = Vector3.right;
    [SerializeField]
    protected Vector3 forwardDirection = Vector3.forward;

    public virtual void SetCorrectedDirections(Vector3 right, Vector3 forward)
    {
        rightDirection = right;
        forwardDirection = forward;
    }

    public abstract Vector3 GetMovementInput();
}
