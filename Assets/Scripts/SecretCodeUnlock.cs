using UnityEngine;
using TMPro;

public class SecretCodeDoorUnlock : MonoBehaviour
{
    public TMP_InputField inputField;

    public TextMeshProUGUI feedbackText;

    public string correctCode = "ROHLAX";

    private MonoBehaviour targetDoor;


    public void SetTargetDoor(MonoBehaviour door)
    {
        targetDoor = door;
    }


    public void TryUnlock()
    {
        if (inputField.text == correctCode)
        {
            feedbackText.text = "Correct code!";

            if (targetDoor != null)
                targetDoor.SendMessage("OpenDoor");

            gameObject.SetActive(false);
        }
        else
        {
            feedbackText.text = "Wrong code";
        }
    }
}