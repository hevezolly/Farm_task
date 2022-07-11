using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input Provider/input selector")]
public class InputSelector : InputProvider
{
    [SerializeField]
    private InputProvider keyboardInput;
    [SerializeField]
    private InputProvider joystickInput;

    private InputProvider selectedInput
    {
        get
        {
#if UNITY_EDITOR
            return keyboardInput;
#endif
            return joystickInput;
        }
    }
    public override void SetCorrectedDirections(Vector3 right, Vector3 forward)
    {
        selectedInput.SetCorrectedDirections(right, forward);
    }
    public override Vector3 GetMovementInput()
    {
        return selectedInput.GetMovementInput();
    }
}
