using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

public class AutoAssignStreamingVideosRuntime : MonoBehaviour
{
    [HideInInspector]
    public List<string> videoFileNames =
        new List<string>();


    System.Collections.IEnumerator Start()
    {
        yield return null;

        AssignVideos();
    }


    void AssignVideos()
    {
        if (videoFileNames.Count == 0)
        {
            Debug.LogError("Video list empty.");
            return;
        }


        VideoPlayer[] players =
            GetComponentsInChildren<VideoPlayer>(true);


        foreach (VideoPlayer player in players)
        {
            string fileName =
                videoFileNames[
                    Random.Range(
                        0,
                        videoFileNames.Count
                    )
                ];


            player.Stop();

            player.source = VideoSource.Url;

            player.url =
                Application.streamingAssetsPath
                + "/" + fileName;

            player.isLooping = true;

            player.waitForFirstFrame = true;

            player.Prepare();
        }


        Debug.Log("Videos assigned automatically.");
    }
}