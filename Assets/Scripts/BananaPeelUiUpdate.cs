using TMPro;
using UnityEngine;

public class BananaUIUpdate : MonoBehaviour
{
    
    public TextMeshProUGUI BananaText;

    void Start()
    {
        UpdateBananaUI();
    }

    void Update()
    {
        UpdateBananaUI();
    }

    void UpdateBananaUI()
    {
        if (Inventory.Instance == null)
            return;

        if (BananaText == null)
            return;

        BananaText.text =
            "x" + Inventory.Instance.bananaCount.ToString();
    }
}

