using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjustInputAxis : MonoBehaviour
{
    [SerializeField]
    private InputProvider provider;

    private void Awake()
    {
        
    }

    private void LateUpdate()
    {
        var forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        var right = Vector3.ProjectOnPlane(transform.right, Vector3.up);
        provider.SetCorrectedDirections(right, forward);
    }
}
