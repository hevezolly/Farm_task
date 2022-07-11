using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BarnCoinSpawner : MonoBehaviour
{
    [SerializeField]
    private SpawnObjectRequest coinSpawner;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Transform spawnPosition;

    [SerializeField]
    private RectTransform coinDestination;

    [SerializeField]
    private float coinSpawnDelay;
    private WaitForSeconds waitDelay;

    public UnityEvent CoinAquiredEvent;

    private void Awake()
    {
        waitDelay = new WaitForSeconds(coinSpawnDelay);
    }

    private IEnumerator WaitAndSpawnCoin() 
    {
        yield return waitDelay;
        SpawnCoin();
    }

    private void SpawnCoin()
    {
        var position = cam.WorldToScreenPoint(spawnPosition.position);
        var coin = coinSpawner.Spawn(position, Quaternion.identity);
        coin.GetComponent<FlyingCoin>().StartMove(coinDestination, () =>
        {
            CoinAquiredEvent?.Invoke();
            coin.GetComponent<PoolledObject>().Despawn();
        });
    }

    public void OnHayBaleCollected()
    {
        StartCoroutine(WaitAndSpawnCoin());
    }
    
    
}
