using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PauseMenuUIGenerator : EditorWindow
{
    Canvas targetCanvas;

    Sprite questionMarkSprite;

    int gridRows = 2;

    int gridColumns = 4;


    [MenuItem("Tools/Generate Pause Menu UI")]
    static void Init()
    {
        GetWindow<PauseMenuUIGenerator>(
            "Pause Menu Generator"
        );
    }


    void OnGUI()
    {
        GUILayout.Label(
            "Pause Menu Generator",
            EditorStyles.boldLabel
        );

        targetCanvas =
        (Canvas)EditorGUILayout.ObjectField(
            "Canvas",
            targetCanvas,
            typeof(Canvas),
            true
        );

        questionMarkSprite =
        (Sprite)EditorGUILayout.ObjectField(
            "Question Mark Sprite",
            questionMarkSprite,
            typeof(Sprite),
            false
        );

        gridRows =
        EditorGUILayout.IntField(
            "Rows",
            gridRows
        );

        gridColumns =
        EditorGUILayout.IntField(
            "Columns",
            gridColumns
        );


        if (GUILayout.Button(
            "Generate Pause Menu UI"
        ))
        {
            Generate();
        }
    }


    void Generate()
    {
        if (targetCanvas == null)
        {
            Debug.LogError("Assign canvas first!");
            return;
        }


        GameObject pausePanel =
        new GameObject("PausePanel");

        pausePanel.transform.SetParent(
            targetCanvas.transform,
            false
        );


        RectTransform panelRect =
            pausePanel.AddComponent<RectTransform>();

        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;

        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;


        Image bg =
            pausePanel.AddComponent<Image>();

        bg.color =
            new Color(0,0,0,0.85f);


        PauseMenuUI pauseScript =
            pausePanel.AddComponent<PauseMenuUI>();


        CreateButton(
            pausePanel.transform,
            "Resume",
            new Vector2(0.5f,0.7f),
            pauseScript.ResumeGame
        );


        CreateButton(
            pausePanel.transform,
            "Obtained Items",
            new Vector2(0.5f,0.55f),
            pauseScript.ShowObtainedItems
        );


        CreateButton(
            pausePanel.transform,
            "Quit",
            new Vector2(0.5f,0.4f),
            pauseScript.QuitGame
        );


        GameObject obtainedPanel =
        CreateObtainedItemsGrid(
            pausePanel.transform
        );


        pauseScript.pausePanel =
            pausePanel;

        pauseScript.obtainedItemsPanel =
            obtainedPanel;


        Debug.Log(
            "Pause menu UI generated successfully!"
        );
    }


    GameObject CreateObtainedItemsGrid(
        Transform parent
    )
    {
        GameObject gridPanel =
        new GameObject("ObtainedItemsPanel");

        gridPanel.transform.SetParent(
            parent,
            false
        );


        RectTransform rect =
            gridPanel.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.25f,0.15f);

        rect.anchorMax =
            new Vector2(0.75f,0.85f);

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;


        GridLayoutGroup grid =
            gridPanel.AddComponent<GridLayoutGroup>();

        grid.cellSize =
            new Vector2(120,120);

        grid.spacing =
            new Vector2(15,15);


        ObtainedItemsGrid gridScript =
            gridPanel.AddComponent<ObtainedItemsGrid>();


        int total =
            gridRows * gridColumns;

        gridScript.slots =
            new Image[total];


        for(int i=0;i<total;i++)
        {
            GameObject slot =
            new GameObject(
                "Slot_" + i
            );

            slot.transform.SetParent(
                gridPanel.transform,
                false
            );


            Image img =
                slot.AddComponent<Image>();


            gridScript.slots[i] = img;
        }


        gridScript.questionMarkSprite =
            questionMarkSprite;


        gridPanel.SetActive(false);

        return gridPanel;
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
            new Vector2(260,80);


        Image img =
            btn.AddComponent<Image>();

        img.color = Color.white;


        Button button =
            btn.AddComponent<Button>();

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

        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;


        Text label =
            txt.AddComponent<Text>();

        label.text = text;

        label.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        label.alignment =
            TextAnchor.MiddleCenter;

        label.color = Color.black;
    }
}