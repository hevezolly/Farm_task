using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyingCoin : MonoBehaviour
{

    [SerializeField]
    private float moveTime;

    [SerializeField]
    private AnimationCurve yMove1;

    [SerializeField]
    private AnimationCurve yMove2;

    private float xT;
    private float yT;

    private Sequence moveSequence;

    private Vector2 fromPosition;
    private Vector2 toPosition;

    private float travelledPercent;

    private System.Action onMoveFinished = null;

    private RectTransform rectTransform;

    private Camera cam;

    private void RandomizeT()
    {

        xT = Random.Range(0f, 1f);
        yT = Random.Range(0f, 1f);
    }

    public void Awake()
    {
        rectTransform = transform as RectTransform;
        cam = Camera.main;
        moveSequence = DOTween.Sequence()
            .AppendCallback(() => travelledPercent = 0)
            .Append(DOTween.To(() => travelledPercent, (v) => OnMove(v), 1f, moveTime))
            .AppendCallback(() => onMoveFinished?.Invoke()).SetEase(Ease.InOutQuad);
    }

    private void OnDisable()
    {
        moveSequence.Pause();
    }

    private void OnDestroy()
    {
        moveSequence?.Kill();
    }

    private void OnMove(float percent)
    {
        var yPercent = Mathf.Lerp(yMove1.Evaluate(percent), yMove2.Evaluate(percent), yT);
        rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(fromPosition.x, toPosition.x, percent),
            Mathf.Lerp(fromPosition.y, toPosition.y, yPercent));
    }

    public void StartMove(RectTransform finishPos, System.Action callBack = null)
    {
        if (!gameObject.activeSelf)
            return;
        RandomizeT();
        onMoveFinished = callBack;
        fromPosition = rectTransform.anchoredPosition;
        toPosition = finishPos.anchoredPosition;
        moveSequence.Restart();
    }

}
