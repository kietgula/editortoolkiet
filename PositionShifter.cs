using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = System.Object;

public class PositionShifter: EditorWindow
{
    [MenuItem("Tools/PositionShifter")]
    public static void ShowWindow()
    {
        PositionShifter wnd = GetWindow<PositionShifter>();
        wnd.titleContent = new GUIContent("Window.PositionShifter");
        wnd.minSize = new Vector2(300f, 100f);
    }
   
    private Vector3 _shiftAmount = Vector3.zero;

    public void OnGUI()
    {
        _shiftAmount = EditorGUILayout.Vector3Field("Shift Amount", _shiftAmount);
        EditorGUILayout.Space();
        if (GUILayout.Button("Shift"))
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                go.transform.localPosition += _shiftAmount;
            }
        }
    }
}