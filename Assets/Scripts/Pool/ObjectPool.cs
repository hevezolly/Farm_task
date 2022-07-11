using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private SpawnObjectRequest spawnRequest;

    [SerializeField]
    private PoolledObject pooledObject;

    [SerializeField]
    private Transform poolObjectsParent;

    [SerializeField]
    [Min(0)]
    private int maxNumberOfObject;
    
    private Queue<PoolledObject> avalibleObjects;

    private void Awake()
    {
        avalibleObjects = new Queue<PoolledObject>();
        for (var i = 0; i < maxNumberOfObject; i++)
        {
            var obj = InstanciateExtraObject();
            obj.gameObject.SetActive(false);
            avalibleObjects.Enqueue(obj);
        }
        if (spawnRequest != null)
            spawnRequest.HookUp(Spawn);
    }

    public int AvalibleObjectsCount => avalibleObjects.Count;

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        PoolledObject poolObj;
        if (avalibleObjects.Count == 0)
             poolObj = InstanciateExtraObject();
        else
            poolObj = avalibleObjects.Dequeue();
        var obj = poolObj.gameObject;
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        pooledObject.InvokeSpawn();
        return obj;
    }

    private PoolledObject InstanciateExtraObject()
    {
        var obj = Instantiate(pooledObject, poolObjectsParent);
        obj.SetPool(this);
        return obj;
    }

    public void Return(PoolledObject obj)
    {
        obj.gameObject.SetActive(false);
        avalibleObjects.Enqueue(obj);
    }
}
