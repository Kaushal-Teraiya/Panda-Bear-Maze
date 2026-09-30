using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class VideoVisibilityController : MonoBehaviour
{
    Camera playerCamera;

    VideoPlayer videoPlayer;

    Renderer quadRenderer;

    bool isVisible = false;


    void Start()
    {
        playerCamera = Camera.main;

        videoPlayer = GetComponent<VideoPlayer>();

        quadRenderer = GetComponent<Renderer>();

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
        }

        // Start optimized visibility checking loop
        StartCoroutine(CheckVisibilityRoutine());
    }


    IEnumerator CheckVisibilityRoutine()
    {
        while (true)
        {
            CheckVisibility();

            // check only 4 times per second instead of every frame
            yield return new WaitForSeconds(0.5f);
        }
    }


    void CheckVisibility()
    {
        if (playerCamera == null)
            return;

        Plane[] planes =
            GeometryUtility.CalculateFrustumPlanes(playerCamera);

        bool nowVisible =
            GeometryUtility.TestPlanesAABB(
                planes,
                quadRenderer.bounds
            );


        if (nowVisible && !isVisible)
        {
            ActivateVideo();
        }
        else if (!nowVisible && isVisible)
        {
            DeactivateVideo();
        }

        isVisible = nowVisible;
    }


    void ActivateVideo()
    {
        quadRenderer.enabled = true;

        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }


    void DeactivateVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        quadRenderer.enabled = false;
    }
}