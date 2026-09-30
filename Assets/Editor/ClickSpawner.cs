using UnityEngine;
using UnityEditor;

public class ClickSpawner : EditorWindow
{
    GameObject prefabToSpawn;

    Vector3 spawnScale = Vector3.one;

    float fixedYPosition = 0f;
    bool overrideYPosition = false;

    [MenuItem("Tools/Click Spawner")]
    public static void ShowWindow()
    {
        GetWindow<ClickSpawner>("Click Spawner");
    }

    void OnGUI()
    {
        GUILayout.Label("Prefab Placement Tool", EditorStyles.boldLabel);

        prefabToSpawn =
            (GameObject)EditorGUILayout.ObjectField(
                "Prefab",
                prefabToSpawn,
                typeof(GameObject),
                false
            );

        spawnScale =
            EditorGUILayout.Vector3Field(
                "Spawn Scale",
                spawnScale
            );

        overrideYPosition =
            EditorGUILayout.Toggle(
                "Override Y Position",
                overrideYPosition
            );

        if (overrideYPosition)
        {
            fixedYPosition =
                EditorGUILayout.FloatField(
                    "Spawn Y Position",
                    fixedYPosition
                );
        }
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += SceneClickHandler;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= SceneClickHandler;
    }

    void SceneClickHandler(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown &&
            e.alt &&
            e.button == 0)
        {
            if (prefabToSpawn == null)
                return;

            Ray ray =
                HandleUtility.GUIPointToWorldRay(
                    e.mousePosition
                );

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject obj =
                    (GameObject)PrefabUtility.InstantiatePrefab(
                        prefabToSpawn
                    );

                Undo.RegisterCreatedObjectUndo(
                    obj,
                    "Spawn Object"
                );

                Vector3 spawnPosition = hit.point;

                if (overrideYPosition)
                    spawnPosition.y = fixedYPosition;

                obj.transform.position =
                    spawnPosition;

                obj.transform.localScale =
                    spawnScale;

                Selection.activeGameObject =
                    obj;

                e.Use();
            }
        }
    }
}