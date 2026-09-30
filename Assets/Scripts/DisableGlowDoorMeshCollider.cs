using UnityEngine;
using System.Collections;

public class GlowDoorCameraVisibility : MonoBehaviour
{
    MeshRenderer glowRenderer;

    public float restoreDelay = 2.5f;

    Coroutine restoreRoutine;

    void Start()
    {
        glowRenderer = GetComponent<MeshRenderer>();

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (restoreRoutine != null)
            StopCoroutine(restoreRoutine);

        glowRenderer.enabled = false;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (restoreRoutine != null)
            StopCoroutine(restoreRoutine);

        restoreRoutine = StartCoroutine(RestoreRenderer());
    }

    IEnumerator RestoreRenderer()
    {
        yield return new WaitForSeconds(restoreDelay);

        glowRenderer.enabled = true;
    }
}