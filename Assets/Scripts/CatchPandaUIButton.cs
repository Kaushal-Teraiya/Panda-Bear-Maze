using UnityEngine;

public class CatchPandaUIButton : MonoBehaviour
{
    public PandaPeekabooController panda;

    public GameObject catchButton; // assign BUTTON GAMEOBJECT

    void Update()
    {
        if (panda == null)
            return;

        bool canCatch =
            panda.playerInCatchRange &&
            !panda.isCapturedInBubble &&
            !panda.movementLocked && !panda.pandaEventRunning;

        if (catchButton.activeSelf != canCatch)
            catchButton.SetActive(canCatch);
    }

    public void OnCatchButtonPressed()
    {
        if (panda == null)
            return;

        if (!panda.playerInCatchRange)
            return;

        if (panda.movementLocked)
            return;
        if (panda.pandaEventRunning)
            return;


        catchButton.SetActive(false);

        panda.StartCoroutine(
            panda.CatchSequence()
        );
    }
}