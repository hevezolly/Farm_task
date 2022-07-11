using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayBaleDimentionsSetter : MonoBehaviour
{
    [SerializeField]
    private ScriptableValueField<Vector3> hayBaleDimentions;

    private void Awake()
    {
        transform.localScale = hayBaleDimentions.Value;
    }
}
