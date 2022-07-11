using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentGroundIntersectionPreventor : MonoBehaviour
{
    [SerializeField]
    private float attachmentLength;
    [SerializeField]
    private Vector3 attachmentDirection;

    private Vector3 globalDirection;
    private float globalLength;

    private void LateUpdate()
    {
        var tip = transform.TransformPoint(attachmentDirection.normalized * attachmentLength);
        globalDirection = (tip - transform.position).normalized;
        globalLength = (tip - transform.position).magnitude;
        if (tip.y < 0)
            SetTipToGround();

    }

    private void SetTipToGround()
    {
        var targetAngle = Mathf.Acos(transform.position.y / globalLength) * Mathf.Rad2Deg;
        var currentAngle = Vector3.Angle(Vector3.down, globalDirection);
        var rotationAxis = Vector3.Cross(Vector3.down, globalDirection).normalized;
        transform.Rotate(transform.InverseTransformDirection(rotationAxis), targetAngle - currentAngle, Space.Self);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.zero, attachmentDirection.normalized * attachmentLength);
        Gizmos.DrawSphere(attachmentDirection.normalized * attachmentLength, 0.1f);
    }
}
