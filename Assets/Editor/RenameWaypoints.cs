using UnityEngine;
using UnityEditor;

public class RenameWaypointsById
{
    [MenuItem("Tools/Rename Waypoints Using Id")]
    static void RenameAllWaypoints()
    {
        Waypoint[] waypoints =
            Object.FindObjectsByType<Waypoint>(
                FindObjectsSortMode.None
            );

        int renamedCount = 0;

        foreach (Waypoint wp in waypoints)
        {
            if (wp == null)
                continue;

            string newName =
                "Waypoint_" + wp.id;

            if (wp.gameObject.name != newName)
            {
                Undo.RecordObject(
                    wp.gameObject,
                    "Rename Waypoint"
                );

                wp.gameObject.name =
                    newName;

                renamedCount++;
            }
        }

        Debug.Log(
            "Renamed " +
            renamedCount +
            " waypoints successfully."
        );
    }
}