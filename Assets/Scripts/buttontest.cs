using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Test);
    }

    void Test()
    {
        Debug.Log("Pickup button clicked");
    }
}