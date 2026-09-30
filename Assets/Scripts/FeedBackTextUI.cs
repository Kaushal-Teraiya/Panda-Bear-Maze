using UnityEngine;
using TMPro;
using System.Collections;

public class FeedbackTextUI : MonoBehaviour
{
    public static FeedbackTextUI Instance;

    TextMeshProUGUI text;

    CanvasGroup canvasGroup;

    Coroutine routine;


    void Awake()
    {
        Instance = this;

        text =
            GetComponentInChildren<TextMeshProUGUI>();

        canvasGroup =
            GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }


    public void ShowMessage(
        string message,
        float duration = 5f
    )
    {
        if (routine != null)
            StopCoroutine(routine);

        routine =
            StartCoroutine(
                ShowRoutine(message, duration)
            );
    }


    IEnumerator ShowRoutine(
        string message,
        float duration
    )
    {
        text.text = message;

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(duration);

        canvasGroup.alpha = 0f;
    }
}