using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HayCutting : MonoBehaviour
{
    [SerializeField]
    private HayGrowing growing;

    public UnityEvent CutEvent;

    private bool shouldBeCutted;


    private void Update()
    {
        if (shouldBeCutted)
        {
            CutEvent?.Invoke();
            growing.Cut();
            shouldBeCutted = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ScytheCutter>(out var cutter))
        {
            shouldBeCutted = cutter.CanCut && !growing.IsGrowing;
        }
    }
}
