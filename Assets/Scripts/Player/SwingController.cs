using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    [SerializeField]
    private ScriptableValue<bool> isInSwingingState;
    [SerializeField]
    private ScytheCutter cutter;
    private bool isInSwing;

    private bool CanChangeSwingState => !isInSwingingState.Value;

    private void Awake()
    {
        cutter.gameObject.SetActive(isInSwingingState.Value);
    }

    private void OnEnable()
    {
        isInSwingingState.ValueChangeEvent.AddListener(OnSwingingStateChanged);
    }

    private void OnDisable()
    {
        isInSwingingState.ValueChangeEvent.RemoveListener(OnSwingingStateChanged);
    }

    public void OnSwingStarted()
    {
        if (isInSwing || CanChangeSwingState)
            return;
        isInSwing = true;
        cutter.CanCut = true;
    }

    private void OnSwingingStateChanged(bool newValue)
    {
        cutter.gameObject.SetActive(newValue);
        if (!newValue && isInSwing)
        {
            CancelSwing();
        }
    }

    private void CancelSwing()
    {
        isInSwing = false;
        cutter.CanCut = false;
    }

    public void OnSwingEnded()
    {
        if (!isInSwing || CanChangeSwingState)
            return;
        CancelSwing();
    }
}
