using UnityEngine;
using System.Collections;

public class BananaPeelTrap : MonoBehaviour
{
    public float stunDuration = 1.2f;

    public string slipAnimationName = "Slip";
    public string crawlAnimationName = "Crawl";

    bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        PandaPeekabooController panda =
            other.GetComponentInParent<PandaPeekabooController>();

        if (panda == null)
            return;

        if (panda.isCapturedInBubble)
            return;

        triggered = true;

        StartCoroutine(
            SlipSequence(panda)
        );
    }


    IEnumerator SlipSequence(
        PandaPeekabooController panda
    )
    {
        Debug.Log("Banana peel triggered");

        // 🚨 lock panda movement globally
        panda.movementLocked = true;

        panda.StopMovement();

        panda.runSpeed = panda.crawlSpeed;
        // play slip animation
        if (panda.pandaAnimator != null)
        {
            panda.pandaAnimator.CrossFade(
                slipAnimationName,
                0.1f
            );
        }


        yield return new WaitForSeconds(
            stunDuration
        );


        // switch to crawl animation
        if (panda.pandaAnimator != null)
        {
            panda.pandaAnimator.CrossFade(
                crawlAnimationName,
                0.1f
            );
        }


        // 🚨 unlock movement again
        panda.movementLocked = false;
        panda.isCrawling = true;
        panda.ResumeMovement();


        Destroy(gameObject);
    }
}