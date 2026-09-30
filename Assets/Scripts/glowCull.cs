using UnityEngine;

public class GlowHideOnPlayerInside : MonoBehaviour
{
    MeshRenderer glowRenderer;

    void Start()
    {
        glowRenderer = GetComponent<MeshRenderer>();

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
            glowRenderer.enabled = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
            glowRenderer.enabled = true;
    }
}
