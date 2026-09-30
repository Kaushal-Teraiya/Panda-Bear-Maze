using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AssignTeaseAnimationsToJailDoorLift : EditorWindow
{
    List<AnimationClip> clips =
        new List<AnimationClip>();

    [MenuItem("Tools/Assign Tease Animations To JailDoorLift")]
    static void Open()
    {
        GetWindow<
            AssignTeaseAnimationsToJailDoorLift
        >("Tease Animation Setup");
    }

    void OnGUI()
    {
        GUILayout.Label(
            "Populate JailDoorLift.teaseAnimations on all TrapTrigger objects",
            EditorStyles.boldLabel
        );

        int count =
            Mathf.Max(
                0,
                EditorGUILayout.IntField(
                    "Animation Count",
                    clips.Count
                )
            );

        while (count > clips.Count)
            clips.Add(null);

        while (count < clips.Count)
            clips.RemoveAt(clips.Count - 1);

        for (int i = 0; i < clips.Count; i++)
        {
            clips[i] =
                (AnimationClip)
                EditorGUILayout.ObjectField(
                    "Clip " + i,
                    clips[i],
                    typeof(AnimationClip),
                    false
                );
        }

        GUILayout.Space(10);

        if (
            GUILayout.Button(
                "Assign To All TrapTrigger Objects"
            )
        )
        {
            AssignAnimations();
        }
    }

    void AssignAnimations()
    {
        GameObject[] allObjects =
            FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        int assignedCount = 0;

        foreach (var obj in allObjects)
        {
            if (obj.name != "TrapTrigger")
                continue;

            JailDoorLift script =
                obj.GetComponent<JailDoorLift>();

            if (script == null)
                continue;

            Undo.RecordObject(
                script,
                "Assign tease animations"
            );

            script.teaseAnimations =
                clips.ToArray();

            EditorUtility.SetDirty(script);

            assignedCount++;
        }

        Debug.Log(
            "Assigned teaseAnimations to "
            + assignedCount +
            " JailDoorLift components."
        );
    }
}