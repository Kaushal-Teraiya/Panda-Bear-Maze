using UnityEngine;
using UnityEngine.UI;

public class GiftDisplayUI : MonoBehaviour
{
    public static GiftDisplayUI Instance;

    public GameObject panel;

    public Image giftImage;


    void Awake()
    {
        Instance = this;

        if (panel != null)
            panel.SetActive(false);
    }


    public void ShowGift(Sprite sprite)
    {
        if (giftImage == null)
        {
            Debug.LogError("GiftImage not assigned!");
            return;
        }

        giftImage.sprite = sprite;

        if (panel != null)
            panel.SetActive(true);

        Time.timeScale = 0f;
    }


    public void CloseGift()
    {
        if (panel != null)
            panel.SetActive(false);

        Time.timeScale = 1f;
    }
}