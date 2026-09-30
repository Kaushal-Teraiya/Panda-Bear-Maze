using UnityEngine;
using UnityEditor;
using TMPro;

public class CreateFeedbackUIEditor
{
    [MenuItem("Tools/Create Feedback Text UI")]
    static void CreateUI()
    {
        Canvas canvas =
            Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("No Canvas found in scene.");
            return;
        }

        GameObject panel =
            new GameObject("FeedbackPanel");

        panel.transform.SetParent(canvas.transform);

        RectTransform panelRect =
            panel.AddComponent<RectTransform>();

        panelRect.anchorMin =
            new Vector2(0.5f, 0.85f);

        panelRect.anchorMax =
            new Vector2(0.5f, 0.85f);

        panelRect.sizeDelta =
            new Vector2(600, 80);

        panelRect.anchoredPosition =
            Vector2.zero;


        CanvasGroup cg =
            panel.AddComponent<CanvasGroup>();


        GameObject textGO =
            new GameObject("FeedbackText");

        textGO.transform.SetParent(panel.transform);


        RectTransform textRect =
            textGO.AddComponent<RectTransform>();

        textRect.anchorMin =
            Vector2.zero;

        textRect.anchorMax =
            Vector2.one;

        textRect.offsetMin =
            Vector2.zero;

        textRect.offsetMax =
            Vector2.zero;


        TextMeshProUGUI text =
            textGO.AddComponent<TextMeshProUGUI>();

        text.text = "Feedback Message";

        text.fontSize = 40;

        text.alignment =
            TextAlignmentOptions.Center;

        text.color = Color.white;


        panel.AddComponent<FeedbackTextUI>();


        Selection.activeGameObject = panel;

        Debug.Log("Feedback UI created successfully.");
    }
}