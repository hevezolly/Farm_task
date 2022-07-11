using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input Provider/external")]
public class ExternalInputProvider : InputProvider
{
    private Vector3 movement;
    public void SetInput(Vector2 input)
    {
        movement = forwardDirection * input.y + rightDirection * input.x;
    }
    public override Vector3 GetMovementInput()
    {
        return movement;
    }
}
