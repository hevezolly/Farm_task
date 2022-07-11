using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EzySlice;

[CreateAssetMenu(menuName = "Hay Provider/Hay Generator")]
public class HayGenerator : HayMeshProvider
{
    [SerializeField]
    private bool useRandomSeed = true;
    [SerializeField]
    private int seed;

    [Header("Resolution")]
    [SerializeField]
    [Min(1)]
    private int xResolution = 1;

    [SerializeField]
    [Min(1)]
    private int zResolution = 1;

    [Header("local size")]
    [SerializeField]
    private Vector3 localSize = Vector3.one;


    [Header("uv")]
    [SerializeField]
    private Vector2 uvxRange = new Vector2(0, 1);

    [SerializeField]
    private Vector2 uvyRange = new Vector2(0, 1);

    [Header("randomization")]
    [SerializeField]
    private Vector2 minMaxXDisplacement;
    [SerializeField]
    private Vector2 minMaxYDisplacement;
    [SerializeField]
    private float maxTiltAngle;

    [Header("slice")]
    [SerializeField]
    [Range(0, 1)]
    private float sliceHeight; 

    private Vector3 initialXZPos;
    private Vector3 halfstep;
    private System.Random random;
    
    public Mesh Generate()
    {
        if (useRandomSeed)
            seed = Random.Range(0, int.MaxValue);
        random = new System.Random(seed);
        initialXZPos = new Vector3(-localSize.x / 2, 0, -localSize.z / 2);
        halfstep = new Vector3(localSize.x / (xResolution * 2), 0, localSize.z / (zResolution * 2));

        var submesh = GenerateSubmesh();

        var mesh = new Mesh();
        mesh.vertices = submesh.positions;
        mesh.uv = submesh.uv;
        mesh.triangles = submesh.tringles;
        mesh.normals = submesh.normals;

        return mesh;
    }

    private Submesh GenerateSubmesh()
    {
        var result = Submesh.Empty;
        for (var x = 0; x < xResolution; x++)
        {
            for (var z = 0; z < zResolution; z++)
            {
                var center = initialXZPos + halfstep + new Vector3(halfstep.x * x * 2, 0, halfstep.z * z * 2);
                result = Submesh.Combine(result, GetCross(center));
            }
        }
        return result;
    }

    private float RandomRange(float min, float max)
    {
        return min + (float)random.NextDouble() * (max - min);
    }

    private Vector2 RandomOnCirkle()
    {
        var angle = RandomRange(0, 360) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private Submesh GetCross(Vector3 centerPosition)
    {
        var tringles = new List<int>();
        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();
        var normals = new List<Vector3>();

        var offset = new Vector3(
            RandomRange(minMaxXDisplacement.x, minMaxXDisplacement.y),
            0,
            RandomRange(minMaxYDisplacement.x, minMaxYDisplacement.y));

        var angle = RandomRange(0, maxTiltAngle) * Mathf.Deg2Rad;
        var tiltDir = RandomOnCirkle() * Mathf.Sin(angle);

        var up = Vector3.up * Mathf.Cos(angle) + new Vector3(tiltDir.x, 0, tiltDir.y);

        var center = new Vector3(centerPosition.x, 0, centerPosition.z);

        vertices.Add(center + offset);
        vertices.Add(center + up * localSize.y + offset);

        normals.Add(up);
        normals.Add(up);

        uv.Add(new Vector2((uvxRange.x + uvxRange.y) / 2, uvyRange.x));
        uv.Add(new Vector2((uvxRange.x + uvxRange.y) / 2, uvyRange.y));

        AddQuad(centerPosition + offset, new Vector3(0, 0, halfstep.z), 
            vertices, uv, tringles, normals,
            2, uvxRange.y, up);

        AddQuad(centerPosition + offset, new Vector3(-halfstep.x, 0, 0), 
            vertices, uv, tringles, normals,
            4, uvxRange.x, up);

        AddQuad(centerPosition + offset, new Vector3(0, 0, -halfstep.z), 
            vertices, uv, tringles, normals,
            6, uvxRange.x, up);

        AddQuad(centerPosition + offset, new Vector3(halfstep.x, 0, 0),
            vertices, uv, tringles, normals,
            8, uvxRange.y, up);

        return new Submesh()
        {
            positions = vertices.ToArray(),
            tringles = tringles.ToArray(),
            uv = uv.ToArray(),
            normals = normals.ToArray(),
        };
    }

    private void OnValidate()
    {
        uvxRange.x = Mathf.Clamp(uvxRange.x, 0, uvxRange.y);
        uvxRange.y = Mathf.Clamp(uvxRange.y, uvxRange.x, 1);
        uvyRange.x = Mathf.Clamp(uvyRange.x, 0, uvyRange.y);
        uvyRange.y = Mathf.Clamp(uvyRange.y, uvyRange.x, 1);
    }

    private void AddQuad(Vector3 centerPosition, 
        Vector3 offset, 
        List<Vector3> vertices, 
        List<Vector2> uv, 
        List<int> tringles, 
        List<Vector3> normals,
        int indexOffset, float uvx, Vector3 up)
    {
        var bottomCenter = new Vector3(centerPosition.x, 0, centerPosition.z);

        vertices.Add(bottomCenter + offset);
        vertices.Add(bottomCenter + up * localSize.y + offset);

        var middleCenter = bottomCenter;
        normals.Add((bottomCenter + offset - middleCenter).normalized);

        middleCenter = bottomCenter + up * localSize.y;
        normals.Add((bottomCenter + up * localSize.y + offset - middleCenter).normalized);

        uv.Add(new Vector2(uvx, uvyRange.x));
        uv.Add(new Vector2(uvx, uvyRange.y));
        
        tringles.Add(0);
        tringles.Add(indexOffset + 1);
        tringles.Add(indexOffset);
        tringles.Add(0);
        tringles.Add(1);
        tringles.Add(indexOffset + 1);
    }

    public override HayMesh GetMesh()
    {
        var mesh = Generate();
        var slice = GetSlice(mesh);
        return new HayMesh()
        {
            hay = mesh,
            slicedHay = slice
        };
    }

    private Mesh GetSlice(Mesh meshToSlice)
    {
        var slicePoint = new Vector3(0, sliceHeight * localSize.y, 0);
        var hull = Slicer.Slice(meshToSlice, new EzySlice.Plane(slicePoint, Vector3.up), new TextureRegion(0, 0, 1, 1), 1);
        return hull.lowerHull;
    }

    private class Submesh
    {
        public Vector2[] uv;
        public Vector3[] positions;
        public Vector3[] normals;
        public int[] tringles;

        public static Submesh Empty => new Submesh()
        {
            uv = new Vector2[0],
            positions = new Vector3[0],
            tringles = new int[0],
            normals = new Vector3[0],
        };

        public Submesh OffsetPosition(Vector3 offset)
        {
            for (var i = 0; i < positions.Length; i++)
            {
                positions[i] += offset;
            }
            return this;
        }

        public Submesh OffsetIndexes(int offset)
        {
            for (var i = 0; i < tringles.Length; i++)
            {
                tringles[i] += offset;
            }
            return this;
        }

        public static Submesh Combine(Submesh m1, Submesh m2)
        {
            m2.OffsetIndexes(m1.positions.Length);
            return new Submesh()
            {
                positions = m1.positions.Concat(m2.positions).ToArray(),
                uv = m1.uv.Concat(m2.uv).ToArray(),
                tringles = m1.tringles.Concat(m2.tringles).ToArray(),
                normals = m1.normals.Concat(m2.normals).ToArray(),
            };
        }
    }

}
