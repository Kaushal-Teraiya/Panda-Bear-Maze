using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class GiftDisplayUIGenerator : EditorWindow
{
    Canvas targetCanvas;

    [MenuItem("Tools/Generate Gift Display UI")]
    static void ShowWindow()
    {
        GetWindow<GiftDisplayUIGenerator>(
            "Gift UI Generator"
        );
    }

    void OnGUI()
    {
        GUILayout.Label(
            "Generate Gift Display UI",
            EditorStyles.boldLabel
        );

        targetCanvas =
        (Canvas)EditorGUILayout.ObjectField(
            "Canvas",
            targetCanvas,
            typeof(Canvas),
            true
        );

        if (GUILayout.Button("Generate UI"))
        {
            GenerateUI();
        }
    }


    void GenerateUI()
    {
        if (targetCanvas == null)
        {
            Debug.LogError("Assign Canvas first!");
            return;
        }


        // PANEL
        GameObject panel =
        new GameObject("GiftDisplayPanel");

        panel.transform.SetParent(
            targetCanvas.transform,
            false
        );

        RectTransform panelRect =
            panel.AddComponent<RectTransform>();

        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;


        Image panelImage =
            panel.AddComponent<Image>();

        panelImage.color =
            new Color(0, 0, 0, 0.85f);


        // GIFT IMAGE
        GameObject imageGO =
        new GameObject("GiftImage");

        imageGO.transform.SetParent(
            panel.transform,
            false
        );

        RectTransform imageRect =
            imageGO.AddComponent<RectTransform>();

        imageRect.anchorMin =
            new Vector2(0.2f, 0.2f);

        imageRect.anchorMax =
            new Vector2(0.8f, 0.8f);

        imageRect.offsetMin = Vector2.zero;
        imageRect.offsetMax = Vector2.zero;

        Image giftImage =
            imageGO.AddComponent<Image>();


        // CLOSE BUTTON
        GameObject buttonGO =
        new GameObject("CloseButton");

        buttonGO.transform.SetParent(
            panel.transform,
            false
        );

        RectTransform buttonRect =
            buttonGO.AddComponent<RectTransform>();

        buttonRect.anchorMin =
            new Vector2(0.85f, 0.9f);

        buttonRect.anchorMax =
            new Vector2(0.95f, 0.98f);

        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;

        Image buttonImage =
            buttonGO.AddComponent<Image>();

        buttonImage.color = Color.white;


        Button button =
            buttonGO.AddComponent<Button>();


        // BUTTON TEXT
        GameObject textGO =
        new GameObject("Text");

        textGO.transform.SetParent(
            buttonGO.transform,
            false
        );

        RectTransform textRect =
            textGO.AddComponent<RectTransform>();

        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        var text =
            textGO.AddComponent<UnityEngine.UI.Text>();

        text.text = "X";
        text.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );
        text.alignment =
            TextAnchor.MiddleCenter;
        text.color = Color.black;


        // ADD DISPLAY SCRIPT
        GiftDisplayUI displayScript =
            panel.AddComponent<GiftDisplayUI>();

        displayScript.panel = panel;
        displayScript.giftImage = giftImage;


        // BUTTON EVENT
        button.onClick.AddListener(
            displayScript.CloseGift
        );


        panel.SetActive(false);


        Debug.Log(
            "Gift Display UI generated successfully!"
        );
    }
}