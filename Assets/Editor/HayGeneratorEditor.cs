using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(HayGenerator))]
public class HayGeneratorEditor : Editor
{
    private const string NewMeshName = "new hay.asset";
    private const string NewMeshSliceName = "new hay slice.asset";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate"))
        {
            GenerateAndSaveMesh();
        }
    }

    private string GetFolderPath()
    {
        var path = "";
        var obj = Selection.activeObject;
        if (obj == null) path = "Assets";
        else path = AssetDatabase.GetAssetPath(obj.GetInstanceID());
        if (path.Length > 0)
        {
            if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                var dir_path = Path.GetDirectoryName(path);

                return dir_path;
            }
        }
        else
        {
            return null;
        }
    }

    private string GetFieName(string path, string name)
    {
        return AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, name));
    }

    private void GenerateAndSaveMesh()
    {
        var path = GetFolderPath();
        if (path == null)
        {
            Debug.LogError("can't create mesh, incorrect path");
            return;
        }
        var hay = ((HayGenerator)target).GetMesh();
        var meshName = GetFieName(path, NewMeshName);
        var sliceName = GetFieName(path, NewMeshSliceName);
        AssetDatabase.CreateAsset(hay.hay, meshName);
        AssetDatabase.CreateAsset(hay.slicedHay, sliceName);
        AssetDatabase.SaveAssets();
        Debug.Log($"mesh {meshName} successfully created");
        Debug.Log($"mesh slice {sliceName} successfully created");
    }
}
