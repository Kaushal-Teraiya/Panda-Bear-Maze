using UnityEngine;

public class WaypointConnector : MonoBehaviour
{
    public float connectionDistance = 18f;

    void Start()
    {
        Waypoint[] points =
            FindObjectsByType<Waypoint>(FindObjectsSortMode.None);

        foreach (Waypoint a in points)
        {
            foreach (Waypoint b in points)
            {
                if (a == b)
                    continue;

                float dist =
                    Vector3.Distance(
                        a.transform.position,
                        b.transform.position
                    );

                if (dist <= connectionDistance)
                {
                    if (!a.neighbors.Contains(b))
                        a.neighbors.Add(b);
                }
            }
        }

        Debug.Log("Waypoint connections built.");
    }
}