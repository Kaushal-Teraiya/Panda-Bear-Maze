using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SimpleScrollTutorialPanel : EditorWindow
{
    Canvas canvas;

    [MenuItem("Tools/Create SIMPLE Scroll Tutorial Panel")]
    static void Open()
    {
        GetWindow<SimpleScrollTutorialPanel>();
    }

    void OnGUI()
    {
        canvas =
        (Canvas)EditorGUILayout.ObjectField(
            "Canvas",
            canvas,
            typeof(Canvas),
            true
        );

        if(GUILayout.Button("Create Panel"))
        {
            CreatePanel();
        }
    }


    void CreatePanel()
    {
        if(canvas == null)
        {
            Debug.LogError("Assign Canvas first");
            return;
        }


        // PANEL
        GameObject panel =
        new GameObject("TutorialPanel");

        panel.transform.SetParent(
            canvas.transform,
            false
        );

        RectTransform panelRect =
        panel.AddComponent<RectTransform>();

        panelRect.anchorMin =
        new Vector2(.2f,.2f);

        panelRect.anchorMax =
        new Vector2(.8f,.8f);

        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelImage =
        panel.AddComponent<Image>();

        panelImage.color =
        new Color(0,0,0,.85f);


        // SCROLLRECT
        ScrollRect scroll =
        panel.AddComponent<ScrollRect>();

        scroll.horizontal = false;


        // VIEWPORT
        GameObject viewport =
        new GameObject("Viewport");

        viewport.transform.SetParent(
            panel.transform,
            false
        );

        RectTransform vpRect =
        viewport.AddComponent<RectTransform>();

        vpRect.anchorMin = Vector2.zero;
        vpRect.anchorMax = Vector2.one;
        vpRect.offsetMin = Vector2.zero;
        vpRect.offsetMax = Vector2.zero;

        Image vpImage =
        viewport.AddComponent<Image>();

        vpImage.color = Color.clear;

        Mask mask =
        viewport.AddComponent<Mask>();

        mask.showMaskGraphic = false;


        // CONTENT
        GameObject content =
        new GameObject("Content");

        content.transform.SetParent(
            viewport.transform,
            false
        );

        RectTransform contentRect =
        content.AddComponent<RectTransform>();

        contentRect.anchorMin =
        new Vector2(0,1);

        contentRect.anchorMax =
        new Vector2(1,1);

        contentRect.pivot =
        new Vector2(.5f,1);

        contentRect.sizeDelta =
        new Vector2(0,600);


        scroll.viewport = vpRect;
        scroll.content = contentRect;


        // TEST ROWS
        for(int i=0;i<5;i++)
        {
            CreateRow(content.transform,i);
        }


        Debug.Log("Simple ScrollRect panel created.");
    }



    void CreateRow(
        Transform parent,
        int index
    )
    {
        GameObject row =
        new GameObject("Row_" + index);

        row.transform.SetParent(
            parent,
            false
        );

        RectTransform rect =
        row.AddComponent<RectTransform>();

        rect.anchorMin =
        new Vector2(0,1);

        rect.anchorMax =
        new Vector2(1,1);

        rect.pivot =
        new Vector2(.5f,1);

        rect.sizeDelta =
        new Vector2(0,80);

        rect.anchoredPosition =
        new Vector2(0,-index * 90);


        GameObject textObj =
        new GameObject("Text");

        textObj.transform.SetParent(
            row.transform,
            false
        );

        RectTransform textRect =
        textObj.AddComponent<RectTransform>();

        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text label =
        textObj.AddComponent<Text>();

        label.text =
        "Tutorial item " + (index+1);

        label.font =
        Resources.GetBuiltinResource<Font>(
            "LegacyRuntime.ttf"
        );

        label.alignment =
        TextAnchor.MiddleLeft;

        label.fontSize = 36;

        label.color = Color.white;
    }
}