using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneViewBookmarksDirectory : ScriptableObject
{
    [SerializeField] [HideInInspector] private string _sceneGuid = null;

    [SerializeField] [Tooltip("The bookmarks")]
    private SceneViewBookmark[] _bookmarks = null;

    public int Count
    {
        get {return _bookmarks == null ? 0 : _bookmarks.Length;}
    }

    public static SceneViewBookmarksDirectory Create(Scene scene)
    {
        SceneViewBookmarksDirectory directory = ScriptableObject.CreateInstance<SceneViewBookmarksDirectory>();
        
        string sceneGuid = AssetDatabase.AssetPathToGUID(scene.path);
        directory._sceneGuid = sceneGuid;
        
        string sceneName = Path.GetFileNameWithoutExtension(scene.path);
        string path = scene.path.Substring(0, scene.path.Length - Path.GetFileName(scene.path).Length);
        path = Path.Combine(path, sceneName + "Bookmarks.asset");
        
        AssetDatabase.CreateAsset(directory, path);

        return directory;
    }

    /// <summary>
    /// Find the book marks directory for provided scene path
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static SceneViewBookmarksDirectory Find(Scene scene)
    {
        string sceneGuid = AssetDatabase.AssetPathToGUID(scene.path);
        foreach (string directoryGuid in AssetDatabase.FindAssets("t:SceneViewBookmarksDirectory"))
        {
            string pathToAsset = AssetDatabase.GUIDToAssetPath(directoryGuid);
            SceneViewBookmarksDirectory directory = AssetDatabase.LoadAssetAtPath<SceneViewBookmarksDirectory>(pathToAsset);
            if (directory._sceneGuid.Equals(sceneGuid))
                return directory;
        }

        return null;
    }

    /// <summary>
    /// Get the bookmark at the provided index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    [CanBeNull]
    public SceneViewBookmark? GetBookmark(int index)
    {
        if (_bookmarks == null || _bookmarks.Length <= index)
            return null;
        return _bookmarks[index];
    }

    /// <summary>
    /// Add a bookmark to this directory
    /// </summary>
    /// <param name="bookmark"></param>
    public void AddBookmark(SceneViewBookmark bookmark)
    {
        if (_bookmarks == null)
        {
            _bookmarks = new SceneViewBookmark[1];
            _bookmarks[0] = bookmark;
        }
        else
        {
            ArrayUtility.Add<SceneViewBookmark>(ref _bookmarks, bookmark);
        }
        
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
    }
}
