using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BarnHayCollecting : MonoBehaviour
{
    [SerializeField]
    private Transform pickUpCenter;
    [SerializeField]
    private Transform storagePosition;
    [SerializeField]
    private Transform intermidiatePosition;
    [SerializeField]
    private HayBaleCollector player;

    [SerializeField]
    private float storageRadius;
    [SerializeField]
    private float stockDelay;

    public UnityEvent OnBaleAquire;

    private Coroutine stockProcess;

    private WaitForSeconds _wait;
    private WaitForSeconds waitForStockDelay
    {
        get
        {
            if (_wait == null)
                _wait = new WaitForSeconds(stockDelay);
            return _wait;
        }
    }

    private void OnEnable()
    {
        stockProcess = StartCoroutine(StockProcess());
    }

    private void OnDisable()
    {
        StopCoroutine(stockProcess);
    }

    private IEnumerator StockProcess()
    {
        while (true)
        {
            if (Vector3.Distance(player.transform.position, pickUpCenter.position) > storageRadius)
            {
                yield return waitForStockDelay;
                continue;
            }

            var bale = player.PickUpHayStackTop();
            if (bale == null)
            {
                yield return waitForStockDelay;
                continue;
            }

            bale.PlaceToBarn(storagePosition, () => OnBaleAquire?.Invoke(), intermidiatePosition);

            yield return waitForStockDelay;
        }
    }

    private void OnDrawGizmos()
    {
        if (pickUpCenter == null)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pickUpCenter.position, storageRadius);
    }
}
