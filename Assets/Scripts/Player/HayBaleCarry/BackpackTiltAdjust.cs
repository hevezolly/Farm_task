using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackTiltAdjust : MonoBehaviour
{
    [SerializeField]
    private Vector3 fullLoadRotation;

    [SerializeField]
    private ScriptableValue<float> load;

    private Quaternion fullLoadRot;
    private Quaternion noLoadRot;

    private void Start()
    {
        fullLoadRot = Quaternion.Euler(fullLoadRotation);
        noLoadRot = transform.localRotation;
    }


    private void OnEnable()
    {
        load.ValueChangeEvent.AddListener(OnLoadChanged);
    }

    private void OnDisable()
    {
        load.ValueChangeEvent.RemoveListener(OnLoadChanged);
    }

    private void OnLoadChanged(float newLoad)
    {
        transform.localRotation = Quaternion.Slerp(noLoadRot, fullLoadRot, newLoad);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.parent.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        var up = Quaternion.Euler(fullLoadRotation) * Vector3.up;
        Gizmos.DrawLine(Vector3.zero, up);
    }

}
