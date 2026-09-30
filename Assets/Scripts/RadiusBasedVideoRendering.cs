using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RadiusVideoActivationManager : MonoBehaviour
{
    public Transform player;

    public float activationRadius = 25f;

    public int maxActiveVideos = 5;

    public float refreshInterval = 3f;

    List<GameObject> allQuads =
        new List<GameObject>();


    void Start()
    {
        if (player == null)
            player = Camera.main.transform;


        foreach (Transform child in transform)
        {
            allQuads.Add(child.gameObject);

            child.gameObject.SetActive(false);
        }


        StartCoroutine(RefreshRoutine());
    }


    IEnumerator RefreshRoutine()
    {
        while (true)
        {
            RefreshActiveVideos();

            yield return new WaitForSeconds(refreshInterval);
        }
    }


    void RefreshActiveVideos()
    {
        if (player == null)
            return;


        var nearbyQuads =
            allQuads
            .Where(q =>
                Vector3.Distance(
                    player.position,
                    q.transform.position
                ) <= activationRadius
            )
            .OrderBy(q =>
                Vector3.Distance(
                    player.position,
                    q.transform.position
                )
            )
            .Take(maxActiveVideos)
            .ToList();


        foreach (GameObject quad in allQuads)
        {
            VideoPlayer vp =
                quad.GetComponent<VideoPlayer>();


            if (nearbyQuads.Contains(quad))
            {
                if (!quad.activeSelf)
                {
                    quad.SetActive(true);

                    if (vp != null)
                    {
                        vp.Prepare();

                        vp.prepareCompleted +=
                            (VideoPlayer player) =>
                            {
                                player.Play();
                            };
                    }
                }
            }
            else
            {
                if (quad.activeSelf)
                {
                    if (vp != null)
                        vp.Stop();

                    quad.SetActive(false);
                }
            }
        }
    }
}