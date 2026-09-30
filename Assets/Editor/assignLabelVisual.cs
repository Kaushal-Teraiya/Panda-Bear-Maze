using UnityEngine;
using UnityEditor;

public class AssignWaypointIcons
{
    [MenuItem("Tools/Assign Icons To Panda Waypoints")]
    static void AssignIcons()
    {
        GameObject[] allObjects =
            Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            Texture2D icon = null;

            if (obj.name.ToLower().Contains("spawn"))
                icon = EditorGUIUtility.IconContent("sv_label_0").image as Texture2D;

            else if (obj.name.ToLower().Contains("stand"))
                icon = EditorGUIUtility.IconContent("sv_label_3").image as Texture2D;

            else if (obj.name.ToLower().Contains("waypoint"))
                icon = EditorGUIUtility.IconContent("sv_label_1").image as Texture2D;

            if (icon != null)
            {
                EditorGUIUtility.SetIconForObject(obj, icon);
                count++;
            }
        }

        Debug.Log("Assigned icons to " + count + " waypoint objects.");
    }
}