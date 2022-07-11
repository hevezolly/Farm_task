using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMovementReaction : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float stopThreshold;

    [SerializeField]
    private ScriptableValue<float> maxVelocity;
    [SerializeField]
    private ScriptableValue<float> currentLoad;

    private int isMovingId;
    private int velocityId;
    private int loadId;
    private void Awake()
    {
        velocityId = Animator.StringToHash("speed");
        isMovingId = Animator.StringToHash("isWalking");
        loadId = Animator.StringToHash("load");
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
        animator.SetFloat(loadId, Mathf.Clamp01(load));
    }

    private void LateUpdate()
    {
        var speed = rb.velocity.magnitude;
        animator.SetFloat(velocityId, Mathf.Min(speed / maxVelocity.Value, 1));
        animator.SetBool(isMovingId, speed > stopThreshold);
    }
}
