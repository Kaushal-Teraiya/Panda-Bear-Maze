using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class AutoAssignGlowMaterials
{
    const float emissionIntensity = 10f;

    [MenuItem("Tools/Assign Glow Materials To All Doors")]
    static void AssignGlowMaterials()
    {
        GameObject[] allObjects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsSortMode.None
            );

        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.name != "glow")
                continue;

            MeshRenderer renderer =
                obj.GetComponent<MeshRenderer>();

            if (renderer == null)
                continue;

            // create URP material
            Material glowMat =
                new Material(
                    Shader.Find(
                        "Universal Render Pipeline/Lit"
                    )
                );

            // random glow color
            Color glowColor =
                Random.ColorHSV(
                    0f, 1f,
                    0.7f, 1f,
                    0.7f, 1f
                );

            glowMat.SetColor("_BaseColor", glowColor);

            // enable emission
            glowMat.EnableKeyword("_EMISSION");

            glowMat.SetColor(
                "_EmissionColor",
                glowColor * Mathf.Pow(2f, emissionIntensity)
            );


            glowMat.globalIlluminationFlags =
                MaterialGlobalIlluminationFlags.RealtimeEmissive;

            renderer.sharedMaterial =
                glowMat;

            count++;
        }

        Debug.Log(
            "Assigned glow materials to " +
            count +
            " glow doors."
        );
    }
}