using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoinSpin : MonoBehaviour
{

    [SerializeField]
    private Image targetImage;

    [SerializeField]
    private List<Sprite> sprites;
    [SerializeField]
    private float fullRotationTime;

    [SerializeField]
    private bool spinOnAwake;

    private Sequence seq;

    private bool isSpinning;

    private void Awake()
    {
        seq = DOTween.Sequence();
        var nextFrameDelay = fullRotationTime / sprites.Count;
        for (var i = 0; i < sprites.Count; i++)
        {
            var index = i;
            seq.AppendCallback(() => targetImage.sprite = sprites[index])
                .AppendInterval(nextFrameDelay);
        }
        seq.SetLoops(-1, LoopType.Restart);
        if (spinOnAwake)
            StartSpinning();
    }

    private void OnDisable()
    {
        if (isSpinning)
            seq.Pause();
    }

    private void OnEnable()
    {
        if (isSpinning)
            seq.Play();
    }

    private void OnDestroy()
    {
        seq?.Kill();
    }

    public void StartSpinning()
    {
        if (isSpinning)
            return;
        isSpinning = true;
        seq.Restart();
    }

    public void StopSpinning()
    {
        if (!isSpinning)
            return;
        isSpinning = false;
        seq.Pause();
        targetImage.sprite = sprites[0];
    }
}
