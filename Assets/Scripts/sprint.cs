using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSprintAbility : MonoBehaviour
{
    public static PlayerSprintAbility Instance;

    public MobilePlayerMovement movementScript;

    public Button sprintButton;

    public float sprintDuration = 20f;
    public float sprintCooldown = 30f;

    float lastSprintTime = -999f;

    bool sprintActive = false;


    void Awake()
    {
        Instance = this;

        Debug.Log("SprintAbility Awake on: " + gameObject.name);
    }


    public void ActivateSprintFromUIButton()
    {
        Debug.Log("Sprint button pressed");

        ActivateSprint();
    }


    void ActivateSprint()
    {
        Debug.Log("ActivateSprint() called");

        if (movementScript == null)
        {
            Debug.LogError("movementScript NOT assigned!");
            return;
        }

        if (FeedbackTextUI.Instance == null)
        {
            Debug.LogError("FeedbackTextUI missing!");
            return;
        }


        float remaining =
            lastSprintTime + sprintCooldown - Time.time;


        if (remaining > 0)
        {
            Debug.Log("Sprint still on cooldown: " + remaining);

            FeedbackTextUI.Instance.ShowMessage(
                "Sprint ready in " +
                Mathf.Ceil(remaining) + "s"
            );

            return;
        }


        if (sprintActive)
        {
            Debug.Log("Sprint already active");
            return;
        }


        Debug.Log("Starting sprint routine");

        StartCoroutine(SprintRoutine());
    }


    IEnumerator SprintRoutine()
    {
        sprintActive = true;

        lastSprintTime = Time.time;

        if (sprintButton != null)
        {
            sprintButton.interactable = false;
            Debug.Log("Sprint button disabled");
        }
        else
        {
            Debug.LogWarning("Sprint button reference missing");
        }


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

        Debug.Log("Sprint duration finished");


        StartCoroutine(
            CooldownRoutine()
        );
    }

    void Start()
    {
        sprintButton =
            GameObject
            .Find("SprintButton")
            .GetComponent<Button>();

        if (sprintButton == null)
        {
            Debug.LogError("SprintButton not found in scene!");
        }
    }
    IEnumerator CooldownRoutine()
    {
        float cooldownRemaining =
            sprintCooldown - sprintDuration;

        Debug.Log("Cooldown started: " + cooldownRemaining);


        while (cooldownRemaining > 0)
        {
            yield return new WaitForSeconds(1f);

            cooldownRemaining--;

            Debug.Log("Cooldown remaining: " + cooldownRemaining);
        }


        if (sprintButton != null)
        {
            sprintButton.interactable = true;

            Debug.Log("Sprint button enabled again");
        }


        FeedbackTextUI.Instance.ShowMessage(
            "Sprint ready!"
        );
    }
}