using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private RectTransform joystickTransform;

    [SerializeField]
    private float maxJoystickMovement;

    [SerializeField]
    private ExternalInputProvider input;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas.enabled = false;
    }

    private void MoveJoystickTo(Vector3 position)
    {
        var offset = position - rectTransform.position;
        var maxOffset = maxJoystickMovement * canvas.transform.localScale.x * transform.localScale.x;
        var magnitude = Mathf.Min(offset.magnitude, maxOffset);
        offset = offset.normalized * magnitude;
        input.SetInput(offset / maxOffset);
        joystickTransform.position = rectTransform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount <= 0)
            return;
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            canvas.enabled = true;
            rectTransform.position = touch.position;
            joystickTransform.position = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            MoveJoystickTo(touch.position);
        }
        else
        {
            input.SetInput(Vector2.zero);
            canvas.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, maxJoystickMovement);
    }
}
