// using UnityEngine;
// using UnityEngine.UI;

// public class PickupButtonController : MonoBehaviour
// {
//     Button button;

//     CollectibleContainer currentTarget;

//     void Awake()
//     {
//         button = GetComponent<Button>();
//         button.onClick.AddListener(OnPressed);

//         gameObject.SetActive(false);
//     }

//     public void RegisterTarget(CollectibleContainer target)
//     {
//         currentTarget = target;
//         gameObject.SetActive(true);
//     // }

    // public void ClearTarget(CollectibleContainer target)
    // {
    //     if(currentTarget == target)
    //     {
    //         currentTarget = null;
    //         gameObject.SetActive(false);
    //     }
    // }

    // void OnPressed()
    // {
    //     if(currentTarget != null)
    //     {
    //         currentTarget.Pickup();
//     //     }
//     //     else
//     //     {
//     //         Debug.Log("No collectible nearby");
//     //     }
//     }
// }