using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class StoryPanelGenerator : EditorWindow
{
    Canvas targetCanvas;

    [MenuItem("Tools/Generate Story Panel")]
    static void Init()
    {
        GetWindow<StoryPanelGenerator>(
            "Story Panel Generator"
        );
    }

    void OnGUI()
    {
        GUILayout.Label(
            "Generate Story Panel",
            EditorStyles.boldLabel
        );

        targetCanvas =
        (Canvas)EditorGUILayout.ObjectField(
            "Canvas",
            targetCanvas,
            typeof(Canvas),
            true
        );

        if(GUILayout.Button("Create Story Panel"))
        {
            Generate();
        }
    }

    void Generate()
    {
        if(targetCanvas == null)
        {
            Debug.LogError("Assign canvas first!");
            return;
        }

        GameObject panel =
        new GameObject("StoryPanel");

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
        new Color(0,0,0,0.85f);


        StoryPanelController controller =
        panel.AddComponent<StoryPanelController>();

        controller.panel = panel;


        CreateStoryText(panel.transform);

        CreateButton(panel.transform, controller);

        Debug.Log("Story panel created successfully!");
    }


    void CreateStoryText(Transform parent)
    {
        GameObject textObj =
        new GameObject("StoryText");

        textObj.transform.SetParent(parent,false);

        RectTransform rect =
        textObj.AddComponent<RectTransform>();

        rect.anchorMin =
        new Vector2(0.2f,0.4f);

        rect.anchorMax =
        new Vector2(0.8f,0.7f);


        Text story =
        textObj.AddComponent<Text>();

        story.text =
        "Panda asked you for a kiss.\n\n" +
        "You said no.\n\n" +
        "Panda got upset and ran into the maze.\n\n" +
        "Now you must find him and make things right.";

        story.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );

        story.fontSize = 42;

        story.alignment =
        TextAnchor.MiddleCenter;

        story.color = Color.white;
    }


    void CreateButton(
        Transform parent,
        StoryPanelController controller
    )
    {
        GameObject btn =
        new GameObject("ContinueButton");

        btn.transform.SetParent(parent,false);

        RectTransform rect =
        btn.AddComponent<RectTransform>();

        rect.anchorMin =
        new Vector2(0.5f,0.2f);

        rect.anchorMax =
        new Vector2(0.5f,0.2f);

        rect.sizeDelta =
        new Vector2(260,80);


        Image img =
        btn.AddComponent<Image>();

        img.color =
        new Color(0.2f,0.6f,1f,1f);


        Button button =
        btn.AddComponent<Button>();

        button.onClick.AddListener(
            controller.ContinueGame
        );


        GameObject txt =
        new GameObject("Text");

        txt.transform.SetParent(btn.transform,false);

        RectTransform txtRect =
        txt.AddComponent<RectTransform>();

        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;


        Text label =
        txt.AddComponent<Text>();

        label.text = "Continue";

        label.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );

        label.alignment =
        TextAnchor.MiddleCenter;

        label.fontSize = 38;

        label.color = Color.white;
    }
}