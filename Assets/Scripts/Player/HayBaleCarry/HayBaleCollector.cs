using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayBaleCollector : MonoBehaviour
{
    [SerializeField]
    private float pickUpDelay;

    [SerializeField]
    private LayerMask hayBaleLayer;

    [SerializeField]
    private float pickUpDistance;

    [SerializeField]
    private ScriptableValueField<int> maxNumberOfHayBales;

    [SerializeField]
    private ScriptableValue<int> currentNumberOfBales;

    [SerializeField]
    private ScriptableValue<float> currentLoad;

    [SerializeField]
    private HayBaleTower stackTower;

    private Stack<HayBaleBehaviour> hayBaleStack = new Stack<HayBaleBehaviour>();

    private WaitForSeconds waitForNextSearch;
    private Coroutine search;

    private int CurrentNumberOfBales => hayBaleStack.Count;

    private void Awake()
    {
        waitForNextSearch = new WaitForSeconds(pickUpDelay);
    }

    private void OnEnable()
    {
        search = StartCoroutine(SearchProcess());
    }

    private void OnDisable()
    {
        StopCoroutine(search);
    }

    public HayBaleBehaviour PickUpHayStackTop()
    {
        if (CurrentNumberOfBales == 0)
            return null;
        var bale = hayBaleStack.Pop();
        UpdateStack();
        return bale;
    }

    private void UpdateStack()
    {
        currentLoad.Value = CurrentNumberOfBales / (float)maxNumberOfHayBales.Value;
        currentNumberOfBales.Value = CurrentNumberOfBales;
    }

    public IEnumerator SearchProcess()
    {
        while (true)
        {
            if (CurrentNumberOfBales >= maxNumberOfHayBales.Value)
            {
                yield return waitForNextSearch;
                continue;
            }
            var collides = Physics.OverlapSphere(transform.position, pickUpDistance, hayBaleLayer);

            foreach (var c in collides)
            {
                if (CurrentNumberOfBales >= maxNumberOfHayBales.Value)
                    break;
                if (!c.TryGetComponent<HayBaleBehaviour>(out var hay))
                    continue;
                if (!hay.CanBeStacked)
                    continue;
                hay.StackTo(stackTower.GetTowerPosition(CurrentNumberOfBales));
                
                hayBaleStack.Push(hay);
                UpdateStack();
                break;
            }

            yield return waitForNextSearch;
        }
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        currentLoad.Value = 0;
        currentNumberOfBales.Value = 0;
    }
#endif

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pickUpDistance);
    }

}
