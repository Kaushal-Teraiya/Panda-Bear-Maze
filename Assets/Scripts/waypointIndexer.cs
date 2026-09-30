using UnityEngine;

public class WaypointIndexer : MonoBehaviour
{
    void Awake()
    {
        Transform parent =
            GameObject.Find("WaypointGraph")?.transform;

        if (parent == null)
        {
            Debug.LogError("WaypointGraph not found.");
            return;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child =
                parent.GetChild(i);

            Waypoint wp =
                child.GetComponent<Waypoint>();

            if (wp != null)
            {
                wp.index = i + 1;

                Debug.Log(
                    child.name +
                    " assigned index " +
                    wp.index
                );
            }
        }

        Debug.Log("Waypoint indices assigned successfully.");
    }
}