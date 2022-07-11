using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolledObject : MonoBehaviour
{
    private ObjectPool pool;

    public UnityEvent SpawnEvent;
    public UnityEvent DespawnEvent;
    public void SetPool(ObjectPool pool)
    {
        this.pool = pool;
    }

    public void InvokeSpawn()
    {
        SpawnEvent?.Invoke();
    }

    public void Despawn()
    {
        DespawnEvent?.Invoke();
        pool.Return(this);
    }
}
