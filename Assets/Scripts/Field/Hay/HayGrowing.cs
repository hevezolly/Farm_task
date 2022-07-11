using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class HayGrowing : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer renderer;
    [SerializeField]
    private HayDisplay display;

    [Header("Grouth settings")]
    [SerializeField]
    private ScriptableValueField<float> waitBeforeGrouthTime;
    [SerializeField]
    private float grouthTime;
    [SerializeField]
    private Gradient colorOverGrouth1;
    [SerializeField]
    private Gradient colorOverGrouth2;

    public UnityEvent GroughFinishedEvent;

    private Vector2 windStrength;

    private float colorVariance;

    private Vector3 scale;
    private Sequence _growSequence;

    private Sequence GrowSequence { 
        get
        {
            _growSequence = _growSequence ?? GetCutSequence();
            return _growSequence;
        } 
    }

    private float grouthPercent;

    public bool IsGrowing { get; private set; } = false;

    public void Cut()
    {
        if (IsGrowing)
            return;
        colorVariance = Random.Range(0f, 1f);
        IsGrowing = true;
        scale = transform.localScale;
        transform.localScale = new Vector3(scale.x, 0, scale.z);
        grouthPercent = 0;
        windStrength = display.GetWindStrength();
        display.SetWindStrength(Vector2.zero);
        GrowSequence.Restart();
    }

    private Sequence GetCutSequence()
    {
        return DOTween.Sequence()
            .AppendInterval(waitBeforeGrouthTime.Value)
            .Append(DOTween.To(() => grouthPercent, (v) =>
            {
                transform.localScale = new Vector3(scale.x, Mathf.Lerp(0, scale.y, v), scale.z);
                var color = Color.Lerp(colorOverGrouth1.Evaluate(v), colorOverGrouth2.Evaluate(v), colorVariance);
                display.SetColor(color);
                display.SetWindStrength(Vector2.Lerp(Vector2.zero, windStrength, v));
                grouthPercent = v;
            }, 1, grouthTime))
            .AppendCallback(() =>
            {

                transform.localScale = scale;
                display.SetWindStrength(windStrength);
                var color = Color.Lerp(colorOverGrouth1.Evaluate(1), colorOverGrouth2.Evaluate(1), colorVariance);
                display.SetColor(color);
                IsGrowing = false;
                GroughFinishedEvent?.Invoke();
            }).SetEase(Ease.Linear);
    }

    private void Update()
    {
        //if (IsGrowing)
        //    return;
        //var random = Random.Range(0, 300);
        //if (random == 0)
        //    Cut();
    }

    private void OnDestroy()
    {
        _growSequence?.Kill();
    }
}
