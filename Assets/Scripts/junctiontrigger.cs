using UnityEngine;

public class JunctionTrigger : MonoBehaviour
{
    public Waypoint[] branchOptions;

    public Waypoint ChooseBranch()
    {
        if (branchOptions == null ||
            branchOptions.Length == 0)
            return null;

        return branchOptions[
            Random.Range(0, branchOptions.Length)
        ];
    }
}