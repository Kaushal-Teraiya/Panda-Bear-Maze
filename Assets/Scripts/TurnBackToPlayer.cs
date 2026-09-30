using System.Collections;
using UnityEngine;

public class PandaTeaseRotation : MonoBehaviour
{
    Transform player;

    void Start()
    {
        player = GameObject
            .FindGameObjectWithTag("Player")
            .transform;
    }
    public float turnSpeed = 6f;

    public void TurnBackToPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTurnAway());
    }

    IEnumerator SmoothTurnAway()
    {
        Vector3 directionAway =
            transform.position - player.position;

        directionAway.y = 0;

        Quaternion targetRotation =
            Quaternion.LookRotation(directionAway.normalized);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * turnSpeed
                );

            yield return null;
        }

        transform.rotation = targetRotation;
    }
}