using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SprintManager : MonoBehaviour
{
    public static SprintManager Instance;

    public MobilePlayerMovement movementScript;

    public float sprintDuration = 20f;
    public float sprintCooldown = 30f;

    float lastSprintTime = -999f;

    bool sprintActive = false;


    void Awake()
    {
        Instance = this;
    }


    public void ActivateSprint()
    {
        Debug.Log("Sprint button pressed");

        float remaining =
            lastSprintTime + sprintCooldown - Time.time;

        if (remaining > 0)
        {
            FeedbackTextUI.Instance.ShowMessage(
                "Sprint ready in " +
                Mathf.Ceil(remaining) + "s"
            );
            return;
        }

        if (sprintActive)
            return;

        StartCoroutine(SprintRoutine());
    }


    IEnumerator SprintRoutine()
    {
        sprintActive = true;

        lastSprintTime = Time.time;

        movementScript.sprintActive = true;

        Debug.Log("Sprint activated");

        FeedbackTextUI.Instance.ShowMessage(
            "Sprint activated!"
        );


        yield return new WaitForSeconds(
            sprintDuration
        );


        movementScript.sprintActive = false;

        sprintActive = false;


        yield return new WaitForSeconds(
            sprintCooldown - sprintDuration
        );


        FeedbackTextUI.Instance.ShowMessage(
            "Sprint ready!"
        );
    }
}