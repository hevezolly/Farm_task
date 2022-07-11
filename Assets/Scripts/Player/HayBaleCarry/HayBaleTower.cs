using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayBaleTower : MonoBehaviour
{
    [SerializeField]
    private ScriptableValueField<int> maxNumbeOfHayBales;
    [SerializeField]
    private ScriptableValueField<Vector3> hayBaleDimentions;

    [SerializeField]
    private Transform initialBolckTransform;

    [SerializeField]
    [Range(0, 1)]
    private float blockAdjustmentSpeed;

    private List<Transform> hayBaleTower = new List<Transform>();

    private float minFps = 10f;


    private void Start()
    {
        hayBaleTower.Add(initialBolckTransform);
        for (var i = 1; i < maxNumbeOfHayBales.Value; i++)
        {
            var pos = GetBaleTargetPosition(i);
            var nextTransform = Instantiate(initialBolckTransform, pos, initialBolckTransform.rotation);
            hayBaleTower.Add(nextTransform);
        }   
    }

    private void LateUpdate()
    {
        var t = Mathf.Pow(blockAdjustmentSpeed, minFps * Time.deltaTime);
        for (var i = 1; i < maxNumbeOfHayBales.Value; i++)
        {
            var targetPos = hayBaleTower[i - 1].TransformPoint(Vector3.up * hayBaleDimentions.Value.y);
            var pos = Vector3.Lerp(hayBaleTower[i].position, targetPos, t);
            var rot = Quaternion.Lerp(hayBaleTower[i].rotation, hayBaleTower[i - 1].rotation, t);
            hayBaleTower[i].SetPositionAndRotation(pos, rot);
        }
    }

    public Transform GetTowerPosition(int index)
    {
        return hayBaleTower[index];
    }

    private Vector3 GetBaleTargetPosition(int baleIndex)
    {
        return initialBolckTransform.TransformPoint(Vector3.up * baleIndex * hayBaleDimentions.Value.y);
    }

    private void OnDrawGizmos()
    {
        if (initialBolckTransform == null || !hayBaleDimentions.HasValue)
            return;
        Gizmos.matrix = initialBolckTransform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, hayBaleDimentions.Value);
        
    }

    private void OnDrawGizmosSelected()
    {
        if (!maxNumbeOfHayBales.HasValue || initialBolckTransform == null || !hayBaleDimentions.HasValue)
            return;
        if (!Application.isPlaying)
        {
            Gizmos.matrix = initialBolckTransform.localToWorldMatrix;
            for (var i = 1; i < maxNumbeOfHayBales.Value; i++)
            {
                Gizmos.DrawWireCube(Vector3.up * i * hayBaleDimentions.Value.y, hayBaleDimentions.Value);
            }
        }
        if (Application.isPlaying)
        {

            Gizmos.color = Color.red;
            for (var i = 1; i < maxNumbeOfHayBales.Value; i++)
            {
                var target = hayBaleTower[i];
                Gizmos.matrix = target.localToWorldMatrix;
                Gizmos.DrawCube(Vector3.zero, hayBaleDimentions.Value);
            }
        }
    }

}
