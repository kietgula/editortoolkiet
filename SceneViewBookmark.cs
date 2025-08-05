using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct SceneViewBookmark
{
    public Vector3 Pivot;
    public Quaternion Rotation;
    public float Size;
    public bool IsOrthographic;

    public static SceneViewBookmark CreateFromSceneView(SceneView sceneView)
    {
        SceneViewBookmark bookmark = new SceneViewBookmark()
            {
                Pivot = sceneView.pivot,
                Rotation = sceneView.rotation,
                Size = sceneView.size,
                IsOrthographic = sceneView.orthographic
            };
        return bookmark;
    }

    /// <summary>
    /// Set the scene view orientation from this Scene View Bookmark Orientation
    /// </summary>
    /// <param name="sceneView"></param>
    public void SetSceneViewOrientation(SceneView sceneView)
    {
        sceneView.pivot = Pivot;
        sceneView.rotation = Rotation;
        sceneView.size = Size;
        IsOrthographic = sceneView.orthographic;
    }
}
