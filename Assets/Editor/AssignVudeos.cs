using UnityEngine;
using UnityEditor;
using UnityEngine.Video;
using System.IO;
using System.Collections.Generic;

public class AssignRandomStreamingVideos : EditorWindow
{
    Transform parentContainer;

    List<string> relativeVideoPaths =
        new List<string>();


    [MenuItem("Tools/Assign Random StreamingAssets Videos")]
    public static void ShowWindow()
    {
        GetWindow<AssignRandomStreamingVideos>(
            "Assign Streaming Videos"
        );
    }


    void OnGUI()
    {
        GUILayout.Label(
            "Assign Random Videos From StreamingAssets",
            EditorStyles.boldLabel
        );


        parentContainer =
        (Transform)EditorGUILayout.ObjectField(
            "Parent Container",
            parentContainer,
            typeof(Transform),
            true
        );


        GUILayout.Space(10);


        if (GUILayout.Button(
            "Scan StreamingAssets Automatically"
        ))
        {
            ScanVideos();
        }


        if (GUILayout.Button(
            "Assign Random Videos"
        ))
        {
            AssignVideos();
        }
    }


    void ScanVideos()
    {
        relativeVideoPaths.Clear();


        string root =
            Path.Combine(
                Application.dataPath,
                "StreamingAssets"
            );


        if (!Directory.Exists(root))
        {
            Debug.LogError(
                "StreamingAssets folder not found."
            );
            return;
        }


        string[] files =
            Directory.GetFiles(
                root,
                "*.mp4",
                SearchOption.AllDirectories
            );


        foreach (string file in files)
        {
            string relativePath =
                file.Substring(root.Length + 1)
                    .Replace("\\", "/");

            relativeVideoPaths.Add(relativePath);
        }


        Debug.Log(
            "Found " +
            relativeVideoPaths.Count +
            " videos."
        );
    }


    void AssignVideos()
    {
        if (parentContainer == null)
        {
            Debug.LogError(
                "Assign parent container first."
            );
            return;
        }


        if (relativeVideoPaths.Count == 0)
        {
            Debug.LogError(
                "Scan StreamingAssets first."
            );
            return;
        }


        VideoPlayer[] players =
            parentContainer
            .GetComponentsInChildren<VideoPlayer>(true);


        int assignedCount = 0;


        foreach (VideoPlayer player in players)
        {
            string relativePath =
                relativeVideoPaths[
                    Random.Range(
                        0,
                        relativeVideoPaths.Count
                    )
                ];


            Undo.RecordObject(
                player,
                "Assign StreamingAssets Video"
            );


            player.source = VideoSource.Url;

            // IMPORTANT: only relative path
            player.url = relativePath;

            player.playOnAwake = true;
            player.isLooping = true;
            player.waitForFirstFrame = true;


            EditorUtility.SetDirty(player);

            assignedCount++;
        }


        Debug.Log(
            "Assigned videos to " +
            assignedCount +
            " VideoPlayers."
        );
    }
}