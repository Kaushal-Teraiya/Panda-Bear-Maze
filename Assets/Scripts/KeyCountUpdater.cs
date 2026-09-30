using UnityEngine;
using TMPro;

public class KeyCountUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI keyText;

    void Start()
    {
        UpdateKeyUI();
    }

    void Update()
    {
        UpdateKeyUI();
    }

    void UpdateKeyUI()
    {
        if (Inventory.Instance == null)
            return;

        if (keyText == null)
            return;

        keyText.text =
            "x" + Inventory.Instance.keyCount.ToString();
    }
}