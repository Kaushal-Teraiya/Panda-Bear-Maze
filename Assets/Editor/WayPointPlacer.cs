using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class WaypointPainterTool
{
    static GameObject parent;

    static float spacing = 20f;

    static Vector3 lastPlacedPosition;

    static bool hasPlacedFirst = false;

    static WaypointPainterTool()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        Handles.BeginGUI();

        GUILayout.BeginArea(
            new Rect(10, 10, 220, 80),
            "Waypoint Painter",
            GUI.skin.window
        );

        GUILayout.Label("Hold SHIFT + Drag Mouse");

        GUILayout.Label("Spacing: " + spacing.ToString("F1"));

        spacing = GUILayout.HorizontalSlider(
            spacing,
            2f,
            40f
        );

        GUILayout.EndArea();

        Handles.EndGUI();


        if (!e.shift)
            return;


        if (parent == null)
        {
            parent = GameObject.Find("WaypointGraph");

            if (parent == null)
                parent = new GameObject("WaypointGraph");
        }


        Ray ray =
            HandleUtility.GUIPointToWorldRay(e.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Handles.color = Color.cyan;

            Handles.DrawWireDisc(
                hit.point,
                Vector3.up,
                spacing * 0.5f
            );


            if (e.type == EventType.MouseDown && e.button == 0)
            {
                PlaceWaypoint(hit.point);

                e.Use();
            }


            if (e.type == EventType.MouseDrag && e.button == 0)
            {
                if (!hasPlacedFirst ||
                    Vector3.Distance(
                        hit.point,
                        lastPlacedPosition
                    ) >= spacing)
                {
                    PlaceWaypoint(hit.point);

                    e.Use();
                }
            }
        }
    }


    static void PlaceWaypoint(Vector3 position)
    {
        GameObject wp =
            new GameObject("Waypoint");

        wp.transform.position =
            position + Vector3.up * 0.1f;

        wp.transform.parent =
            parent.transform;


        Waypoint component =
            wp.AddComponent<Waypoint>();


        component.id =
            parent.transform.childCount;


        lastPlacedPosition =
            position;


        hasPlacedFirst = true;
    }
}