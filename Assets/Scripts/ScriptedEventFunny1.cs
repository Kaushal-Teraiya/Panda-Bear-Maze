using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JailPandaTeaseEvent : MonoBehaviour
{
    [Header("Setup")]
    public GameObject pandaPrefab;

    public List<Transform> waypoints;

    public Camera playerCamera;
    public Camera cinematicCamera;

    [Header("Animation")]
    [SerializeField] AnimationClip[] teaseAnimations;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float animSpeed = 1f;

    bool triggered = false;

    GameObject pandaInstance;
    Animator pandaAnimator;
    public AnimationClip currentTease;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggered)
            return;

        if (waypoints.Count < 2)
        {
            Debug.LogError("Need at least 2 waypoints");
            return;
        }

        triggered = true;

        StartCoroutine(PlayScene());
    }

    IEnumerator PlayScene()
    {
        Debug.Log("Starting panda tease waypoint event");

        // switch to cinematic camera
        playerCamera.gameObject.SetActive(false);
        cinematicCamera.gameObject.SetActive(true);

        // spawn panda at waypoint 0
        pandaInstance = Instantiate(
            pandaPrefab,
            waypoints[0].position,
            waypoints[0].rotation
        );

        pandaInstance.SetActive(true);

        pandaAnimator = pandaInstance.GetComponent<Animator>();

        if (pandaAnimator == null)
        {
            Debug.LogError("Animator missing on panda prefab");
            yield break;
        }

        // move forward through waypoint list
        for (int i = 1; i < waypoints.Count; i++)
        {
            yield return MoveTo(waypoints[i].position);
        }

        // face player
        Vector3 lookPos = playerCamera.transform.position;
        lookPos.y = pandaInstance.transform.position.y;

        pandaInstance.transform.LookAt(lookPos);

        // play tease animation
        if (teaseAnimations != null && teaseAnimations.Length > 0)
        {
            AnimationClip tease =
                teaseAnimations[
                    Random.Range(0, teaseAnimations.Length)
                ];
                currentTease = tease;
            pandaAnimator.Play(tease.name);
        }
        else
        {
            Debug.LogWarning("No tease animations assigned.");
        }

        Debug.Log("Playing tease animation");

        // restore player camera immediately when tease starts
        cinematicCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);

        // wait exactly animation duration
        yield return new WaitForSeconds(currentTease.length / animSpeed);

        // move backward through waypoint list
        for (int i = waypoints.Count - 2; i >= 0; i--)
        {
            yield return MoveTo(waypoints[i].position);
        }

        Destroy(pandaInstance);

        Debug.Log("Panda tease event finished");
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (
            Vector3.Distance(
                pandaInstance.transform.position,
                target
            ) > 0.1f
        )
        {
            pandaInstance.transform.position =
                Vector3.MoveTowards(
                    pandaInstance.transform.position,
                    target,
                    moveSpeed * Time.deltaTime
                );

            pandaInstance.transform.LookAt(target);

            yield return null;
        }
    }
}