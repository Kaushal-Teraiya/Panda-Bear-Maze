using UnityEngine;
using UnityEngine.EventSystems;

public class PickupUIButton : MonoBehaviour, IPointerClickHandler
{
    public CollectibleContainer currentContainer;

    public RandomGiftBoxPickup giftBoxPickupCurrent;


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("BUTTON CLICK DETECTED");


        // PRIORITY 1 → gift box interaction
        if (giftBoxPickupCurrent != null)
        {
            giftBoxPickupCurrent.OnPickupPressed();
            return;
        }


        // PRIORITY 2 → normal collectibles
        if (currentContainer != null)
        {
            currentContainer.PickupFromUIButton();
        }
    }
}