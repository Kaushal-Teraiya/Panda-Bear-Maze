using UnityEngine;
using UnityEditor;

public class AssignSecretCodeUIToTrapTriggers : EditorWindow
{
    GameObject secretCodeUI;

    [MenuItem("Tools/Assign SecretCodeUI To TrapTriggers")]
    public static void ShowWindow()
    {
        GetWindow<AssignSecretCodeUIToTrapTriggers>(
            "Assign SecretCodeUI"
        );
    }


    void OnGUI()
    {
        GUILayout.Label(
            "Assign SecretCodeUI to all TrapTriggers",
            EditorStyles.boldLabel
        );


        secretCodeUI =
        (GameObject)EditorGUILayout.ObjectField(
            "Secret Code UI",
            secretCodeUI,
            typeof(GameObject),
            true
        );


        if (GUILayout.Button("Assign To TrapTriggers"))
        {
            AssignUI();
        }
    }


    void AssignUI()
    {
        if (secretCodeUI == null)
        {
            Debug.LogError(
                "Assign SecretCodeUI first!"
            );
            return;
        }


        GameObject[] allObjects =
        Object.FindObjectsByType<GameObject>(
            FindObjectsSortMode.None
        );


        int assignedCount = 0;


        foreach (GameObject obj in allObjects)
        {
            if (!obj.name.ToLower().Contains("traptrigger"))
                continue;


            JailDoorLift door =
            obj.GetComponent<JailDoorLift>();


            if (door == null)
                continue;


            Undo.RecordObject(
                door,
                "Assign SecretCodeUI"
            );


            door.secretCodeUI =
            secretCodeUI;


            // 🔥 REQUIRED so Unity saves the change
            EditorUtility.SetDirty(door);


            // 🔥 REQUIRED if TrapTrigger is prefab instance
            PrefabUtility.RecordPrefabInstancePropertyModifications(door);


            assignedCount++;
        }


        Debug.Log(
            "Assigned SecretCodeUI to "
            + assignedCount +
            " TrapTriggers."
        );
    }
}