using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpdator : MonoBehaviour
{
    [SerializeField]
    private ScriptableValue<int> moneyCount;

    

    public void AddMoney(int number)
    {
        moneyCount.Value += number;
    }

    

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        moneyCount.Value = 0;
    }
#endif
}
