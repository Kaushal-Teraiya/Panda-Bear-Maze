using System.Collections.Generic;
using UnityEngine;

public class RandomGiftBoxPickup : MonoBehaviour
{
    [Header("Possible Gifts")]

    public Sprite[] possibleGifts;


    [Header("Pickup Button Reference")]

    public PickupUIButton pickupUIButton;


    bool playerInsideTrigger = false;


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInsideTrigger = true; // ⭐ missing line

        pickupUIButton.gameObject.SetActive(true);

        pickupUIButton.giftBoxPickupCurrent = this;
    }


    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInsideTrigger = false; // ⭐ missing line

        pickupUIButton.gameObject.SetActive(false);

        pickupUIButton.giftBoxPickupCurrent = null;
    }
    public void OnPickupPressed()
    {
        if (!playerInsideTrigger)
            return;


        OpenRandomGift();
    }


    void OpenRandomGift()
    {
        if (GiftDisplayUI.Instance == null)
        {
            Debug.LogError("GiftDisplayUI missing!");
            return;
        }

        if (possibleGifts == null || possibleGifts.Length == 0)
        {
            Debug.LogError("No gift sprites assigned!");
            return;
        }

        // remove already obtained gifts
        List<Sprite> availableGifts =
            new List<Sprite>();

        foreach (Sprite gift in possibleGifts)
        {
            if (!GiftInventory.Instance.HasGift(gift))
                availableGifts.Add(gift);
        }

        if (availableGifts.Count == 0)
        {
            FeedbackTextUI.Instance.ShowMessage(
                "All gifts already collected!"
            );

            Destroy(gameObject);
            return;
        }

        // 🎲 40% empty chance
        if (Random.value < 0.2f)
        {
            FeedbackTextUI.Instance.ShowMessage(
                "Oh no gift yet… go hunt them!"
            );

            Destroy(gameObject);
            return;
        }

        Sprite chosenGift =
            availableGifts[
                Random.Range(0, availableGifts.Count)
            ];

        GiftInventory.Instance.AddGift(chosenGift);

        GiftDisplayUI.Instance.ShowGift(chosenGift);

        Destroy(gameObject);
    }
}