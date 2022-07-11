using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ScriptableValue<T> : ScriptableObject
{
    [SerializeField]
    private T currentValue;

    private T previusValue;

    public UnityEvent<T> ValueChangeEvent;

    public virtual T Value
    {
        get => currentValue;
        set 
        {
            currentValue = value;
            ValueChangeEvent?.Invoke(currentValue);
        }
    }

    private void OnValidate()
    {
        if (!previusValue.Equals(currentValue))
        {
            previusValue = currentValue;
            Value = currentValue;
        }
    }
}
