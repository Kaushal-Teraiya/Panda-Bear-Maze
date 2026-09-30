using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class TutorialPanelGenerator : EditorWindow
{
    Canvas targetCanvas;

    int tutorialRows = 3;

    Sprite[] rowIcons;
    string[] rowTexts;

    string noteText =
        "Note: Some collectibles are traps disguised as rewards.";


    [MenuItem("Tools/Generate Tutorial Panel")]
    static void Init()
    {
        GetWindow<TutorialPanelGenerator>(
            "Tutorial Panel Generator"
        );
    }


    void OnEnable()
    {
        ResizeArrays();
    }


    void ResizeArrays()
    {
        rowIcons = new Sprite[tutorialRows];
        rowTexts = new string[tutorialRows];
    }


    void OnGUI()
    {
        GUILayout.Label(
            "Tutorial Panel Generator",
            EditorStyles.boldLabel
        );


        targetCanvas =
        (Canvas)EditorGUILayout.ObjectField(
            "Canvas",
            targetCanvas,
            typeof(Canvas),
            true
        );


        int newRowCount =
        EditorGUILayout.IntField(
            "Number Of Rows",
            tutorialRows
        );


        if (newRowCount != tutorialRows)
        {
            tutorialRows = newRowCount;
            ResizeArrays();
        }


        GUILayout.Space(10);


        for (int i = 0; i < tutorialRows; i++)
        {
            EditorGUILayout.LabelField(
                "Row " + (i + 1),
                EditorStyles.boldLabel
            );


            rowIcons[i] =
            (Sprite)EditorGUILayout.ObjectField(
                "Icon",
                rowIcons[i],
                typeof(Sprite),
                false
            );


            rowTexts[i] =
            EditorGUILayout.TextField(
                "Description",
                rowTexts[i]
            );


            GUILayout.Space(5);
        }


        GUILayout.Space(10);


        noteText =
        EditorGUILayout.TextField(
            "Warning Note",
            noteText
        );


        GUILayout.Space(15);


        if (GUILayout.Button(
            "Generate Tutorial Panel"
        ))
        {
            Generate();
        }
    }



    void Generate()
    {
        if (targetCanvas == null)
        {
            Debug.LogError(
                "Assign Canvas first!"
            );

            return;
        }


        GameObject panel =
        new GameObject("TutorialPanel");


        panel.transform.SetParent(
            targetCanvas.transform,
            false
        );


        RectTransform panelRect =
        panel.AddComponent<RectTransform>();


        panelRect.anchorMin =
        new Vector2(.2f, .15f);


        panelRect.anchorMax =
        new Vector2(.8f, .85f);


        panelRect.offsetMin =
        Vector2.zero;


        panelRect.offsetMax =
        Vector2.zero;


        Image bg =
        panel.AddComponent<Image>();


        bg.color =
        new Color(0, 0, 0, .88f);



        ScrollRect scroll =
        panel.AddComponent<ScrollRect>();


        scroll.horizontal = false;



        GameObject viewport =
        new GameObject("Viewport");


        viewport.transform.SetParent(
            panel.transform, false
        );


        RectTransform vpRect =
        viewport.AddComponent<RectTransform>();


        vpRect.anchorMin =
        Vector2.zero;


        vpRect.anchorMax =
        Vector2.one;


        vpRect.offsetMin =
        Vector2.zero;


        vpRect.offsetMax =
        Vector2.zero;


        Mask mask =
        viewport.AddComponent<Mask>();


        mask.showMaskGraphic = false;


        Image vpImage =
        viewport.AddComponent<Image>();


        vpImage.color =
        Color.clear;



        GameObject content =
        new GameObject("Content");


        content.transform.SetParent(
            viewport.transform, false
        );


        RectTransform contentRect =
        content.AddComponent<RectTransform>();


        contentRect.anchorMin = new Vector2(0, 0);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.offsetMin = Vector2.zero;
        contentRect.offsetMax = Vector2.zero;


        contentRect.pivot =
        new Vector2(.5f, 1);


        contentRect.anchoredPosition =
        Vector2.zero;



        VerticalLayoutGroup layout =
        content.AddComponent<
        VerticalLayoutGroup>();


        layout.spacing = 25;


        layout.padding =
        new RectOffset(40, 40, 40, 40);


        layout.childControlWidth = true;


        layout.childControlHeight = true;


        layout.childForceExpandWidth = true;


        layout.childForceExpandHeight = false;



        ContentSizeFitter fitter =
        content.AddComponent<
        ContentSizeFitter>();


        fitter.verticalFit =
        ContentSizeFitter.FitMode.PreferredSize;



        scroll.viewport =
        vpRect;


        scroll.content =
        contentRect;



        for (int i = 0; i < tutorialRows; i++)
        {
            CreateRow(
                content.transform,
                rowIcons[i],
                rowTexts[i]
            );
        }


        CreateWarningNote(
            content.transform,
            noteText
        );


        CreateBackButton(
            content.transform,
            panel
        );


        MainMenuController controller =
        targetCanvas.GetComponentInChildren<
        MainMenuController>();


        if (controller != null)
        {
            controller.tutorialPanel =
            panel;
        }


        panel.SetActive(false);


        Debug.Log(
            "Tutorial panel generated successfully!"
        );
    }



    void CreateRow(
     Transform parent,
     Sprite icon,
     string description
 )
    {
        GameObject row =
        new GameObject("TutorialRow");

        row.transform.SetParent(parent, false);


        RectTransform rowRect =
        row.AddComponent<RectTransform>();


        rowRect.anchorMin = new Vector2(0, 1);
        rowRect.anchorMax = new Vector2(1, 1);
        rowRect.pivot = new Vector2(.5f, .5f);


        LayoutElement layoutElement =
        row.AddComponent<LayoutElement>();

        layoutElement.preferredHeight = 90;



        HorizontalLayoutGroup layout =
        row.AddComponent<HorizontalLayoutGroup>();


        layout.spacing = 25;

        layout.childControlWidth = true;

        layout.childForceExpandWidth = true;



        // ICON
        GameObject iconObj =
        new GameObject("Icon");

        iconObj.transform.SetParent(row.transform, false);


        RectTransform iconRect =
        iconObj.AddComponent<RectTransform>();


        iconRect.sizeDelta = new Vector2(70, 70);


        Image iconImg =
        iconObj.AddComponent<Image>();


        iconImg.sprite = icon;



        LayoutElement iconLayout =
        iconObj.AddComponent<LayoutElement>();


        iconLayout.preferredWidth = 70;
        iconLayout.preferredHeight = 70;



        // TEXT
        GameObject textObj =
        new GameObject("Text");

        textObj.transform.SetParent(row.transform, false);


        RectTransform textRect =
        textObj.AddComponent<RectTransform>();


        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;



        Text txt =
        textObj.AddComponent<Text>();


        txt.text = description;


        txt.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );


        txt.fontSize = 34;

        txt.color = Color.white;

        txt.alignment =
        TextAnchor.MiddleLeft;



        LayoutElement textLayout =
        textObj.AddComponent<LayoutElement>();


        textLayout.flexibleWidth = 1;



        Outline outline =
        textObj.AddComponent<Outline>();


        outline.effectColor = Color.black;

        outline.effectDistance =
        new Vector2(2, 2);
    }
    void CreateWarningNote(
        Transform parent,
        string note
    )
    {
        GameObject noteObj =
        new GameObject("WarningNote");


        noteObj.transform.SetParent(
            parent, false
        );


        LayoutElement height =
        noteObj.AddComponent<LayoutElement>();


        height.minHeight = 70;



        Text txt =
        noteObj.AddComponent<Text>();


        txt.text = note;


        txt.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );


        txt.fontSize = 26;


        txt.fontStyle =
        FontStyle.Italic;


        txt.alignment =
        TextAnchor.MiddleCenter;


        txt.color =
        new Color(1f, .8f, .8f);
    }



    void CreateBackButton(
        Transform parent,
        GameObject panel
    )
    {
        GameObject btn =
        new GameObject("BackButton");


        btn.transform.SetParent(
            parent, false
        );


        LayoutElement height =
        btn.AddComponent<LayoutElement>();


        height.minHeight = 80;



        Image img =
        btn.AddComponent<Image>();


        img.color =
        new Color(.2f, .2f, .25f, .95f);


        Button button =
        btn.AddComponent<Button>();


        button.onClick.AddListener(
            () => panel.SetActive(false)
        );



        GameObject txt =
        new GameObject("Text");


        txt.transform.SetParent(
            btn.transform, false
        );


        RectTransform txtRect =
        txt.AddComponent<RectTransform>();


        txtRect.anchorMin =
        Vector2.zero;


        txtRect.anchorMax =
        Vector2.one;


        txtRect.offsetMin =
        Vector2.zero;


        txtRect.offsetMax =
        Vector2.zero;



        Text label =
        txt.AddComponent<Text>();


        label.text = "Back";


        label.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );


        label.fontSize = 36;


        label.alignment =
        TextAnchor.MiddleCenter;


        label.color =
        Color.white;
    }
}