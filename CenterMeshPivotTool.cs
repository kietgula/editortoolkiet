using UnityEngine;
using UnityEditor;

public class CenterMeshPivotTool : EditorWindow
{
    [MenuItem("Tools/Center Mesh Pivot")]
    public static void CenterPivot()
    {
        var selected = Selection.gameObjects;
        int count = 0;

        foreach (var go in selected)
        {
            MeshFilter mf = go.GetComponent<MeshFilter>();
            if (mf == null || mf.sharedMesh == null)
                continue;

            Mesh original = mf.sharedMesh;
            Mesh meshCopy = Object.Instantiate(original);
            Vector3[] vertices = meshCopy.vertices;

            Vector3 center = meshCopy.bounds.center;

            for (int i = 0; i < vertices.Length; i++)
                vertices[i] -= center;

            meshCopy.vertices = vertices;
            meshCopy.RecalculateBounds();

            Undo.RecordObject(go.transform, "Reposition GameObject");
            go.transform.position += go.transform.TransformVector(center);

            string path = "Assets/GeneratedCenteredMeshes/";
            System.IO.Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(meshCopy, path + go.name + "_centered.asset");
            mf.sharedMesh = meshCopy;

            count++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✔ Recentered {count} GameObject. The new meshes are saved in Assets/GeneratedCenteredMeshes/");
    }
}
