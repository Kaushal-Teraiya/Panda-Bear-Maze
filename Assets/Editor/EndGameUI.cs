using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class EndGameUIGenerator : EditorWindow
{
    Canvas targetCanvas;

    string endMessage =
        "Made with love by yours truly Kaushal\n\nI love you Bambu. Never leave my side.";

    [MenuItem("Tools/Generate End Game UI")]
    static void Init()
    {
        GetWindow<EndGameUIGenerator>("End Game UI Generator");
    }

    void OnGUI()
    {
        GUILayout.Label(
            "End Game UI Generator",
            EditorStyles.boldLabel
        );

        targetCanvas =
        (Canvas)EditorGUILayout.ObjectField(
            "Canvas",
            targetCanvas,
            typeof(Canvas),
            true
        );

        GUILayout.Space(10);

        endMessage =
        EditorGUILayout.TextArea(
            endMessage,
            GUILayout.Height(80)
        );

        GUILayout.Space(10);

        if (GUILayout.Button("Generate End Game UI"))
        {
            Generate();
        }
    }

    void Generate()
    {
        if (targetCanvas == null)
        {
            Debug.LogError("Assign Canvas first!");
            return;
        }

        GameObject panel =
        new GameObject("EndGamePanel");

        panel.transform.SetParent(
            targetCanvas.transform,
            false
        );

        RectTransform rect =
        panel.AddComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;


        Image bg =
        panel.AddComponent<Image>();

        bg.color =
        new Color(0,0,0,0.88f);


        GameObject textObj =
        new GameObject("MessageText");

        textObj.transform.SetParent(
            panel.transform,
            false
        );


        RectTransform textRect =
        textObj.AddComponent<RectTransform>();

        textRect.anchorMin =
        new Vector2(0.2f,0.3f);

        textRect.anchorMax =
        new Vector2(0.8f,0.7f);


        Text text =
        textObj.AddComponent<Text>();

        text.text = endMessage;

        text.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );

        text.fontSize = 46;

        text.alignment =
        TextAnchor.MiddleCenter;

        text.color =
        new Color(1f,0.85f,0.95f);


        Outline outline =
        textObj.AddComponent<Outline>();

        outline.effectColor =
        new Color(0f,0f,0f,0.9f);

        outline.effectDistance =
        new Vector2(2,-2);


        GameObject button =
        CreateButton(
            panel.transform,
            "Finish",
            new Vector2(0.5f,0.18f)
        );


        button.GetComponent<Button>()
        .onClick.AddListener(() =>
        {
            Debug.Log("End Screen Closed");
            panel.SetActive(false);
        });


        Debug.Log("End Game UI created successfully 💛");
    }


    GameObject CreateButton(
        Transform parent,
        string label,
        Vector2 anchor
    )
    {
        GameObject btn =
        new GameObject(label);

        btn.transform.SetParent(
            parent,
            false
        );

        RectTransform rect =
        btn.AddComponent<RectTransform>();

        rect.anchorMin = anchor;
        rect.anchorMax = anchor;

        rect.sizeDelta =
        new Vector2(260,75);


        Image img =
        btn.AddComponent<Image>();

        img.color =
        new Color(0.15f,0.18f,0.28f);


        Button button =
        btn.AddComponent<Button>();


        GameObject textObj =
        new GameObject("Text");

        textObj.transform.SetParent(
            btn.transform,
            false
        );


        RectTransform txtRect =
        textObj.AddComponent<RectTransform>();

        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;


        Text txt =
        textObj.AddComponent<Text>();

        txt.text = label;

        txt.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );

        txt.fontSize = 32;

        txt.alignment =
        TextAnchor.MiddleCenter;

        txt.color = Color.white;


        return btn;
    }
}