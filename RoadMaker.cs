using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = System.Object;

public class RoadMaker : EditorWindow
{
    [MenuItem("Window/RoadMaker")]
    public static void ShowWindow()
    {
        RoadMaker wnd = GetWindow<RoadMaker>();
        wnd.titleContent = new GUIContent("Window.RoadMaker");
    }

    private GameObject roadParent = null;

    [Tooltip("Tick cái này thì tool mới thao tác được")]
    private bool isMakingRoad = false;
    
    [Tooltip("Tự động focus vào điêm cuối của đường đi mỗi khi tạo đường")]
    private bool isAutoFocusLastRoadPoint = false;
    
    [Tooltip("Điểm bắt đầu của đường đi")]
    [SerializeField]
    private Vector3 lastRoadPoint = Vector3.zero;
    
    [Tooltip("Độ rộng của đường đi")]
    private float roadWidth = 5f;

    private void Awake()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        minSize = new Vector2(270, 220);
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    public void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Road Parent:", EditorStyles.boldLabel);
        roadParent = EditorGUILayout.ObjectField(roadParent, typeof(GameObject), true) as GameObject;
        if (GUILayout.Button("Setup"))
        {
            if (roadParent == null)
            {
                Debug.LogError("Road Parent is null");
                return;
            }
            roadParent.AddComponent<NavMeshModifier>();
            roadParent.GetComponent<NavMeshModifier>().overrideArea = true;
            roadParent.GetComponent<NavMeshModifier>().area = 1;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("####################################");
        EditorGUILayout.Space();
        
        isMakingRoad = EditorGUILayout.Toggle("Is Making Road", isMakingRoad);
        
        isAutoFocusLastRoadPoint = EditorGUILayout.Toggle("Is Auto Focus Last Road", isAutoFocusLastRoadPoint);
        
        roadWidth = EditorGUILayout.FloatField("Road Width", roadWidth);
        if (GUILayout.Button("Go to last Road & Reset last Point"))  
        {
            if (roadParent != null)
            {
                if (roadParent.transform.childCount > 0)
                {
                    SceneView.lastActiveSceneView.pivot = roadParent.transform.GetChild(roadParent.transform.childCount - 1).gameObject.transform.position;
                    SceneView.lastActiveSceneView.Focus();
                    lastRoadPoint = Vector3.zero;
                }
                else
                {
                    lastRoadPoint = Vector3.zero;
                    Debug.LogError("Road Parent is empty");
                }
            }
        }
        if (GUILayout.Button("Delete last road"))
        {
            if (roadParent != null)
            {
                if (roadParent.transform.childCount > 0)
                {
                    DestroyImmediate(roadParent.transform.GetChild(roadParent.transform.childCount - 1).gameObject);
                    lastRoadPoint = Vector3.zero;
                }
            }
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("####################################");
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        
        lastRoadPoint = EditorGUILayout.Vector3Field("Last Road Point", lastRoadPoint);
        if (GUILayout.Button("Reset"))
        {
            lastRoadPoint = Vector3.zero;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (isMakingRoad && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.K)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (lastRoadPoint == Vector3.zero)
                {
                    lastRoadPoint = hit.point;
                }
                else
                {
                    if (Vector3.Distance(lastRoadPoint, hit.point) > 1)
                    {
                        GameObject road = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        road.transform.position = (lastRoadPoint + hit.point) / 2;
                        road.transform.localScale = new Vector3(roadWidth, 1f, Vector3.Distance(lastRoadPoint, hit.point) + 1f);
                        road.transform.LookAt(hit.point);
                        road.transform.parent = roadParent.transform;
                        lastRoadPoint = hit.point;

                        if (isAutoFocusLastRoadPoint)
                        {
                            SceneView.lastActiveSceneView.pivot = lastRoadPoint;
                            SceneView.lastActiveSceneView.Focus();
                        }
                    }
                }
            }
            
            Repaint();  
            
        }
    }
}
