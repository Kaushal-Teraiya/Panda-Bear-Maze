using UnityEngine;
using UnityEngine.UI;

public class StoryPanelController : MonoBehaviour
{
    public GameObject panel;

    public MonoBehaviour playerMovementScript;

    void Start()
    {
        if(panel != null)
            panel.SetActive(true);

        if(playerMovementScript != null)
            playerMovementScript.enabled = false;
    }

    public void ContinueGame()
    {
        panel.SetActive(false);

        if(playerMovementScript != null)
            playerMovementScript.enabled = true;
    }
}