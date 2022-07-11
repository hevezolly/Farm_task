using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SetNumberOfSequences : MonoBehaviour
{
    [SerializeField]
    private int numberOfSequences;

    private void Awake()
    {
        DOTween.SetTweensCapacity(numberOfSequences, numberOfSequences);
    }
}
