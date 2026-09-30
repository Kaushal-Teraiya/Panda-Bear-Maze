using UnityEngine;
using UnityEditor;

public class AssignPeekabooController
{
    [MenuItem("Tools/Assign Panda Peekaboo Controller To TrapTriggers")]
    static void AssignController()
    {
        PandaPeekabooController controller =
            UnityEngine.Object.FindFirstObjectByType<PandaPeekabooController>();

        if (controller == null)
        {
            Debug.LogError(
                "No PandaPeekabooController found in scene."
            );
            return;
        }

        GameObject[] allObjects =
            Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        int assignedCount = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "TrapTrigger")
            {
                var trap =
                    obj.GetComponent<JailDoorLift>();

                if (trap != null)
                {
                    trap.pandaPeekabooController =
                        controller;

                    EditorUtility.SetDirty(trap);

                    assignedCount++;
                }
            }
        }

        Debug.Log(
            "Assigned PandaPeekabooController to "
            + assignedCount +
            " TrapTrigger objects."
        );
    }
}