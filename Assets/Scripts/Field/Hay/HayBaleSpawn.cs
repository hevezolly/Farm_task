using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayBaleSpawn : MonoBehaviour
{
    [SerializeField]
    private SpawnObjectRequest spawner;
    [SerializeField]
    private HayDisplay hay;
    [SerializeField]
    private ScriptableValue<Vector3> baleDimentions;
    [SerializeField]
    private float spawnForce;
    [SerializeField]
    private float spawnTorque;

    private float spawnHeight;

    private void Awake()
    {
        spawnHeight = Mathf.Sqrt(
            baleDimentions.Value.x * baleDimentions.Value.x +
            baleDimentions.Value.y * baleDimentions.Value.y +
            baleDimentions.Value.z * baleDimentions.Value.z);
    }
    public void Spawn()
    {
        var lounchForce = Random.insideUnitSphere * spawnForce;
        var lounchTorque = Random.insideUnitCircle * spawnTorque;
        lounchForce.y = Mathf.Abs(lounchForce.y);
        var bale = spawner.Spawn(transform.position + Vector3.up * spawnHeight, Quaternion.identity);
        bale.GetComponent<MeshRenderer>().material.color = hay.GetColor();
        var rb = bale.GetComponent<Rigidbody>();
        rb.AddForce(lounchForce, ForceMode.Impulse);
        rb.AddTorque(lounchTorque, ForceMode.Impulse);
    }
}
