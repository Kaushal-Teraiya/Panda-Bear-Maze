using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class StartMenuUIGenerator : EditorWindow
{
    Canvas targetCanvas;

    Sprite backgroundSprite;

    string gameTitle = "MAZE D'AMOUR";


    [MenuItem("Tools/Generate Start Menu UI")]
    static void Init()
    {
        GetWindow<StartMenuUIGenerator>(
            "Start Menu Generator"
        );
    }


    void OnGUI()
    {
        GUILayout.Label(
            "Start Menu Generator",
            EditorStyles.boldLabel
        );


        targetCanvas =
        (Canvas)EditorGUILayout.ObjectField(
            "Canvas",
            targetCanvas,
            typeof(Canvas),
            true
        );


        backgroundSprite =
        (Sprite)EditorGUILayout.ObjectField(
            "Background Image",
            backgroundSprite,
            typeof(Sprite),
            false
        );


        gameTitle =
        EditorGUILayout.TextField(
            "Game Title",
            gameTitle
        );


        if(GUILayout.Button(
            "Generate Start Menu UI"
        ))
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


        GameObject menuPanel =
        new GameObject("MainMenuPanel");

        menuPanel.transform.SetParent(
            targetCanvas.transform,
            false
        );


        RectTransform panelRect =
        menuPanel.AddComponent<RectTransform>();

        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;

        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;


        Image bg =
        menuPanel.AddComponent<Image>();


        if(backgroundSprite != null)
        {
            bg.sprite = backgroundSprite;
            bg.color = Color.white;
        }
        else
        {
            bg.color =
            new Color(0.05f,0.07f,0.12f);
        }


        // readability overlay
        GameObject overlay =
        new GameObject("Overlay");

        overlay.transform.SetParent(
            menuPanel.transform,
            false
        );

        Image overlayImg =
        overlay.AddComponent<Image>();

        overlayImg.color =
        new Color(0,0,0,0.25f);

        RectTransform overlayRect =
        overlay.GetComponent<RectTransform>();

        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;

        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;


        MainMenuController controller =
        menuPanel.AddComponent<MainMenuController>();


        CreateTitle(menuPanel.transform);


        CreateButton(
            menuPanel.transform,
            "Play",
            new Vector2(0.5f,0.30f),
            controller.Play
        );


        CreateButton(
            menuPanel.transform,
            "Tutorial",
            new Vector2(0.5f,0.20f),
            controller.Tutorial
        );


        CreateButton(
            menuPanel.transform,
            "Quit",
            new Vector2(0.5f,0.10f),
            controller.Quit
        );


        GameObject tutorialPanel =
        CreateTutorialPanel(
            menuPanel.transform,
            controller
        );


        controller.tutorialPanel =
        tutorialPanel;


        Debug.Log(
            "Start menu generated successfully!"
        );
    }



    void CreateTitle(Transform parent)
    {
        GameObject title =
        new GameObject("Title");


        title.transform.SetParent(
            parent,
            false
        );


        RectTransform rect =
        title.AddComponent<RectTransform>();


        rect.anchorMin =
        new Vector2(0.2f,0.82f);


        rect.anchorMax =
        new Vector2(0.8f,0.95f);


        Text txt =
        title.AddComponent<Text>();


        txt.text = gameTitle;


        txt.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );


        txt.fontSize = 92;


        txt.fontStyle =
        FontStyle.Bold;


        txt.alignment =
        TextAnchor.MiddleCenter;


        txt.color =
        new Color(0.95f,0.75f,0.85f);
    }



    GameObject CreateTutorialPanel(
        Transform parent,
        MainMenuController controller
    )
    {
        GameObject tutorialPanel =
        new GameObject("TutorialPanel");


        tutorialPanel.transform.SetParent(
            parent,
            false
        );


        RectTransform rect =
        tutorialPanel.AddComponent<RectTransform>();


        rect.anchorMin =
        new Vector2(0.25f,0.2f);


        rect.anchorMax =
        new Vector2(0.75f,0.8f);


        Image bg =
        tutorialPanel.AddComponent<Image>();


        bg.color =
        new Color(0,0,0,0.85f);



        GameObject textObj =
        new GameObject("TutorialText");


        textObj.transform.SetParent(
            tutorialPanel.transform,
            false
        );


        RectTransform txtRect =
        textObj.AddComponent<RectTransform>();


        txtRect.anchorMin =
        new Vector2(0.1f,0.35f);


        txtRect.anchorMax =
        new Vector2(0.9f,0.8f);


        Text txt =
        textObj.AddComponent<Text>();


        txt.text =
        "Explore the maze\nCollect items\nAvoid traps";


        txt.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );


        txt.fontSize = 32;


        txt.alignment =
        TextAnchor.MiddleCenter;


        txt.color =
        new Color(0.92f,0.92f,0.95f);



        CreateButton(
            tutorialPanel.transform,
            "Back",
            new Vector2(0.5f,0.15f),
            controller.Back
        );


        tutorialPanel.SetActive(false);


        return tutorialPanel;
    }



    void CreateButton(
        Transform parent,
        string text,
        Vector2 anchor,
        UnityEngine.Events.UnityAction action
    )
    {
        GameObject btn =
        new GameObject(text);


        btn.transform.SetParent(
            parent,
            false
        );


        RectTransform rect =
        btn.AddComponent<RectTransform>();


        rect.anchorMin = anchor;
        rect.anchorMax = anchor;


        rect.sizeDelta =
        new Vector2(320,85);


        Image img =
        btn.AddComponent<Image>();


        img.color =
        new Color(0.15f,0.15f,0.18f,0.85f);


        Button button =
        btn.AddComponent<Button>();


        ColorBlock colors =
        button.colors;


        colors.highlightedColor =
        new Color(0.35f,0.35f,0.40f);


        colors.pressedColor =
        new Color(0.1f,0.1f,0.12f);


        button.colors = colors;


        button.onClick.AddListener(action);



        GameObject txt =
        new GameObject("Text");


        txt.transform.SetParent(
            btn.transform,
            false
        );


        RectTransform txtRect =
        txt.AddComponent<RectTransform>();


        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;


        Text label =
        txt.AddComponent<Text>();


        label.text = text;


        label.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );


        label.fontSize = 36;


        label.fontStyle =
        FontStyle.Bold;


        label.alignment =
        TextAnchor.MiddleCenter;


        label.color =
        new Color(0.92f,0.92f,0.95f);
    }
}