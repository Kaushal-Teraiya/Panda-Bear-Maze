using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeCollectibleSpawner : MonoBehaviour
{
    [Header("Waypoint Setup")]
    public Transform waypointParent;

    [Header("Collectible Prefabs")]
    public GameObject[] collectiblePrefabs;

    [Header("Spawn Settings")]
    public int numberToSpawn = 100;

    public float lifetime = 60f; // destroy after 1 minute
    public Color[] collectibleColors =
 {
    new Color(0.6f, 0f, 1f),   // purple
    Color.blue,
    new Color(1f, 0.3f, 0.7f), // pink
    Color.yellow,
    Color.green
};


    void Start()
    {
        SpawnInitialCollectibles();
    }


    void SpawnInitialCollectibles()
    {
        if (waypointParent == null)
        {
            Debug.LogError("Waypoint parent missing!");
            return;
        }

        if (collectiblePrefabs.Length == 0)
        {
            Debug.LogError("No collectible prefabs assigned!");
            return;
        }


        List<Transform> availableWaypoints =
            new List<Transform>();


        foreach (Transform wp in waypointParent)
        {
            availableWaypoints.Add(wp);
        }


        // shuffle waypoint list
        for (int i = 0; i < availableWaypoints.Count; i++)
        {
            Transform temp =
                availableWaypoints[i];

            int randomIndex =
                Random.Range(i, availableWaypoints.Count);

            availableWaypoints[i] =
                availableWaypoints[randomIndex];

            availableWaypoints[randomIndex] =
                temp;
        }


        int spawnCount =
            Mathf.Min(
                numberToSpawn,
                availableWaypoints.Count
            );


        for (int i = 0; i < spawnCount; i++)
        {
            Transform wp =
                availableWaypoints[i];


            GameObject prefab =
                collectiblePrefabs[
                    Random.Range(
                        0,
                        collectiblePrefabs.Length
                    )
                ];


            GameObject spawned =
    Instantiate(
        prefab,
        wp.position + Vector3.up * 2.8f,
        Quaternion.identity
    );
            // apply emission
            Renderer[] renderers =
    spawned.GetComponentsInChildren<Renderer>();

            Color randomColor =
      collectibleColors[
          Random.Range(0, collectibleColors.Length)
      ];

            var collectibleScript =
                spawned.GetComponentInChildren<CollectibleContainer>();

            // if (collectibleScript != null)
            // {
            //     collectibleScript.ApplyRandomColor(randomColor);
            // }
            float emissionIntensity = 7f;

            foreach (Renderer rend in renderers)
            {
                foreach (Material mat in rend.materials)
                {
                    // change base color (URP)
                    if (mat.HasProperty("_BaseColor"))
                        mat.SetColor("_BaseColor", randomColor);

                    // fallback for Standard shader
                    if (mat.HasProperty("_Color"))
                        mat.SetColor("_Color", randomColor);


                    // enable emission
                    mat.EnableKeyword("_EMISSION");

                    mat.SetColor(
                        "_EmissionColor",
                        randomColor * Mathf.Pow(2f, emissionIntensity)
                    );

                    mat.globalIlluminationFlags =
                    MaterialGlobalIlluminationFlags.RealtimeEmissive;
                }
            }

            StartCoroutine(
                DestroyAfterTime(spawned, wp)
            );
        }


        Debug.Log(
            spawnCount +
            " collectibles spawned randomly across maze"
        );
    }


    IEnumerator DestroyAfterTime(GameObject obj, Transform wp)
    {
        yield return new WaitForSeconds(lifetime);

        if (obj != null)
            Destroy(obj);

        yield return new WaitForSeconds(1f); // small delay before respawn

        SpawnCollectibleAtWaypoint(wp);
    }

    void SpawnCollectibleAtWaypoint(Transform wp)
    {
        GameObject prefab =
            collectiblePrefabs[
                Random.Range(0, collectiblePrefabs.Length)
            ];

        GameObject spawned =
            Instantiate(
                prefab,
                wp.position + Vector3.up * 2.8f,
                Quaternion.identity
            );

        StartCoroutine(ApplyColorNextFrame(spawned));

        Renderer[] renderers =
            spawned.GetComponentsInChildren<Renderer>();

        Color randomColor =
            collectibleColors[
                Random.Range(0, collectibleColors.Length)
            ];

        var collectibleScript =
            spawned.GetComponentInChildren<CollectibleContainer>();

        if (collectibleScript != null)
        {
            collectibleScript.ApplyRandomColor(randomColor);
        }

        float emissionIntensity = 7f;

        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", randomColor);

                if (mat.HasProperty("_Color"))
                    mat.SetColor("_Color", randomColor);

                mat.EnableKeyword("_EMISSION");

                mat.SetColor(
                    "_EmissionColor",
                    randomColor * Mathf.Pow(2f, emissionIntensity)
                );

                mat.globalIlluminationFlags =
                    MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }
        }

        StartCoroutine(
            DestroyAfterTime(spawned, wp)
        );
    }

    IEnumerator ApplyColorNextFrame(GameObject spawned)
    {
        yield return null;

        Renderer[] renderers =
            spawned.GetComponentsInChildren<Renderer>();

        Color randomColor =
            collectibleColors[
                Random.Range(0, collectibleColors.Length)
            ];

        float emissionIntensity = 7f;

        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                if (mat.HasProperty("_BaseColor"))
                    mat.SetColor("_BaseColor", randomColor);

                if (mat.HasProperty("_Color"))
                    mat.SetColor("_Color", randomColor);

                mat.EnableKeyword("_EMISSION");

                mat.SetColor(
                    "_EmissionColor",
                    randomColor * Mathf.Pow(2f, emissionIntensity)
                );

                mat.globalIlluminationFlags =
                    MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }
        }
    }
}