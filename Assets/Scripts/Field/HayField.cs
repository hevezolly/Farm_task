using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
public class HayField : MonoBehaviour
{
    [SerializeField]
    private HayDisplay HayObject;
    

    [SerializeField]
    private Vector2Int resolution;
    [SerializeField]
    private Vector2 singleBlockSize;
    [SerializeField]
    private Vector2 blocksOffset;

    [Header("material settings")]
    [SerializeField]
    private Material hayMaterial;
    [SerializeField]
    private Color initialColor1;
    [SerializeField]
    private Color initialColor2;
    [SerializeField]
    private Vector2 initialWindStrength;



    // Start is called before the first frame update
    void Start()
    {

        var forward = Vector3.forward;
        var right = Vector3.right;
        var up = Vector3.up;
        foreach (var (position, index) in GetBlocksCenters(Vector3.zero, forward, right, up))
        {
            var hay = Instantiate(HayObject, transform.TransformPoint(position), transform.rotation);
            hay.transform.localScale = transform.localScale;
            var color = Color.Lerp(initialColor1, initialColor2, Random.Range(0f, 1f));
            hay.SetUp(color);
        }
    }
    
    private IEnumerable<(Vector3, Vector2Int)> GetBlocksCenters(Vector3 center, Vector3 forward, Vector3 right)
    {
        return GetBlocksCenters(center, forward, right, Vector3.Cross(right, forward).normalized);
    }

    private IEnumerable<(Vector3, Vector2Int)> GetBlocksCenters(Vector3 center, Vector3 forward, Vector3 right, Vector3 up)
    {
        var start2D = singleBlockSize / 2 -
            (new Vector2(singleBlockSize.x * resolution.x, singleBlockSize.y * resolution.y) +
             new Vector2(blocksOffset.x * (resolution.x - 1), blocksOffset.y * (resolution.y - 1))) / 2f;
        var start = new Vector3(center.x + start2D.x, center.y, center.z + start2D.y);
        for (var x = 0; x < resolution.x; x++)
        {
            for (var z = 0; z < resolution.y; z++)
            {
                var offset = new Vector3((singleBlockSize.x + blocksOffset.x) * x, 0,
                    (singleBlockSize.y + blocksOffset.y) * z);
                var localPos = start + offset;
                yield return (forward * localPos.z + right * localPos.x + up * localPos.y, new Vector2Int(x, z));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        var forward = Vector3.forward;
        var right = Vector3.right;

        var size = new Vector3(singleBlockSize.x * right.magnitude, 0, singleBlockSize.y * forward.magnitude);
        foreach (var (pos, _) in GetBlocksCenters(Vector3.zero, forward, right))
        {
            Gizmos.DrawWireCube(pos, size);
        }
    }
}
