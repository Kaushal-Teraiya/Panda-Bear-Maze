using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class CreateInventoryBarUI : EditorWindow
{
    Canvas targetCanvas;

    int slotCount = 4;

    string slotPrefix = "ItemSlot";


    [MenuItem("Tools/Create Inventory Bar UI")]
    static void ShowWindow()
    {
        GetWindow<CreateInventoryBarUI>(
            "Inventory Bar Generator"
        );
    }


    void OnGUI()
    {
        GUILayout.Label(
            "Inventory Bar Generator",
            EditorStyles.boldLabel
        );


        targetCanvas =
        (Canvas)EditorGUILayout.ObjectField(
            "Target Canvas",
            targetCanvas,
            typeof(Canvas),
            true
        );


        slotCount =
        EditorGUILayout.IntSlider(
            "Slot Count",
            slotCount,
            1,
            12
        );


        slotPrefix =
        EditorGUILayout.TextField(
            "Slot Name Prefix",
            slotPrefix
        );


        if (GUILayout.Button("Generate Inventory Bar"))
        {
            GenerateInventoryBar();
        }
    }


    void GenerateInventoryBar()
    {
        if (targetCanvas == null)
        {
            Debug.LogError(
                "Assign a Canvas first!"
            );
            return;
        }


        GameObject panel =
        CreateUIObject(
            "InventoryBar",
            targetCanvas.transform
        );


        RectTransform rect =
        panel.GetComponent<RectTransform>();

        rect.anchorMin =
        rect.anchorMax =
        new Vector2(0.5f, 1);

        rect.pivot =
        new Vector2(0.5f, 1);

        rect.anchoredPosition =
        new Vector2(0, -40);

        rect.sizeDelta =
        new Vector2(700, 100);


        Image bg =
        panel.AddComponent<Image>();

        bg.color =
        new Color(0.05f, 0.05f, 0.05f, 0.75f);


        HorizontalLayoutGroup layout =
        panel.AddComponent<HorizontalLayoutGroup>();

        layout.spacing = 15;

        layout.padding =
        new RectOffset(25, 25, 20, 20);

        layout.childAlignment =
        TextAnchor.MiddleCenter;

        layout.childForceExpandWidth = false;

        layout.childForceExpandHeight = false;


        for (int i = 0; i < slotCount; i++)
        {
            CreateSlot(
                panel.transform,
                slotPrefix + "_" + i
            );
        }


        Selection.activeGameObject = panel;


        Debug.Log(
            "Inventory bar created with "
            + slotCount +
            " slots."
        );
    }


    GameObject CreateSlot(
        Transform parent,
        string name
    )
    {
        GameObject slot =
        CreateUIObject(name, parent);


        RectTransform rect =
        slot.GetComponent<RectTransform>();

        rect.sizeDelta =
        new Vector2(75, 75);


        Image frame =
        slot.AddComponent<Image>();

        frame.color =
        new Color(1f, 1f, 1f, 0.18f);


        GameObject icon =
        CreateUIObject(
            "Icon",
            slot.transform
        );


        Image iconImg =
        icon.AddComponent<Image>();

        iconImg.color =
        new Color(1f, 1f, 1f, 0.95f);


        RectTransform iconRect =
        icon.GetComponent<RectTransform>();

        iconRect.sizeDelta =
        new Vector2(42, 42);

        iconRect.anchoredPosition =
        new Vector2(0, 8);


        GameObject count =
        CreateUIObject(
            "CountText",
            slot.transform
        );


        TextMeshProUGUI text =
        count.AddComponent<TextMeshProUGUI>();

        text.text = "x0";

        text.fontSize = 22;

        text.alignment =
        TextAlignmentOptions.Center;


        RectTransform textRect =
        count.GetComponent<RectTransform>();

        textRect.sizeDelta =
        new Vector2(60, 25);

        textRect.anchoredPosition =
        new Vector2(0, -25);


        return slot;
    }


    GameObject CreateUIObject(
        string name,
        Transform parent
    )
    {
        GameObject obj =
        new GameObject(name);

        obj.transform.SetParent(parent);

        RectTransform rect =
        obj.AddComponent<RectTransform>();

        rect.localScale =
        Vector3.one;

        rect.anchorMin =
        rect.anchorMax =
        new Vector2(0.5f, 0.5f);

        rect.anchoredPosition =
        Vector2.zero;

        return obj;
    }
}