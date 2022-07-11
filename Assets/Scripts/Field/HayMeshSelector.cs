using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Hay Provider/Hay selector")]
public class HayMeshSelector : HayMeshProvider
{
    [SerializeField]
    private List<HayMesh> pregeneratedMeshes;
    public override HayMesh GetMesh()
    {
        return pregeneratedMeshes[Random.Range(0, pregeneratedMeshes.Count)];
    }
}
