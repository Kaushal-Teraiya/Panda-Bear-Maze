using UnityEngine;
using UnityEngine.Video;

public class GlobalVideoManager : MonoBehaviour
{
    public static GlobalVideoManager Instance;

    VideoPlayer currentActivePlayer;


    void Awake()
    {
        Instance = this;
    }


    public void RequestPlay(VideoPlayer player)
    {
        if (currentActivePlayer == player)
            return;

        if (currentActivePlayer != null)
        {
            currentActivePlayer.Stop();
        }

        currentActivePlayer = player;

        currentActivePlayer.Play();
    }


    public void StopIfActive(VideoPlayer player)
    {
        if (currentActivePlayer == player)
        {
            player.Stop();
            currentActivePlayer = null;
        }
    }
}