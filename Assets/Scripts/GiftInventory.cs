using UnityEngine;
using System.Collections.Generic;

public class GiftInventory : MonoBehaviour
{
    public static GiftInventory Instance;

    public List<Sprite> obtainedGifts =
        new List<Sprite>();

    // 👇 assign ALL possible gift sprites here in inspector
    public Sprite[] allGiftSprites;


    void Awake()
    {
        Instance = this;
    }


    public void AddGift(Sprite gift)
    {
        if (HasGift(gift))
            return;

        obtainedGifts.Add(gift);

        GameSaveManager.Instance.SaveGame();

        Debug.Log("Gift obtained: " + gift.name);
    }


    public bool HasGift(Sprite gift)
    {
        foreach (Sprite s in obtainedGifts)
        {
            if (s.name == gift.name)
                return true;
        }

        return false;
    }


    public List<string> GetSavedGiftNames()
    {
        List<string> names =
            new List<string>();

        foreach (var gift in obtainedGifts)
            names.Add(gift.name);

        return names;
    }


    public void LoadFromNames(List<string> names)
    {
        obtainedGifts.Clear();

        foreach (string name in names)
        {
            foreach (Sprite sprite in allGiftSprites)
            {
                if (sprite.name == name)
                {
                    obtainedGifts.Add(sprite);
                    break;
                }
            }
        }
    }
}