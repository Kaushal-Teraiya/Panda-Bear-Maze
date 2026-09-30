using UnityEngine;
using System.Collections;

public class GlowDoorCameraDistanceTrigger : MonoBehaviour
{
    public TPSCamera cameraController;

    public float insideDistance = 10f;
    public float defaultDistance = 35f;

    public float transitionSpeed = 5f;
    public float minimumStayTime = 3f;

    Coroutine distanceRoutine;

    bool stayLocked = false;
    bool playerStillInside = false;

    void Awake()
    {
        cameraController = Camera.main.GetComponent<TPSCamera>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerStillInside = true;

        StartDistanceChange(insideDistance);

        if (!stayLocked)
            StartCoroutine(LockDistanceTimer());
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerStillInside = false;

        if (!stayLocked)
        {
            StartDistanceChange(defaultDistance);
        }
    }

    IEnumerator LockDistanceTimer()
    {
        stayLocked = true;

        yield return new WaitForSeconds(minimumStayTime);

        stayLocked = false;

        // restore AFTER timer if player already left trigger
        if (!playerStillInside)
        {
            StartDistanceChange(defaultDistance);
        }
    }

    void StartDistanceChange(float targetDistance)
    {
        if (distanceRoutine != null)
            StopCoroutine(distanceRoutine);

        distanceRoutine = StartCoroutine(
            SmoothDistanceChange(targetDistance)
        );
    }

    IEnumerator SmoothDistanceChange(float target)
    {
        while (Mathf.Abs(cameraController.distance - target) > 0.05f)
        {
            cameraController.distance = Mathf.Lerp(
                cameraController.distance,
                target,
                Time.deltaTime * transitionSpeed
            );

            yield return null;
        }

        cameraController.distance = target;
    }
}