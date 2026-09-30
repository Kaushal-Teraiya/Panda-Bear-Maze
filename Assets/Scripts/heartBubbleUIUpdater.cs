using TMPro;
using UnityEngine;

public class heartBubbleUIUpdater : MonoBehaviour
{
    
    public TextMeshProUGUI HeartText;

    void Start()
    {
        UpdateHeartUI();
    }

    void Update()
    {
        UpdateHeartUI();
    }

    void UpdateHeartUI()
    {
        if (Inventory.Instance == null)
            return;

        if (HeartText == null)
            return;

        HeartText.text =
            "x" + Inventory.Instance.bubbleCount.ToString();
    }
}

