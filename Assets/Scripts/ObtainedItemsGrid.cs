using UnityEngine;
using UnityEngine.UI;

public class ObtainedItemsGrid : MonoBehaviour
{
    public Image[] slots;

    public Sprite questionMarkSprite;

    public Sprite[] obtainedSprites;


    void Start()
    {
        RefreshGrid();
    }


    public void RefreshGrid()
    {
        var gifts =
            GiftInventory.Instance.obtainedGifts;


        for (int i = 0; i < slots.Length; i++)
        {
            if (i < gifts.Count)
            {
                slots[i].sprite = gifts[i];
            }
            else
            {
                slots[i].sprite = questionMarkSprite;
            }
        }


        LayoutRebuilder.ForceRebuildLayoutImmediate(
            GetComponent<RectTransform>()
        );
    }
}