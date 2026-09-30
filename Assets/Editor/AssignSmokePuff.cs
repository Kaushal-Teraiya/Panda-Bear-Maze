using UnityEngine;
using UnityEditor;

public class AssignSmokePuffTool : EditorWindow
{
    GameObject smokePrefab;

    [MenuItem("Tools/Assign Smoke Puff To TrapTriggers")]
    public static void ShowWindow()
    {
        GetWindow<AssignSmokePuffTool>(
            "Assign Smoke Puff"
        );
    }

    void OnGUI()
    {
        GUILayout.Label(
            "Assign Smoke Puff Prefab",
            EditorStyles.boldLabel
        );

        smokePrefab =
            (GameObject)EditorGUILayout.ObjectField(
                "Smoke Puff Prefab",
                smokePrefab,
                typeof(GameObject),
                false
            );

        GUILayout.Space(10);

        if (GUILayout.Button(
            "Assign To All TrapTriggers"
        ))
        {
            AssignSmoke();
        }
    }

    void AssignSmoke()
    {
        if (smokePrefab == null)
        {
            Debug.LogError(
                "Please assign a smoke prefab first."
            );
            return;
        }

        GameObject[] allObjects =
            UnityEngine.Object.FindObjectsByType<GameObject>(
                FindObjectsSortMode.None
            );

        int assignedCount = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "TrapTrigger")
            {
                JailDoorLift trigger =
                    obj.GetComponent<JailDoorLift>();

                if (trigger != null)
                {
                    trigger.smokePuffPrefab =
                        smokePrefab;

                    EditorUtility.SetDirty(trigger);

                    assignedCount++;
                }
            }
        }

        Debug.Log(
            "Assigned Smoke Puff to "
            + assignedCount +
            " TrapTrigger objects."
        );
    }
}