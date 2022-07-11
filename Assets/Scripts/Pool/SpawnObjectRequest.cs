using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(menuName = "SpawnObjectRequest")]
public class SpawnObjectRequest : ScriptableObject
{
    private Func<Vector3, Quaternion, GameObject> spawnFunction = null;

    public void HookUp(Func<Vector3, Quaternion, GameObject> spawnFunc)
    {
        spawnFunction = spawnFunc;
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        return spawnFunction?.Invoke(position, rotation);
    }
}
