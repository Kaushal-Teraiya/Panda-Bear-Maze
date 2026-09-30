using UnityEngine;
using UnityEditor;

public class SnapWaypointsToGround
{
    const float heightOffset = 0.1f;

    [MenuItem("Tools/Snap Waypoints To Ground + 0.1")]
    static void SnapWaypoints()
    {
        GameObject[] allObjects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsSortMode.None
            );

        int modifiedCount = 0;

        foreach (GameObject obj in allObjects)
        {
            string name = obj.name.ToLower();

            if (
                name.Contains("waypoint") ||
                name.Contains("spawnpoint") ||
                name.Contains("standpoint")
            )
            {
                Transform t = obj.transform;

                RaycastHit hit;

                Vector3 rayStart =
                    t.position + Vector3.up * 200f;

                if (
                    Physics.Raycast(
                        rayStart,
                        Vector3.down,
                        out hit,
                        500f,
                        Physics.DefaultRaycastLayers,
                        QueryTriggerInteraction.Ignore
                    )
                )
                {
                    Undo.RecordObject(
                        t,
                        "Snap waypoint to ground"
                    );

                    Vector3 pos = t.position;

                    pos.y =
                        hit.point.y + heightOffset;

                    t.position = pos;

                    modifiedCount++;
                }
                else
                {
                    Debug.LogWarning(
                        "No ground found under: " +
                        obj.name
                    );
                }
            }
        }

        Debug.Log(
            "Snapped " +
            modifiedCount +
            " waypoint objects successfully."
        );
    }
}