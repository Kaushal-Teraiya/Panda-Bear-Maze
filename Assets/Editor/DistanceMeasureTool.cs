using UnityEngine;
using UnityEditor;

public class DistanceMeasureTool : EditorWindow
{
    Vector3? pointA = null;
    Vector3? pointB = null;

    [MenuItem("Tools/Distance Measure Tool")]
    public static void Open()
    {
        GetWindow<DistanceMeasureTool>(
            "Distance Tool"
        );
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnGUI()
    {
        GUILayout.Label(
            "Click two points in Scene View",
            EditorStyles.boldLabel
        );

        if (GUILayout.Button("Reset Points"))
        {
            pointA = null;
            pointB = null;
        }

        if (pointA.HasValue && pointB.HasValue)
        {
            float dist =
                Vector3.Distance(
                    pointA.Value,
                    pointB.Value
                );

            GUILayout.Label(
                "Distance: " + dist.ToString("F2")
            );
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown &&
            e.button == 0 &&
            !e.alt)
        {
            Ray ray =
                HandleUtility.GUIPointToWorldRay(
                    e.mousePosition
                );

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (!pointA.HasValue)
                {
                    pointA = hit.point;
                }
                else if (!pointB.HasValue)
                {
                    pointB = hit.point;

                    float dist =
                        Vector3.Distance(
                            pointA.Value,
                            pointB.Value
                        );

                    Debug.Log(
                        "Distance: " +
                        dist.ToString("F2")
                    );
                }
                else
                {
                    pointA = hit.point;
                    pointB = null;
                }

                e.Use();
            }
        }

        DrawMarkers();
    }

    void DrawMarkers()
    {
        if (pointA.HasValue)
        {
            Handles.color = Color.green;
            Handles.SphereHandleCap(
                0,
                pointA.Value,
                Quaternion.identity,
                1.5f,
                EventType.Repaint
            );
        }

        if (pointB.HasValue)
        {
            Handles.color = Color.red;
            Handles.SphereHandleCap(
                0,
                pointB.Value,
                Quaternion.identity,
                1.5f,
                EventType.Repaint
            );

            Handles.DrawLine(
                pointA.Value,
                pointB.Value
            );
        }
    }
}