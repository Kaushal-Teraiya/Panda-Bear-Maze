using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class JailDoorLift : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform jailDoor;
    public Transform player;

    public float closedY = 38.2f;
    public float openedY = -2.1f;
    public float moveDuration = 0.4f;
    public float groundThreshold = 0.3f;
    public float interactDistance = 2f;
    bool doorInteractionUnlocked = false;
    public GameObject interactButtonUI;

    bool playerInRange = false;
    static JailDoorLift activeDoorTrigger = null;
    Vector3 openPosition;
    Vector3 closedPosition;

    Coroutine moveRoutine;

    bool isOpen = true;


    // 🐼 PANDA EVENT SETTINGS
    [Header("Panda Waypoints")]
    public List<Transform> waypoints;

    [SerializeField] public AnimationClip[] teaseAnimations;

    public float pandaMoveSpeed = 60f;
    public float pandaAnimSpeed = 1f;

    public float hiddenYOffset = -80f;


    bool pandaEventRunning = false;
    public GameObject smokePuffPrefab;
    GameObject pandaInstance;
    Animator pandaAnimator;
    public AnimationClip currentTease;

    [Header("Jump Settings")]
    public AnimationClip jumpAnimation;
    public AnimationClip idleAnimation;

    public PandaPeekabooController pandaPeekabooController;

    public float jumpHeight = 2.5f;
    public float jumpDuration = 0.6f;
    [Header("Elevated Waypoint Settings")]
    public bool usesElevatedWaypoints = false;

    void Start()
    {
        if (player == null)
            player =
                GameObject
                .FindGameObjectWithTag("Player")
                .transform;

        openPosition =
            new Vector3(
                jailDoor.position.x,
                openedY,
                jailDoor.position.z
            );

        closedPosition =
            new Vector3(
                jailDoor.position.x,
                closedY,
                jailDoor.position.z
            );

        if (interactButtonUI != null)
            interactButtonUI.SetActive(false);

        jailDoor.position = openPosition;

        isOpen = true;
        AutoAssignWaypoints();



        // AUTO FIND PANDA BY TAG
        pandaInstance =
            GameObject.FindGameObjectWithTag("Panda");

        if (pandaInstance != null)
        {
            pandaAnimator =
                pandaInstance.GetComponent<Animator>();


            // hide panda underground initially
            Vector3 hiddenPos =
                pandaInstance.transform.position;

            hiddenPos.y = hiddenYOffset;

            pandaInstance.transform.position =
                hiddenPos;
        }
        else
        {
            Debug.LogError(
                "No object with tag 'Panda' found in scene."
            );
        }
    }


    void Update()
    {
        if (jailDoor == null)
            return;

        float distance =
            Vector3.Distance(player.position, jailDoor.position);

        bool inRange = distance <= interactDistance;


        if (inRange && !isOpen && doorInteractionUnlocked)
        {
            activeDoorTrigger = this;

            if (interactButtonUI != null)
                interactButtonUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
                TryOpenDoor();
        }
        else
        {
            if (activeDoorTrigger == this)
            {
                activeDoorTrigger = null;

                if (interactButtonUI != null)
                    interactButtonUI.SetActive(false);
            }
        }
    }
    void SpawnSmoke(Vector3 pos)
    {
        if (smokePuffPrefab == null)
            return;

        GameObject puff =
            Instantiate(
                smokePuffPrefab,
                pos,
                Quaternion.identity
            );

        ParticleSystem ps =
            puff.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            Destroy(
                puff,
                ps.main.duration +
                ps.main.startLifetime.constantMax
            );
        }
        else
        {
            Destroy(puff, 2f);
        }
    }

    public static void OpenActiveDoor()
    {
        if (activeDoorTrigger != null)
            activeDoorTrigger.TryOpenDoor();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        doorInteractionUnlocked = false;
        CloseDoor();

        if (
            !pandaPeekabooController.pandaEventRunning &&
            waypoints.Count >= 2 &&
            pandaInstance != null
        )
        {
            StartCoroutine(PlayPandaScene());
        }
    }




    public GameObject secretCodeUI;
    public void TryOpenDoor()
    {
        if (isOpen)
            return;


        // no keys → show feedback + open code panel
        if (Inventory.Instance.keyCount <= 0)
        {
            FeedbackTextUI.Instance.ShowMessage(
                "No keys left. Find a key OR Give your BAMBU a kissi to unlock!:)"
            );

            if (secretCodeUI != null)
            {
                secretCodeUI.SetActive(true);

                secretCodeUI
                .GetComponent<SecretCodeDoorUnlock>()
                .SetTargetDoor(this);
            }

            return;
        }


        // key exists → consume key and open door
        Inventory.Instance.UseKey();

        FeedbackTextUI.Instance.ShowMessage(
            "Door unlocked!"
        );

        OpenDoor();
    }

    public void OpenDoor()
    {
        MoveDoor(openPosition);

        isOpen = true;
    }


    public void CloseDoor()
    {
        MoveDoor(closedPosition);

        isOpen = false;
    }


    void MoveDoor(Vector3 destination)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine =
            StartCoroutine(
                LerpDoor(destination)
            );
    }


    IEnumerator LerpDoor(Vector3 destination)
    {
        Vector3 initialPosition =
            jailDoor.position;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            jailDoor.position =
                Vector3.Lerp(
                    initialPosition,
                    destination,
                    elapsed / moveDuration
                );

            elapsed += Time.deltaTime;

            yield return null;
        }

        jailDoor.position = destination;
    }


    // 🐼 PANDA EVENT LOGIC


    IEnumerator PlayPandaScene()
    {

        if (pandaPeekabooController == null)
        {
            Debug.LogError("Peekaboo controller missing reference!");
            yield break;
        }
        pandaAnimator.SetBool("enteredTriggerScene", true);
        pandaPeekabooController.StopPeekaboo();
         //pandaEventRunning = true;
        pandaPeekabooController.pandaEventRunning = true;
        //pandaAnimSpeed = pandaAnimator.GetCurrentAnimatorStateInfo(0).speed;
        doorInteractionUnlocked = false;
        // bring panda to waypoint start
        pandaInstance.transform.position =
            waypoints[0].position;

        pandaInstance.transform.rotation =
            waypoints[0].rotation;



        // move forward path
        for (int i = 1; i < waypoints.Count; i++)
        {
            while (pandaPeekabooController.isCapturedInBubble)
                yield return null;

            yield return MovePanda(waypoints[i]);
        }


        // face player before tease animation
        Vector3 lookPos = player.position;

        lookPos.y =
            pandaInstance.transform.position.y;

        pandaInstance.transform.LookAt(lookPos);

        if (teaseAnimations != null && teaseAnimations.Length > 0)
        {
            AnimationClip tease =
                teaseAnimations[
                    Random.Range(0, teaseAnimations.Length)
                ];

            currentTease = tease;

            pandaAnimator.CrossFade(tease.name, 0.1f);

            yield return StartCoroutine(
      WaitUntilTeaseAnimationFinishes(
          pandaAnimator,
          tease
      )
  ); doorInteractionUnlocked = true;
            while (pandaPeekabooController.isCapturedInBubble)
            {
                pandaAnimator.CrossFade("idle", 0.15f);
                yield return null;
            }

        }
        else
        {
            Debug.LogWarning("No tease animations assigned.");
        }

        // agent.isStopped = false;
        // rotate toward next waypoint BEFORE enabling movement animation



        Vector3 direction =
            (waypoints[waypoints.Count - 2].position -
             pandaInstance.transform.position).normalized;

        pandaInstance.transform.forward = direction;
        // move back path
        for (
            int i = waypoints.Count - 2;
            i >= 0;
            i--
        )
        {
            while (pandaPeekabooController.isCapturedInBubble)
                yield return null;

            yield return MovePanda(waypoints[i]);
        }


        // play disappear smoke BEFORE going underground
        SpawnSmoke(pandaInstance.transform.position);

        yield return new WaitForSeconds(0.25f);
        while (pandaPeekabooController.isCapturedInBubble)
            yield return null;

        // hide panda underground again
        Vector3 hiddenPos =
            pandaInstance.transform.position;

        hiddenPos.y = hiddenYOffset;

        pandaInstance.transform.position =
            hiddenPos;
        //pandaEventRunning = false;
        pandaPeekabooController.pandaEventRunning = false;
        pandaPeekabooController.transform.position =
        pandaInstance.transform.position;
        pandaAnimator.SetBool("enteredTriggerScene", false);
        pandaPeekabooController.ResumePeekaboo();
    }


    IEnumerator MovePanda(Transform target)
    {
        Vector3 start = pandaInstance.transform.position;
        Vector3 end = target.position;

        bool startElevated = start.y > groundThreshold;
        bool endElevated = end.y > groundThreshold;


        bool shouldJump =
            usesElevatedWaypoints &&
            (startElevated || endElevated);

        if (shouldJump)
        {
            pandaAnimator.Play(jumpAnimation.name, 0, 0f);

            float timer = 0f;



            while (timer < jumpDuration)
            {
                float t = timer / jumpDuration;

                // horizontal movement
                Vector3 horizontal =
                    Vector3.Lerp(start, end, t);

                // vertical arc (true parabola)
                float arc =
                    4 * jumpHeight * t * (1 - t);

                Vector3 position =
                    new Vector3(
                        horizontal.x,
                        Mathf.Lerp(start.y, end.y, t) + arc,
                        horizontal.z
                    );

                pandaInstance.transform.position = position;

                // smooth facing direction
                Vector3 lookDir = end - start;
                lookDir.y = 0;

                if (lookDir != Vector3.zero)
                {
                    pandaInstance.transform.rotation =
                        Quaternion.Slerp(
                            pandaInstance.transform.rotation,
                            Quaternion.LookRotation(lookDir),
                            10f * Time.deltaTime
                        );
                }

                timer += Time.deltaTime;

                yield return null;
            }
            pandaInstance.transform.position = end;


            if (endElevated)
            {
                if (usesElevatedWaypoints && jumpAnimation != null)
                {
                    pandaAnimator.Play(jumpAnimation.name, 0, 0f);
                }

                yield return new WaitForSeconds(0.5f);
            }


        }
        else
        {
            // normal walking movement
            while (
                Vector3.Distance(
                    pandaInstance.transform.position,
                    end
                ) > 0.1f
            )
            {
                pandaInstance.transform.position =
                    Vector3.MoveTowards(
                        pandaInstance.transform.position,
                        end,
                        pandaMoveSpeed * Time.deltaTime
                    );

                pandaInstance.transform.LookAt(end);

                yield return null;
            }
        }
    }

    void AutoAssignWaypoints()
    {
        List<Transform> sorted = new List<Transform>();

        Transform spawnPoint = null;
        Transform standPoint = null;

        List<Transform> numberedWaypoints = new List<Transform>();


        foreach (Transform child in transform)
        {
            string lower = child.name.ToLower();

            if (lower.Contains("spawn"))
            {
                spawnPoint = child;
            }
            else if (lower.Contains("stand"))
            {
                standPoint = child;
            }
            else if (lower.Contains("waypoint"))
            {
                numberedWaypoints.Add(child);
            }
        }


        // sort waypoint_1 waypoint_2 waypoint_3
        numberedWaypoints.Sort((a, b) =>
        {
            int aNum = ExtractNumber(a.name);
            int bNum = ExtractNumber(b.name);

            return aNum.CompareTo(bNum);
        });


        if (spawnPoint != null)
            sorted.Add(spawnPoint);

        sorted.AddRange(numberedWaypoints);

        if (standPoint != null)
            sorted.Add(standPoint);


        waypoints = sorted;

        Debug.Log(
            name +
            " auto-assigned " +
            waypoints.Count +
            " panda waypoints"
        );
    }
    int ExtractNumber(string text)
    {
        int underscoreIndex = text.IndexOf('_');

        if (underscoreIndex == -1)
            return 0;

        string numberPart = "";

        for (int i = underscoreIndex + 1; i < text.Length; i++)
        {
            if (char.IsDigit(text[i]))
            {
                numberPart += text[i];
            }
            else
            {
                break; // stop reading once digits end
            }
        }

        if (numberPart == "")
            return 0;

        return int.Parse(numberPart);
    }

    IEnumerator WaitUntilTeaseAnimationFinishes(
        Animator animator,
        AnimationClip teaseClip
    )
    {
        if (teaseClip == null)
            yield break;

        // wait until animator actually switches to tease clip
        while (true)
        {
            var clips =
                animator.GetCurrentAnimatorClipInfo(0);

            if (
                clips.Length > 0 &&
                clips[0].clip == teaseClip
            )
                break;

            yield return null;
        }

        // now wait until animation completes
        while (
            animator.GetCurrentAnimatorStateInfo(0)
                .normalizedTime < 1f
        )
        {
            yield return null;
        }
    }
}