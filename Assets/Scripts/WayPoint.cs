using UnityEngine;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Waypoint : MonoBehaviour
{
    public int index;

    public int id;
    public List<Waypoint> neighbors =
           new List<Waypoint>();
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(transform.position, 0.15f);

        Handles.Label(
            transform.position + Vector3.up * 0.5f,
            "WP " + id
        );
    }
#endif
}