using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadCounter : MonoBehaviour
{
    [SerializeField]
    private ScriptableValueField<int> maxNumberOfLoad;
    [SerializeField]
    private ScriptableValue<int> currentLoad;

    [SerializeField]
    private TextMeshProUGUI totalText;
    [SerializeField]
    private TextMeshProUGUI currentText;

    
    private void Start()
    {
        totalText.text = maxNumberOfLoad.Value.ToString();
    }

    private void OnLoadChange(int currentLoad)
    {
        currentText.text = currentLoad.ToString();
    }

    private void OnEnable()
    {
        currentLoad.ValueChangeEvent.AddListener(OnLoadChange);
    }

    private void OnDisable()
    {
        currentLoad.ValueChangeEvent.RemoveListener(OnLoadChange);
    }

}
