using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneViewBookmarksTool
{
    const string MENU_PATH = "Tools/Bookmarks/" ;

    private static int _bookmarkIndex = 0;


    [MenuItem(MENU_PATH + "Add %#&b")]
    public static void AddMenuItem()
    {
        Scene scene = EditorSceneManager.GetActiveScene();
        SceneViewBookmarksDirectory directory = SceneViewBookmarksDirectory.Find(scene);
        if (directory == null)
            directory = SceneViewBookmarksDirectory.Create(scene);
        
        directory.AddBookmark(SceneViewBookmark.CreateFromSceneView(SceneView.lastActiveSceneView));
    }

    [MenuItem(MENU_PATH + "Switch _b")]
    public static void SwitchMenuItem()
    {
        Scene scene = EditorSceneManager.GetActiveScene();
        SceneViewBookmarksDirectory directory = SceneViewBookmarksDirectory.Find(scene);
        if (directory != null)
        {
            SceneViewBookmark? bookmark = directory.GetBookmark(_bookmarkIndex);
            if (bookmark.HasValue)
            {
                bookmark.Value.SetSceneViewOrientation(SceneView.lastActiveSceneView);
                
                _bookmarkIndex++;
                if (_bookmarkIndex >= directory.Count)
                    _bookmarkIndex = 0;
            }
        }
    }

    [InitializeOnLoadMethod]
    static void Initialize()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }

    private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        _bookmarkIndex = 0;
    }
}
