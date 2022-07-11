using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField]
    private ScriptableValue<int> moneyCount;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private float delayBeforeEachIncrease;

    private int currentMoneyCount;

    private WaitForSeconds moneyUpdateDelay;
    private WaitForEndOfFrame nextFrameDelay;
    private int moneyIncreasePerStep;
    private bool isCurrentlyUpdating;

    private YieldInstruction Delay
    {
        get
        {
            if (Time.deltaTime > delayBeforeEachIncrease)
                return nextFrameDelay;
            return moneyUpdateDelay;
        }
    }

    private int MoneyIncreasePerStep
    {
        get
        {
            return Mathf.RoundToInt(Mathf.Max(Time.deltaTime, delayBeforeEachIncrease) / delayBeforeEachIncrease);
        }
    }

    private void Awake()
    {

        moneyUpdateDelay = new WaitForSeconds(delayBeforeEachIncrease);
        nextFrameDelay = new WaitForEndOfFrame();
    }

    private IEnumerator MoneyUpdateProcess()
    {
        isCurrentlyUpdating = true;
        while (currentMoneyCount < moneyCount.Value)
        {
            currentMoneyCount += MoneyIncreasePerStep;
            currentMoneyCount = Mathf.Min(currentMoneyCount, moneyCount.Value);
            UpdateMoneyCounter();
            yield return Delay;
        }
        isCurrentlyUpdating = false;
    }

    private void OnEnable()
    {
        moneyCount.ValueChangeEvent.AddListener(OnMoneyChange);
    }

    private void OnDisable()
    {
        moneyCount.ValueChangeEvent.RemoveListener(OnMoneyChange);
    }

    private void UpdateMoneyCounter()
    {
        text.text = currentMoneyCount.ToString();
    }

    private void OnMoneyChange(int moneyCount)
    {
        if (moneyCount < currentMoneyCount)
        {
            currentMoneyCount = moneyCount;
            UpdateMoneyCounter();
            return;
        }
        if (!isCurrentlyUpdating)
        {
            StartCoroutine(MoneyUpdateProcess());
        }
    }
}
