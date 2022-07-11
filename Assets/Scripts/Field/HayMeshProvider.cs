using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HayMeshProvider : ScriptableObject
{
    public abstract HayMesh GetMesh();
}

[System.Serializable]
public class HayMesh
{
    public Mesh hay;
    public Mesh slicedHay;
}
