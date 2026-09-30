using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class CreateSecretCodeUI
{
    [MenuItem("Tools/Create Secret Code UI")]
    static void CreateUI()
    {
        // Canvas
        GameObject canvasGO = new GameObject("SecretCodeCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();


        // Panel
        GameObject panel =
            CreateUIObject("SecretCodePanel", canvasGO.transform);

        RectTransform panelRect =
            panel.GetComponent<RectTransform>();

        panelRect.sizeDelta = new Vector2(400, 300);


        // Title text
        GameObject title =
            CreateTMPText(
                "TitleText",
                panel.transform,
                "Enter Secret Code",
                32,
                new Vector2(0, 100)
            );


        // Input field
        GameObject inputGO =
            CreateTMPInputField(panel.transform);


        RectTransform inputRect =
            inputGO.GetComponent<RectTransform>();

        inputRect.anchoredPosition =
            new Vector2(0, 30);


        // Button
        GameObject buttonGO =
            CreateButton(panel.transform);


        RectTransform buttonRect =
            buttonGO.GetComponent<RectTransform>();

        buttonRect.anchoredPosition =
            new Vector2(0, -50);


        // Feedback text
        GameObject feedback =
            CreateTMPText(
                "FeedbackText",
                panel.transform,
                "",
                24,
                new Vector2(0, -120)
            );


        Debug.Log("Secret Code UI created successfully!");
    }


    static GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject obj = new GameObject(name);

        obj.transform.SetParent(parent);

        RectTransform rect =
            obj.AddComponent<RectTransform>();

        rect.localScale = Vector3.one;

        rect.anchorMin =
            rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition = Vector2.zero;

        return obj;
    }


    static GameObject CreateTMPText(
        string name,
        Transform parent,
        string text,
        int size,
        Vector2 pos
    )
    {
        GameObject textGO =
            CreateUIObject(name, parent);

        TextMeshProUGUI tmp =
            textGO.AddComponent<TextMeshProUGUI>();

        tmp.text = text;
        tmp.fontSize = size;
        tmp.alignment = TextAlignmentOptions.Center;

        RectTransform rect =
            textGO.GetComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(300, 50);

        rect.anchoredPosition = pos;

        return textGO;
    }


    static GameObject CreateTMPInputField(Transform parent)
    {
        GameObject inputGO =
            CreateUIObject("CodeInputField", parent);

        TMP_InputField input =
            inputGO.AddComponent<TMP_InputField>();

        Image bg =
            inputGO.AddComponent<Image>();

        bg.color = Color.white;

        RectTransform rect =
            inputGO.GetComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(250, 50);


        GameObject textGO =
            CreateUIObject("Text", inputGO.transform);

        TextMeshProUGUI text =
            textGO.AddComponent<TextMeshProUGUI>();

        text.text = "";

        input.textComponent = text;

        return inputGO;
    }


    static GameObject CreateButton(Transform parent)
    {
        GameObject buttonGO =
            CreateUIObject("SubmitButton", parent);

        Button btn =
            buttonGO.AddComponent<Button>();

        Image img =
            buttonGO.AddComponent<Image>();

        img.color = Color.gray;

        RectTransform rect =
            buttonGO.GetComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(160, 50);


        GameObject label =
            CreateUIObject("Label", buttonGO.transform);

        TextMeshProUGUI text =
            label.AddComponent<TextMeshProUGUI>();

        text.text = "Unlock";

        text.alignment =
            TextAlignmentOptions.Center;

        RectTransform labelRect =
            label.GetComponent<RectTransform>();

        labelRect.sizeDelta =
            new Vector2(160, 50);

        return buttonGO;
    }
}

