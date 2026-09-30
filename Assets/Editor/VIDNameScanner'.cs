using UnityEngine;
using UnityEditor;
using System.IO;

public class StreamingAssetsVideoScanner
{
    [MenuItem("Tools/Scan StreamingAssets Videos")]
    static void Scan()
    {
        string root =
            Path.Combine(
                Application.dataPath,
                "StreamingAssets"
            );


        if (!Directory.Exists(root))
        {
            Debug.LogError(
                "StreamingAssets folder missing."
            );
            return;
        }


        string[] files =
            Directory.GetFiles(
                root,
                "*.mp4",
                SearchOption.AllDirectories
            );


        AutoAssignStreamingVideosRuntime target =
            Object.FindFirstObjectByType
            <AutoAssignStreamingVideosRuntime>();


        if (target == null)
        {
            Debug.LogError(
                "AutoAssignStreamingVideosRuntime not found in scene."
            );
            return;
        }


        target.videoFileNames.Clear();


        foreach (string file in files)
        {
            string relative =
                file.Substring(root.Length + 1)
                .Replace("\\", "/");


            target.videoFileNames.Add(relative);
        }


        EditorUtility.SetDirty(target);


        Debug.Log(
            "Found " +
            target.videoFileNames.Count +
            " videos automatically."
        );
    }
}