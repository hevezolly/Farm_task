using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputProvider input;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    [Min(0)]
    private float moveAcceleration;

    [SerializeField]
    private ScriptableValueField<float> maxVelocity;

    [SerializeField]
    [Range(0, 1)]
    private float minMoveInputValue;

    [SerializeField]
    private ScriptableValue<float> currentLoad;

    [SerializeField]
    [Range(0, 1)]
    private float fullyLoadedSpeedPercent;

    private Vector3 targetVelocity;

    private float loadMultiplayer = 1;

    private void Update()
    {
        var movement = input.GetMovementInput();
        if (movement.magnitude < minMoveInputValue)
            movement = Vector3.zero;
        targetVelocity = movement * loadMultiplayer * maxVelocity.Value;
    }

    private void OnEnable()
    {
        OnLoadChanged(currentLoad.Value);
        currentLoad.ValueChangeEvent.AddListener(OnLoadChanged);
    }

    private void OnDisable()
    {
        currentLoad.ValueChangeEvent.RemoveListener(OnLoadChanged);
    }

    private void OnLoadChanged(float load)
    {
        loadMultiplayer = Mathf.Lerp(1, fullyLoadedSpeedPercent, Mathf.Clamp01(load));
    }

    private void FixedUpdate()
    {
        var currentVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        var velocityDelta = targetVelocity - currentVel;
        var acceleration = Mathf.Min(velocityDelta.magnitude / Time.deltaTime, moveAcceleration);
        rb.AddForce(velocityDelta.normalized * acceleration, ForceMode.Acceleration);
    }
}
