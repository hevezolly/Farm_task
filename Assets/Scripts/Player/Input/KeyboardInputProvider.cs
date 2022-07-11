using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input Provider/Keyboard")]
public class KeyboardInputProvider : InputProvider
{
    public override Vector3 GetMovementInput()
    {
        var forward = Input.GetAxisRaw("Vertical") * forwardDirection;
        var right = Input.GetAxisRaw("Horizontal") * rightDirection;
        return (forward + right).normalized;
    }

}
