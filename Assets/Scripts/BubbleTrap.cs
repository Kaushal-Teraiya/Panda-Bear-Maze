using UnityEngine;
using System.Collections;

public class BubbleTrap : MonoBehaviour
{
    [Header("References")]

    public GameObject placeholderMesh;

    public GameObject heartBubbleChild;
    Quaternion storedRunRotation;
    Transform player;
    bool facePlayerWhileCaptured = false;


    [Header("Bubble Motion")]

    public float liftHeight = 1.5f;

    public float liftDuration = 2f;

    public float holdTime = 1f;

    public float wobbleStrength = 0.02f;

    public float wobbleSpeed = 4f;


    bool triggered = false;
    public PandaPeekabooController panda;
    PandaPeekabooController capturedPanda;

    void Start()
    {
        if (heartBubbleChild != null)
            heartBubbleChild.SetActive(false);

        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player tag not found!");
    }

    void Awake()
    {
        panda = GetComponent<PandaPeekabooController>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        PandaPeekabooController panda =
            other.GetComponentInParent<PandaPeekabooController>();

        if (panda == null)
            return;

        // 🚨 allow capture ONLY if panda is free
        if (panda.isCapturedInBubble)
            return;

        triggered = true;

        StartCoroutine(
            BubbleCaptureSequence(panda)
        );
    }
    void LateUpdate()
    {
        if (!facePlayerWhileCaptured)
            return;

        if (capturedPanda == null)
            return;

        Vector3 lookDir =
            player.position -
            heartBubbleChild.transform.position;

        lookDir.y = 0f;

        if (lookDir == Vector3.zero)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(lookDir);

        // Heart mesh forward axis = Y → compensate
        heartBubbleChild.transform.rotation =
            targetRotation *
            Quaternion.Euler(-90f, 0f, 0f);
    }
    IEnumerator BubbleCaptureSequence(
        PandaPeekabooController panda
    )
    {
        Debug.Log("Bubble trap triggered");

        facePlayerWhileCaptured = true;
        capturedPanda = panda;
        // hide ONLY the placeholder visual mesh
        if (placeholderMesh != null)
        {
            MeshRenderer mr =
                placeholderMesh.GetComponent<MeshRenderer>();

            if (mr != null)
                mr.enabled = false;
        }


        // disable trap trigger collider
        GetComponent<Collider>().enabled = false;


        // activate heart bubble
        heartBubbleChild.SetActive(true);


        // place bubble exactly at panda position
        heartBubbleChild.transform.position =
            panda.transform.position;


        // rotate bubble toward player direction
        Vector3 lookDir =
      player.position -
      panda.transform.position;

        lookDir.y = 0f;

        Quaternion facePlayer =
            Quaternion.LookRotation(lookDir);

        // heart forward axis = Y
        heartBubbleChild.transform.rotation =
            facePlayer *
            Quaternion.Euler(-90f, 0f, 0f);

        storedRunRotation =
            panda.transform.rotation;
        // attach panda inside bubble
        panda.transform.SetParent(
            heartBubbleChild.transform
        );
        // lock panda movement + behaviour
        panda.StopMovement();
        panda.isCapturedInBubble = true;
        Vector3 startPos =
            heartBubbleChild.transform.position;

        Vector3 targetPos =
            startPos + Vector3.up * liftHeight;


        float timer = 0f;


        // LIFT ANIMATION
        while (timer < liftDuration)
        {
            float t =
                timer / liftDuration;

            Vector3 basePos =
                Vector3.Lerp(startPos, targetPos, t);

            float wobble =
                Mathf.Sin(Time.time * wobbleSpeed)
                * wobbleStrength;

            heartBubbleChild.transform.position =
                basePos +
                heartBubbleChild.transform.right * wobble;

            timer += Time.deltaTime;

            yield return null;
        }


        // FLOAT HOLD TIME
        yield return new WaitForSeconds(holdTime);


        // detach panda from bubble
        panda.transform.SetParent(null);


        // hide bubble
        heartBubbleChild.SetActive(false);

        facePlayerWhileCaptured = false;
        // resume panda movement
        capturedPanda = null;
        panda.ResumeMovement();
        panda.isCapturedInBubble = false;
        // destroy trap object
        Destroy(gameObject);
    }


}