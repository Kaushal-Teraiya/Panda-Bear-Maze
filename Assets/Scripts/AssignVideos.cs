using UnityEngine;
using UnityEngine.Video;

public class StreamingVideoLoader : MonoBehaviour
{
    VideoPlayer player;

    void Awake()
    {
        player = GetComponent<VideoPlayer>();

        if (player.source != VideoSource.Url)
            return;

        if (string.IsNullOrEmpty(player.url))
            return;

        player.url =
            System.IO.Path.Combine(
                Application.streamingAssetsPath,
                player.url
            );

        player.Prepare();

        player.prepareCompleted += OnPrepared;
    }

    void OnPrepared(VideoPlayer vp)
    {
        vp.Play();
    }
}