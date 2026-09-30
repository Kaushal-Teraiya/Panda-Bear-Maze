using UnityEngine;
using UnityEditor;

public class TrapTriggerWaypointGenerator
{
    [MenuItem("Tools/Create Missing Panda Waypoints")]
    static void CreateWaypoints()
    {
        GameObject[] allObjects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsSortMode.None
            );

        int modifiedCount = 0;

        foreach (GameObject obj in allObjects)
        {
            if (!obj.name.ToLower().Contains("traptrigger"))
                continue;

            // ✅ Only create if NO children exist
            if (obj.transform.childCount > 0)
            {
                Debug.Log(
                    "Skipping (already has children): " +
                    obj.name
                );

                continue;
            }

            CreateChild(obj.transform, "PandaSpawnPoint");
            CreateChild(obj.transform, "wayPoint_1");
            CreateChild(obj.transform, "wayPoint_2");
            CreateChild(obj.transform, "PandaStandPoint");

            modifiedCount++;
        }

        Debug.Log(
            "Finished. Created waypoint sets for " +
            modifiedCount +
            " TrapTriggers."
        );
    }


    static void CreateChild(
        Transform parent,
        string childName
    )
    {
        GameObject child =
            new GameObject(childName);

        child.transform.SetParent(parent);

        child.transform.localPosition =
            Vector3.zero;

        child.transform.localRotation =
            Quaternion.identity;

        AssignSceneIcon(child);
    }


    static void AssignSceneIcon(GameObject obj)
    {
        Texture2D icon = null;

        if (obj.name.Contains("Spawn"))
            icon =
            EditorGUIUtility.IconContent("sv_label_0")
            .image as Texture2D;

        else if (obj.name.Contains("Stand"))
            icon =
            EditorGUIUtility.IconContent("sv_label_3")
            .image as Texture2D;

        else if (obj.name.Contains("wayPoint"))
            icon =
            EditorGUIUtility.IconContent("sv_label_1")
            .image as Texture2D;

        if (icon != null)
            EditorGUIUtility.SetIconForObject(obj, icon);
    }
}