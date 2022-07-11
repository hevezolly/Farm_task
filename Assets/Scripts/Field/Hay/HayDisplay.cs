using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class HayDisplay : MonoBehaviour
{
    private const string ColorName = "_Color";
    private const string WindStrengthName = "_WindStrength";


    [SerializeField]
    private GameObject slicedHayObj;

    [SerializeField]
    private HayMeshProvider generator;

    [SerializeField]
    private MeshFilter filter;

    [SerializeField]
    private MeshRenderer renderer;

    [SerializeField]
    private ScriptableValue<float> slicedTime;

    private Color color;
    private Vector2 windStrength;

    private MeshRenderer slicedHayRenderer;
    private MeshFilter slicedHayFilter;

    private Sequence displaySlicedSequence;


    public void SetUp(Color color)
    {
        windStrength = renderer.material.GetVector(WindStrengthName);
        SpawnSlicedVersion();
        displaySlicedSequence = GetDisplaySlicedSequence();
        ResetMesh();    
        SetColor(color);
    }

    private Sequence GetDisplaySlicedSequence()
    {
        return DOTween.Sequence()
            .AppendCallback(() =>
            {
                slicedHayRenderer.gameObject.SetActive(true);
            })
            .AppendInterval(slicedTime.Value)
            .AppendCallback(() => slicedHayRenderer.gameObject.SetActive(false));
    }

    private void SpawnSlicedVersion()
    {
        var slicedHay = Instantiate(slicedHayObj, transform.position, transform.rotation);
        slicedHay.transform.localScale = transform.localScale;
        slicedHayRenderer = slicedHay.GetComponent<MeshRenderer>();
        slicedHayFilter = slicedHay.GetComponent<MeshFilter>();
        slicedHay.SetActive(false);
    }

    public Color GetColor()
    {
        return color;//sharedColor.texture.GetPixel(lookupCord.x, lookupCord.y);
    }

    public Vector2 GetWindStrength()
    {
        //var color = sharedWind.texture.GetPixel(lookupCord.x, lookupCord.y);
        return windStrength; //new Vector2(color.r, color.g);
    }

    public void SetColor(Color color)
    {
        //sharedColor.SetPixel(lookupCord, color);
        this.color = color;
        renderer.material.SetColor(ColorName, color);
    }

    public void SetWindStrength(Vector2 strength)
    {
        // var color = new Color(strength.x, strength.y, 0, 0);
        windStrength = strength;
        renderer.material.SetVector(WindStrengthName, windStrength);
        slicedHayRenderer.material.SetVector(WindStrengthName, windStrength);
    }

    public void DisplaySliced()
    {
        slicedHayRenderer.material.SetColor(ColorName, color);
        displaySlicedSequence.Restart();
    }

    public void ResetMesh()
    {
        var hay = generator.GetMesh();
        filter.mesh = hay.hay;
        slicedHayFilter.mesh = hay.slicedHay;
    }

    private void OnDrawGizmosSelected()
    {
        if (filter == null || filter.mesh == null)
            return;
        Gizmos.matrix = filter.transform.localToWorldMatrix;
        foreach (var (p, n) in filter.mesh.vertices.Zip(filter.mesh.normals, (p, n) => (p, n)))
        {
            Gizmos.DrawLine(p, p + n);
        }
    }

    private void OnDestroy()
    {
        displaySlicedSequence?.Kill();
    }
}
