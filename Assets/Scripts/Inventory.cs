using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("Keys")]
    public int keyCount = 0;

    [Header("Bubble Traps")]
    public int bubbleCount = 0;


    void Awake()
    {
        Instance = this;
    }


    // KEY SYSTEM
    public void AddKey()
    {
        keyCount++;

        Debug.Log("Key collected. Total keys: " + keyCount);
    }


    public bool UseKey()
    {
        if (keyCount <= 0)
        {
            FeedbackTextUI.Instance.ShowMessage(
                "Need a key to open door"
            );

            return false;
        }

        keyCount--;

        Debug.Log("Key used. Remaining keys: " + keyCount);

        return true;
    }


    // BUBBLE SYSTEM
    public void AddBubble()
    {
        bubbleCount++;

        Debug.Log("Bubble collected. Total bubbles: " + bubbleCount);
    }


    public bool UseBubble()
    {
        if (bubbleCount <= 0)
        {
            FeedbackTextUI.Instance.ShowMessage(
                "No heart bubbles left!"
            );

            return false;
        }

        bubbleCount--;

        Debug.Log("Bubble used. Remaining bubbles: " + bubbleCount);

        return true;
    }

    public int bananaCount = 0;


    public bool UseBanana()
    {
        if (bananaCount <= 0)
        {
            FeedbackTextUI.Instance.ShowMessage(
                "No bananas left!"
            );

            return false;
        }

        bananaCount--;
        return true;
    }


    public void AddBanana()
    {
        bananaCount++;
    }
}