using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayLocator : MonoBehaviour
{
    [SerializeField]
    private ScriptableValue<bool> isInSwingingState;
    [SerializeField]
    private ScriptableValue<int> maxNumberOfHayBales;
    [SerializeField]
    private ScriptableValue<int> currentNumberOfHayBales;
    [SerializeField]
    private bool swingWhenFullyLoaded;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float delayBetweenSearches;
    [SerializeField]
    [Range(1, 360)]
    private float detectAngle;
    [SerializeField]
    [Range(-180, 180)]
    private float offsetAngle;
    [SerializeField]
    private float maxDetectDistance;
    [SerializeField]
    private float minDetectDistance;
    [SerializeField]
    private LayerMask hayLayer;

    private WaitForSeconds nextUpdateDelay;
    private int isSwingingId;
    private Coroutine update;

    private Quaternion rotationToDetectionAreaCenter;

    private void Awake()
    {
        isSwingingId = Animator.StringToHash("isSwinging");
        nextUpdateDelay = new WaitForSeconds(delayBetweenSearches);
        rotationToDetectionAreaCenter = Quaternion.Euler(Vector3.up * offsetAngle);
    }

    private void OnEnable()
    {
        update = StartCoroutine(HayLocationProcess());
    }

    private void OnDisable()
    {
        StopCoroutine(update);
    }

    private IEnumerator HayLocationProcess()
    {
        while (true)
        {
            if (!swingWhenFullyLoaded)
            {
                if (currentNumberOfHayBales.Value == maxNumberOfHayBales.Value)
                {
                    if (isInSwingingState.Value)
                    {
                        animator.SetBool(isSwingingId, false);
                        isInSwingingState.Value = false;
                    }
                    yield return nextUpdateDelay;
                    continue;
                }
            }
            var colliders = Physics.OverlapSphere(transform.position, maxDetectDistance, hayLayer);
            var success = false;
            foreach (var c in colliders)
            {
                if (Vector3.Distance(transform.position, c.transform.position) > maxDetectDistance)
                    continue;
                if (Vector3.Distance(transform.position, c.transform.position) < minDetectDistance)
                    continue;
                var hay = c.GetComponent<HayGrowing>();
                if (hay == null)
                    continue;
                if (hay.IsGrowing)
                    continue;
                var directionToTarget = c.transform.position - transform.position;
                if (Vector3.Angle(directionToTarget, rotationToDetectionAreaCenter * transform.forward) > detectAngle / 2)
                    continue;
                success = true;
                break;
            }
            if (success != isInSwingingState.Value)
            {
                animator.SetBool(isSwingingId, success);
                isInSwingingState.Value = success;
            }

            yield return nextUpdateDelay;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        var numOfSegments = detectAngle / 10;
        var rotator = Quaternion.Euler(0, detectAngle / numOfSegments, 0);
        var previusLong = Quaternion.Euler(0, -detectAngle / 2 + offsetAngle, 0) * (transform.forward * maxDetectDistance);
        var previusShort = Quaternion.Euler(0, -detectAngle / 2 + offsetAngle, 0) * (transform.forward * minDetectDistance);
        Gizmos.DrawLine(transform.position + previusShort, transform.position + previusLong);
        for (var i = 0; i < numOfSegments; i++)
        {
            var nextLong = rotator * previusLong;
            var nextShort = rotator * previusShort;
            Gizmos.DrawLine(transform.position + previusLong, transform.position + nextLong);
            Gizmos.DrawLine(transform.position + previusShort, transform.position + nextShort);
            previusLong = nextLong;
            previusShort = nextShort;
        }
        Gizmos.DrawLine(transform.position + previusShort, transform.position + previusLong);
    }
}
