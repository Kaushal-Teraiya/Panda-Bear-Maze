using UnityEngine;
using UnityEditor;

public class AssignInteractButtonToTrapTriggers
{
    [MenuItem("Tools/Assign Interact Button To TrapTriggers")]
    static void AssignButton()
    {
        GameObject button =
            GameObject.Find("OpenDoor");

        if (button == null)
        {
            Debug.LogError(
                "InteractButton not found in scene!"
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
            if (obj.name != "TrapTrigger")
                continue;

            JailDoorLift jailDoorLift =
                obj.GetComponent<JailDoorLift>();

            if (jailDoorLift == null)
                continue;

            Undo.RecordObject(
                jailDoorLift,
                "Assign Interact Button"
            );

            jailDoorLift.interactButtonUI =
                button;

            EditorUtility.SetDirty(
                jailDoorLift
            );

            assignedCount++;
        }

        Debug.Log(
            "Assigned interact button to "
            + assignedCount
            + " TrapTriggers."
        );
    }
}