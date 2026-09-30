using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CreateTrapButtonsUI : EditorWindow
{
    Canvas targetCanvas;

    [MenuItem("Tools/Create Vertical Trap Buttons UI")]
    public static void ShowWindow()
    {
        GetWindow<CreateTrapButtonsUI>(
            "Trap Buttons UI"
        );
    }

    void OnGUI()
    {
        GUILayout.Label(
            "Generate Vertical Trap Buttons",
            EditorStyles.boldLabel
        );

        targetCanvas =
        (Canvas)EditorGUILayout.ObjectField(
            "Target Canvas",
            targetCanvas,
            typeof(Canvas),
            true
        );

        if (GUILayout.Button(
            "Create Vertical Trap UI"
        ))
        {
            CreateUI();
        }
    }


    void CreateUI()
    {
        if (targetCanvas == null)
        {
            Debug.LogError(
                "Assign Canvas first!"
            );

            return;
        }


        // PANEL
        GameObject panel =
        new GameObject("TrapButtonsPanel");

        panel.transform.SetParent(
            targetCanvas.transform,
            false
        );


        RectTransform panelRect =
        panel.AddComponent<RectTransform>();


        // anchor RIGHT middle
        panelRect.anchorMin =
        new Vector2(1f, 0.5f);

        panelRect.anchorMax =
        new Vector2(1f, 0.5f);

        panelRect.pivot =
        new Vector2(1f, 0.5f);


        panelRect.anchoredPosition =
        new Vector2(-80f, 0f);


        Image bg =
        panel.AddComponent<Image>();

        bg.color =
        new Color(0, 0, 0, 0.35f);


        VerticalLayoutGroup layout =
        panel.AddComponent<VerticalLayoutGroup>();


        layout.spacing = 25;

        layout.childAlignment =
        TextAnchor.MiddleCenter;

        layout.childForceExpandWidth = false;

        layout.childForceExpandHeight = false;


        ContentSizeFitter fitter =
        panel.AddComponent<ContentSizeFitter>();

        fitter.horizontalFit =
        ContentSizeFitter.FitMode.PreferredSize;

        fitter.verticalFit =
        ContentSizeFitter.FitMode.PreferredSize;


        CreateTrapButton(
            panel.transform,
            "HeartTrapButton"
        );

        CreateTrapButton(
            panel.transform,
            "BananaTrapButton"
        );


        Debug.Log(
            "Vertical Trap UI Created Successfully"
        );
    }


    void CreateTrapButton(
        Transform parent,
        string name
    )
    {
        GameObject buttonObj =
        new GameObject(name);

        buttonObj.transform.SetParent(
            parent,
            false
        );


        RectTransform rect =
        buttonObj.AddComponent<RectTransform>();

        rect.sizeDelta =
        new Vector2(140, 140);


        Image img =
        buttonObj.AddComponent<Image>();

        img.color =
        new Color(1f, 1f, 1f, 0.95f);


        Button btn =
        buttonObj.AddComponent<Button>();


        // ICON CHILD
        GameObject icon =
        new GameObject("Icon");

        icon.transform.SetParent(
            buttonObj.transform,
            false
        );


        RectTransform iconRect =
        icon.AddComponent<RectTransform>();


        iconRect.anchorMin =
        Vector2.zero;

        iconRect.anchorMax =
        Vector2.one;

        iconRect.offsetMin =
        new Vector2(10, 10);

        iconRect.offsetMax =
        new Vector2(-10, -10);


        Image iconImage =
        icon.AddComponent<Image>();

        iconImage.color = Color.white;
    }
}