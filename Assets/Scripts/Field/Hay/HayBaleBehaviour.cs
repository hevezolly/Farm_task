using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HayBaleBehaviour : MonoBehaviour
{
    [SerializeField]
    private float stackTime;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private PoolledObject pooledObj;

    [SerializeField]
    private Collider collider;

    private Sequence moveSequence;
    public bool CanBeStacked { get; private set; } = true;

    private System.Action onMoveFinished;

    private Sequence GetMoveSequence(Ease ease, Transform intermidiateTarget = null)
    {
        var moveSeq = DOTween.Sequence();

        var moveTime = stackTime;
        if (intermidiateTarget != null)
        {
            var toIntermidiate = Vector3.Distance(transform.position, intermidiateTarget.position);
            var toFinish = Vector3.Distance(intermidiateTarget.position, transform.parent.position);
            moveTime = stackTime * toFinish / (toFinish + toIntermidiate);
            var toIntermidiateMoveTime = stackTime * toIntermidiate / (toFinish + toIntermidiate);
            moveSeq.Append(transform.DOMove(intermidiateTarget.position, toIntermidiateMoveTime));
        }
        moveSeq.Append(transform.DOLocalMove(Vector3.zero, moveTime));

        return DOTween.Sequence()
            .Append(moveSeq)
            .Join(transform.DOLocalRotateQuaternion(Quaternion.identity, stackTime))
            .AppendCallback(() =>
            {
                transform.localRotation = Quaternion.identity;
                transform.localPosition = Vector3.zero;
                onMoveFinished?.Invoke();
            }).SetEase(ease);
    }

    public void StackTo(Transform target)
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        CanBeStacked = false;
        collider.enabled = false;
        transform.SetParent(target, true);
        moveSequence?.Kill();
        moveSequence = GetMoveSequence(Ease.OutQuad);
        moveSequence.Play();
    }

    public void PlaceToBarn(Transform target, System.Action onFinish, Transform intermidiateTarget = null)
    {
        this.onMoveFinished = () =>
        {
            onFinish?.Invoke();
            pooledObj.Despawn();
        };
        transform.SetParent(target, true);
        moveSequence.Kill();
        moveSequence = GetMoveSequence(Ease.InQuad, intermidiateTarget);
        moveSequence.Play();
    }

    public void OnDespawn()
    {
        moveSequence.Pause();
        rb.isKinematic = false;
        collider.enabled = true;
        transform.SetParent(null, true);
        onMoveFinished = null;
        CanBeStacked = true;
    }

    private void OnDestroy()
    {
        moveSequence?.Kill();
    }
}
