using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCoinInterracttion : MonoBehaviour
{
    [SerializeField]
    private CoinSpin coin;

    [SerializeField]
    private float interractionTime;

    private Coroutine interraction;

    private WaitForSeconds waitForInterraction;
    private void Awake()
    {
        waitForInterraction = new WaitForSeconds(interractionTime);
    }
    public void StartOrContinueInterraction()
    {
        if (interraction != null)
            StopCoroutine(interraction);
        interraction = StartCoroutine(Interraction());
    }

    private IEnumerator Interraction()
    {
        coin.StartSpinning();
        yield return waitForInterraction;
        coin.StopSpinning();
    }
}
