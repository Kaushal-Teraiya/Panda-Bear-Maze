using UnityEngine;
using System.Collections;

public class EndGameTrigger : MonoBehaviour
{
    [Header("End UI Panel")]
    public CanvasGroup endPanel;

    [Header("Fade Settings")]
    public float fadeDuration = 2f;

    [Header("Optional Player Movement Lock")]
    public MonoBehaviour playerMovementScript;

    bool triggered = false;


    void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;

        StartCoroutine(FadeInEndPanel());
    }


    IEnumerator FadeInEndPanel()
    {
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;


        endPanel.gameObject.SetActive(true);


        float timer = 0f;


        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            endPanel.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    timer / fadeDuration
                );

            yield return null;
        }


        endPanel.alpha = 1f;

        endPanel.interactable = true;

        endPanel.blocksRaycasts = true;
    }
}