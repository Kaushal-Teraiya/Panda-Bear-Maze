using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PlaceBearWithClick
{
    static PlaceBearWithClick()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (
            e.type == EventType.MouseDown &&
            e.button == 0 &&
            e.control
        )
        {
            Ray ray =
                HandleUtility.GUIPointToWorldRay(
                    e.mousePosition
                );

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject bear =
                    GameObject.FindWithTag("Player");

                if (bear != null)
                {
                    Undo.RecordObject(
                        bear.transform,
                        "Move Bear"
                    );

                    bear.transform.position =
                        hit.point + Vector3.up * 0.1f;

                    Debug.Log(
                        "Bear moved to: " +
                        hit.point
                    );
                }
                else
                {
                    Debug.LogWarning(
                        "No object with tag 'Player' found."
                    );
                }

                e.Use();
            }
        }
    }
}