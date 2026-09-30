using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PandaPeekabooController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform waypointParent;
    public Animator pandaAnimator;
    public MonoBehaviour playerMovementScript;
    public GameObject smokePuffPrefab;
    public GameObject catchSmokePrefab;
    [Header("Behaviour Settings")]
    public float detectionDistance = 150f;
    public float waitTime = 10f;
    public float runSpeed = 7f;
    public float eyeHeightOffset = 1.6f;
    public float corridorStepDistance = 55f;
    public float junctionSearchRadius = 6f;

    public float catchDistance = 3f;

    public bool playerInCatchRange = false;
    public float crawlSpeed;
    private int previousDirection = 0;
    float originalRunSpeed;


    Waypoint previousWaypoint = null;
    [Header("Animations")]
    public List<string> idleAnimations;
    public List<string> surpriseAnimations;
    bool hasSpawnedOnce = false;

    JunctionTrigger currentJunction = null;
    public GameObject[] giftPrefabs;
    List<Transform> waypoints = new List<Transform>();
    public bool useCrawlMovement = false;
    public bool isCrawling = false;
    bool playerSawPanda = false;
    private bool isTeleporting;
    public bool peakabooEnabled = true;
    public Coroutine pandaLoopRoutine;
    public float minSpawnDistance;
    public float maxSpawnDistance;
    private Vector3 playerMoveDirection;
    private Vector3 lastPlayerPosition;
    [Header("Spawn Timing")]
    public float respawnDelay = 4f;
    public bool isCapturedInBubble;
    public bool pandaEventRunning = false;
    void Start()
    {
        storedRunSpeed = runSpeed;
        gameObject.SetActive(true);
        CollectWaypoints();

        pandaLoopRoutine =
 StartCoroutine(PandaLoop());

        crawlSpeed = runSpeed / 2f;
    }

    public void StopPeekaboo()
    {
        if (pandaLoopRoutine != null)
        {
            StopCoroutine(pandaLoopRoutine);
            pandaLoopRoutine = null;
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.Log("PLAYER REF MISSING");
            return;
        }

        float dist =
            Vector3.Distance(
                transform.position,
                player.position
            );

        playerInCatchRange =
            dist <= catchDistance;

        //        Debug.Log("Catch range check: " + playerInCatchRange);
    }

    public void TryCatchPanda()
    {
        if (!playerInCatchRange)
            return;

        if (movementLocked)
            return;

        // if (pandaEventRunning)
        // {
        //     return;
        // }

        if (isCapturedInBubble)
        {
            return;
        }

        StartCoroutine(
            CatchSequence()
        );
    }

    public IEnumerator CatchSequence()
    {
        Debug.Log("Panda caught!");

        movementLocked = true;

        StopMovement();

        playerMovementScript.enabled = false;


        // // rotate panda toward player
        // Vector3 lookDir =
        //     player.position -
        //     transform.position;

        // lookDir.y = 0;

        // transform.rotation =
        //     Quaternion.LookRotation(lookDir);


        // rotate player toward panda
        Vector3 playerLookDir =
            transform.position -
            player.position;

        playerLookDir.y = 0;

        player.rotation =
            Quaternion.LookRotation(playerLookDir);


        pandaAnimator.CrossFade(
            "TakenHostage",
            0.1f
        );

        var playerAnimator = player.GetComponent<Animator>();

        playerAnimator.CrossFade(
            "Catch",
            0.1f
        );


        yield return new WaitForSeconds(1.2f);


        DropGift();


        SpawnCatchSmoke(transform.position);

        Vector3 hiddenPos =
            transform.position;

        hiddenPos.y = -80f;

        transform.position = hiddenPos;


        yield return new WaitForSeconds(3f);

        movementLocked = false;

        playerMovementScript.enabled = true;
    }

    void DropGift()
    {
        if (giftPrefabs.Length == 0)
            return;

        GameObject gift =
            Instantiate(
                giftPrefabs[
                    Random.Range(0, giftPrefabs.Length)
                ],
                transform.position,
                Quaternion.identity
            );

        Debug.Log("Gift dropped: " + gift.name);
    }

    void OnTriggerEnter(Collider other)
    {
        JunctionTrigger junction =
        other.GetComponent<JunctionTrigger>();

        if (junction != null)
        {
            currentJunction = junction;
        }
    }

    bool IsInFrontOfPlayer(Vector3 candidatePos)
    {
        Vector3 toCandidate =
            (candidatePos - player.position).normalized;

        float dot =
            Vector3.Dot(
                player.forward,
                toCandidate
            );

        // 0.4 ≈ ~66° forward cone
        return dot > 0.4f;
    }

    public void ResumePeekaboo()
    {
        if (pandaLoopRoutine == null)
        {
            pandaLoopRoutine =
            StartCoroutine(PandaLoop());
        }
    }
    void CollectWaypoints()
    {
        waypoints.Clear();

        foreach (Transform child in waypointParent)
        {
            waypoints.Add(child);
        }

        Debug.Log("Collected " + waypoints.Count + " panda waypoints.");
    }


    public IEnumerator PandaLoop()
    {
        while (peakabooEnabled)
        {
            if (!isCapturedInBubble && !movementLocked)
                yield return SpawnAtRandomWaypoint();

            pandaAnimator.SetBool(
                "enteredTriggerScene",
                false
            );

            playerSawPanda = false;

            float timer = 0f;


            while (timer < waitTime)
            {
                if (!peakabooEnabled)
                    yield break;

                if (PlayerCanSeePanda())
                {
                    playerSawPanda = true;
                    break;
                }

                timer += Time.deltaTime;

                yield return null;
            }


            if (playerSawPanda)
            {
                yield return SurpriseSequence();

                yield return RunAwaySequence();

                yield return new WaitForSeconds(respawnDelay);

            }

            //   yield return new WaitForSeconds(respawnDelay);
        }
    }

    public bool IsInJailScene()
    {
        return pandaAnimator.GetBool("enteredTriggerScene");
    }
    public bool movementLocked = false;
    private float storedRunSpeed;

    public void StopMovement()
    {
        movementLocked = true;
    }


    public void ResumeMovement()
    {
        movementLocked = false;
    }

    IEnumerator SpawnAtRandomWaypoint()
    {
        runSpeed = storedRunSpeed;
        if (isTeleporting)
            yield break;

        if (!peakabooEnabled)
            yield break;

        if (isCapturedInBubble)
            yield break;
        isTeleporting = true;


        if (movementLocked)
            yield break;



        if (hasSpawnedOnce)
        {
            SpawnSmoke(transform.position);
            yield return new WaitForSeconds(0.25f);
        }


        Vector3 origin =
            player.position + Vector3.up * eyeHeightOffset;


        bool playerMoving =
            playerMoveDirection.magnitude > 0.1f;

        bool forwardBlocked =
            Physics.Raycast(origin, player.forward, 75f);


        Vector3 directionReference;


        // detect walls around player
        bool leftBlocked =
            Physics.Raycast(
                origin,
                -player.right,
                4f
            );

        bool rightBlocked =
            Physics.Raycast(
                origin,
                player.right,
                4f
            );


        // MOVING → spawn ahead
        if (playerMoving)
        {
            directionReference =
                playerMoveDirection.normalized;

            Debug.Log("Spawn mode: forward (player moving)");
        }


        // IDLE + WALL AHEAD + BOTH SIDES BLOCKED → spawn forward
        else if (forwardBlocked && leftBlocked && rightBlocked)
        {
            directionReference =
                player.forward;

            Debug.Log("Spawn mode: forward (corridor dead-end)");
        }


        // IDLE + WALL AHEAD → spawn sideways
        else if (forwardBlocked)
        {
            directionReference =
                Random.value < 0.5f
                ? player.right
                : -player.right;

            Debug.Log("Spawn mode: sideways (wall ahead)");
        }


        // IDLE + OPEN CORRIDOR
        else
        {
            directionReference =
                player.forward;

            Debug.Log("Spawn mode: forward (idle)");
        }

        Transform bestCandidate = null;

        float bestScore = float.MinValue;


        foreach (Transform wp in waypoints)
        {
            float dist =
            Vector3.Distance(
                wp.position,
                player.position
            );


            if (dist < minSpawnDistance)
                continue;

            if (dist > maxSpawnDistance)
                continue;


            Vector3 dir =
            (wp.position - origin).normalized;


            float directionalDot =
            Vector3.Dot(directionReference, dir);


            // tighten cone
            float coneLimit = 0.6f;

            if (!playerMoving && !forwardBlocked)
                coneLimit = 0.75f;

            if (directionalDot < coneLimit)
                continue;

            bool hidden =
            Physics.Raycast(
                origin,
                dir,
                Vector3.Distance(origin, wp.position)
            );


            float score = 0f;


            // direction priority
            score += directionalDot * 3f;


            // hidden bonus
            if (hidden)
                score += 4f;


            // distance preference
            score -= dist * 0.015f;


            if (score > bestScore)
            {
                bestScore = score;
                bestCandidate = wp;
            }
        }


        if (bestCandidate == null)
        {
            Debug.Log("Fallback spawn triggered");

            bestCandidate =
            waypoints[
                Random.Range(0, waypoints.Count)
            ];
        }


        transform.SetPositionAndRotation(
            bestCandidate.position,
            Quaternion.identity
        );


        previousWaypoint = null;

        SpawnSmoke(transform.position);

        PlayRandomIdleAnimation();

        hasSpawnedOnce = true;

        yield return null;

        isTeleporting = false;
    }

    void SpawnCatchSmoke(Vector3 pos)
    {
        if (catchSmokePrefab == null)
            return;

        GameObject puff =
            Instantiate(
                catchSmokePrefab,
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
    bool IsReachableWithinSteps(
        Waypoint start,
        Waypoint target,
        int maxSteps
    )
    {
        Queue<Waypoint> queue =
            new Queue<Waypoint>();

        HashSet<Waypoint> visited =
            new HashSet<Waypoint>();

        queue.Enqueue(start);

        visited.Add(start);

        int steps = 0;

        while (queue.Count > 0 && steps <= maxSteps)
        {
            int count = queue.Count;

            for (int i = 0; i < count; i++)
            {
                Waypoint current =
                    queue.Dequeue();

                if (current == target)
                    return true;

                foreach (Waypoint n in current.neighbors)
                {
                    if (!visited.Contains(n))
                    {
                        visited.Add(n);
                        queue.Enqueue(n);
                    }
                }
            }

            steps++;
        }

        return false;
    }

    IEnumerator SurpriseSequence()
    {
        if (!peakabooEnabled)
            yield break;
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        yield return null;

        ForcePlayerIdle();
        string anim =
            surpriseAnimations[
                Random.Range(0, surpriseAnimations.Count)
            ];

        pandaAnimator.Play(anim);

        yield return null;

        yield return new WaitForSeconds(
            pandaAnimator.GetCurrentAnimatorStateInfo(0).length
        );



        if (playerMovementScript != null)
            playerMovementScript.enabled = true;


    }

    void ForcePlayerIdle()
    {
        Animator playerAnimator =
            player.GetComponentInChildren<Animator>();

        if (playerAnimator == null)
            return;

        playerAnimator.SetFloat("Speed", 0f);

        playerAnimator.CrossFade("Idle", 0.05f);
    }
    IEnumerator RunAwaySequence()
    {
        if (!peakabooEnabled)
            yield break;

        Debug.Log("Surprise finished. Starting escape.");

        Waypoint current =
            GetClosestWaypoint(transform.position);

        int direction =
            GetEscapeDirection(current);
        previousDirection = direction;
        int steps = 0;

        bool lostSight = false;


        while (steps < 25)
        {
            while (movementLocked)
                yield return null;
            while (isCapturedInBubble)
                yield return null;



            Waypoint next = null;
            direction = GetEscapeDirection(current);
            // PRIORITY 1: junction override
            if (currentJunction != null)
            {
                next = currentJunction.ChooseBranch();

                Debug.Log("Junction override → choosing WP " + next.index);

                currentJunction = null;
            }

            // PRIORITY 2: normal smart waypoint movement
            else
            {
                next =
                    GetNextSmartWaypoint(current, direction);
            }

            if (next == null)
            {
                Debug.Log("Dead end reached.");
                break;
            }

            Debug.Log("Running to WP " + next.index);

            yield return MoveToWaypoint(next);

            current = next;

            steps++;

            if (!PlayerCanSeePanda())
            {
                Debug.Log("Player lost sight.");
                lostSight = true;
                break;
            }
        }


        if (lostSight)
        {
            int extraSteps =
                Random.Range(10, 20);

            for (int i = 0; i < extraSteps; i++)
            {

                while (movementLocked || isCapturedInBubble)
                    yield return null;
                Waypoint next = null;


                // PRIORITY 1: junction override
                if (currentJunction != null)
                {
                    next = currentJunction.ChooseBranch();

                    Debug.Log("Junction override → choosing WP " + next.index);

                    currentJunction = null;
                }


                // PRIORITY 2: normal smart waypoint movement
                else
                {
                    next =
                        GetNextSmartWaypoint(current, direction);
                }

                if (next == null)
                    break;

                yield return MoveToWaypoint(next);

                current = next;

                Debug.Log("taking extra steps");
            }
        }


        if (!isCapturedInBubble && !movementLocked)
        {
            SpawnSmoke(transform.position);

            Vector3 hiddenPos = transform.position;
            hiddenPos.y = -80f;

            transform.position = hiddenPos;
        }

        yield break;

    }

    public IEnumerator WaitUntilBubbleReleased()
    {
        while (isCapturedInBubble)
            yield return null;
    }
    Waypoint GetNextSmartWaypoint(Waypoint current, int direction)
    {
        Debug.Log("---- Checking next waypoint from WP " + current.index);

        List<Waypoint> nearby =
            new List<Waypoint>();


        foreach (Transform t in waypoints)
        {
            Waypoint wp =
                t.GetComponent<Waypoint>();

            if (wp == null)
                continue;

            if (wp == current)
                continue;

            if (wp == previousWaypoint)
            {
                if (direction == previousDirection)
                {
                    Debug.Log("Skipping previous waypoint: " + wp.index);
                    continue;
                }

                Debug.Log("Direction changed. Allowing reverse to previous waypoint.");
            }


            float dist =
            Vector3.Distance(
                current.transform.position,
                wp.transform.position
            );


            if (dist <= junctionSearchRadius)
            {
                // ignore candidates behind walls
                Vector3 start =
     current.transform.position + Vector3.up * 0.4f;

                Vector3 end =
                    wp.transform.position + Vector3.up * 0.4f;

                LayerMask wallMask = LayerMask.GetMask("walls");

                if (!Physics.Linecast(
                        current.transform.position,
                        wp.transform.position,
                        wallMask
                    ))
                {
                    Debug.Log(
                        "Valid nearby candidate: " +
                        wp.index +
                        " distance=" + dist
                    );

                    nearby.Add(wp);
                }
                else
                {
                    Debug.Log(
                        "Blocked by collider: " +
                        // hit.collider.name +
                        " while checking WP " +
                        wp.index
                    );
                }
            }
        }


        if (nearby.Count == 0)
        {
            Debug.Log("Dead end detected at WP " + current.index);
            return null;
        }


        // detect forward junction branches
        List<Waypoint> junctionBranches =
            new List<Waypoint>();


        foreach (Waypoint wp in nearby)
        {
            if (direction > 0 &&
                wp.index > current.index + 1)
            {
                Debug.Log("Forward junction branch: " +
                          current.index + " -> " + wp.index);

                junctionBranches.Add(wp);
            }

            else if (direction < 0 &&
                     wp.index < current.index - 1)
            {
                Debug.Log("Forward junction branch: " +
                          current.index + " -> " + wp.index);

                junctionBranches.Add(wp);
            }
        }


        // try straight corridor first (60% chance)
        if (junctionBranches.Count > 0)
        {
            int nextIndex =
                current.index + direction;

            Waypoint straight =
                GetWaypointByIndex(nextIndex);

            if (straight != null)
            {
                float dist =
                Vector3.Distance(
                    current.transform.position,
                    straight.transform.position
                );

                if (dist <= corridorStepDistance)
                {
                    if (Random.value < 0.5f)
                    {
                        Debug.Log("Junction ignored → continuing straight");

                        previousWaypoint = current;
                        return straight;
                    }
                }
            }


            Debug.Log("Junction taken at WP " + current.index);

            Waypoint chosen =
            junctionBranches[
                Random.Range(
                    0,
                    junctionBranches.Count
                )
            ];

            Debug.Log("Chosen junction branch: " + chosen.index);

            previousWaypoint = current;

            return chosen;
        }


        // normal corridor continuation
        int nextIndexCorridor =
            current.index + direction;

        Waypoint indexedNext =
            GetWaypointByIndex(nextIndexCorridor);


        if (indexedNext != null)
        {
            float dist =
            Vector3.Distance(
                current.transform.position,
                indexedNext.transform.position
            );

            if (dist <= corridorStepDistance)
            {
                Debug.Log("Continuing corridor: " +
                          current.index +
                          " -> " +
                          indexedNext.index);
                Debug.Log(
                    $"Trying {indexedNext.index} distance={dist} max={corridorStepDistance}"
                );
                previousWaypoint = current;

                return indexedNext;
            }
        }


        // fallback safety
        Waypoint fallback =
            nearby[
                Random.Range(0, nearby.Count)
            ];

        Debug.Log("Fallback random nearby selected: " +
                  fallback.index);

        previousWaypoint = current;

        return fallback;
    }
    Waypoint GetWaypointByIndex(int index)
    {
        foreach (Transform t in waypointParent)
        {
            Waypoint wp =
                t.GetComponent<Waypoint>();

            if (wp.index == index)
                return wp;
        }

        return null;
    }

    Waypoint GetClosestWaypoint(Vector3 pos)
    {
        Waypoint closest = null;

        float bestDistance = Mathf.Infinity;

        foreach (Transform t in waypointParent)
        {
            float dist =
                Vector3.Distance(pos, t.position);

            if (dist < bestDistance)
            {
                bestDistance = dist;

                closest = t.GetComponent<Waypoint>();
            }
        }

        return closest;
    }

    int GetEscapeDirection(Waypoint pandaWP)
    {
        Waypoint playerWP =
            GetClosestWaypoint(player.position);

        Vector3 awayFromPlayer =
            pandaWP.transform.position -
            playerWP.transform.position;

        awayFromPlayer.Normalize();


        Waypoint forwardCandidate =
            GetWaypointByIndex(pandaWP.index + 1);

        Waypoint backwardCandidate =
            GetWaypointByIndex(pandaWP.index - 1);


        float forwardScore = -999f;
        float backwardScore = -999f;


        if (forwardCandidate != null)
        {
            Vector3 dir =
            (forwardCandidate.transform.position -
             pandaWP.transform.position).normalized;

            forwardScore =
            Vector3.Dot(dir, awayFromPlayer);
        }


        if (backwardCandidate != null)
        {
            Vector3 dir =
            (backwardCandidate.transform.position -
             pandaWP.transform.position).normalized;

            backwardScore =
            Vector3.Dot(dir, awayFromPlayer);
        }


        if (forwardScore > backwardScore)
        {
            Debug.Log("Escape direction: forward index");
            return +1;
        }
        else
        {
            Debug.Log("Escape direction: backward index");
            return -1;
        }
    }

    Vector3 GetSpawnDirection()
    {
        Vector3 directionReference;

        // if player moving → use movement direction
        if (playerMoveDirection.magnitude > 0.1f)
            directionReference = playerMoveDirection.normalized;

        else
        {
            // player standing still → check forward wall

            Vector3 origin =
                player.position + Vector3.up * eyeHeightOffset;

            bool forwardBlocked =
                Physics.Raycast(
                    origin,
                    player.forward,
                    6f
                );

            if (!forwardBlocked)
                directionReference = player.forward;

            else
            {
                // choose left or right randomly
                if (Random.value < 0.5f)
                    directionReference = -player.right;
                else
                    directionReference = player.right;

                Debug.Log("Forward blocked → switching sideways spawn direction");
            }
        }

        return directionReference;
    }
    Transform GetEscapeWaypoint()
    {
        Transform best = null;

        float bestDistance = 0f;

        foreach (Transform wp in waypoints)
        {
            float dist =
                Vector3.Distance(player.position, wp.position);

            if (dist > bestDistance)
            {
                bestDistance = dist;
                best = wp;
            }
        }

        return best;
    }


    bool PlayerCanSeePanda()
    {
        Vector3 rayOrigin =
            player.position + Vector3.up * eyeHeightOffset;

        Vector3 direction =
            transform.position - rayOrigin;

        float distance =
            direction.magnitude;

        if (distance > detectionDistance)
            return false;


        Ray ray =
            new Ray(rayOrigin, direction.normalized);


        if (
            Physics.Raycast(
                ray,
                out RaycastHit hit,
                distance
            )
        )
        {
            if (hit.transform == transform)
            {
                Debug.DrawRay(
                    rayOrigin,
                    direction,
                    Color.green
                );

                return true;
            }
            else
            {
                Debug.DrawRay(
                    rayOrigin,
                    direction,
                    Color.red
                );
            }
        }

        return false;
    }


    void PlayRandomIdleAnimation()
    {
        if (idleAnimations.Count == 0)
            return;

        pandaAnimator.Play(
            idleAnimations[
                Random.Range(0, idleAnimations.Count)
            ]
        );
    }

    Waypoint GetClosestWaypoint()
    {
        Waypoint closest = null;

        float bestDistance = Mathf.Infinity;

        foreach (Transform t in waypointParent)
        {
            float dist =
                Vector3.Distance(
                    transform.position,
                    t.position
                );

            if (dist < bestDistance)
            {
                bestDistance = dist;
                closest = t.GetComponent<Waypoint>();
            }
        }

        return closest;
    }


    Waypoint GetBestEscapeNeighbor(Waypoint current)
    {
        Waypoint best = null;

        float bestScore = 0;

        foreach (Waypoint neighbor in current.neighbors)
        {
            float dist =
                Vector3.Distance(
                    player.position,
                    neighbor.transform.position
                );

            if (dist > bestScore)
            {
                bestScore = dist;
                best = neighbor;
            }
        }

        return best;
    }

    IEnumerator MoveToWaypoint(Waypoint target)
    {


        while (
            Vector3.Distance(
                transform.position,
                target.transform.position
            ) > 0.1f
        )
        {
            if (movementLocked)
                yield break;
            transform.position =
                Vector3.MoveTowards(
                    transform.position,
                    target.transform.position,
                    runSpeed * Time.deltaTime
                );

            Vector3 dir =
target.transform.position - transform.position;

            dir.y = 0;

            if (dir != Vector3.zero)
                transform.rotation =
                Quaternion.LookRotation(dir);

            yield return null;
        }
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (player == null)
            return;

        Vector3 origin =
            player.position + Vector3.up * eyeHeightOffset;

        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(origin, 0.2f);

        Gizmos.DrawLine(origin, transform.position);
    }
#endif
}