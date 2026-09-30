using UnityEngine;
using UnityEditor;

public class MazeCornerGenerator : EditorWindow
{
    Texture2D mazeTexture;
    Transform parentContainer;

    int gridSize = 20;
    float groundSize = 100f;

    [MenuItem("Tools/Maze Corner Generator")]
    static void Init()
    {
        GetWindow<MazeCornerGenerator>();
    }

    void OnGUI()
    {
        GUILayout.Label("Maze Corner Detector", EditorStyles.boldLabel);

        mazeTexture =
        (Texture2D)EditorGUILayout.ObjectField(
            "Maze Texture",
            mazeTexture,
            typeof(Texture2D),
            false
        );

        parentContainer =
        (Transform)EditorGUILayout.ObjectField(
            "Parent Container",
            parentContainer,
            typeof(Transform),
            true
        );

        gridSize =
        EditorGUILayout.IntField("Grid Size", gridSize);

        groundSize =
        EditorGUILayout.FloatField("Ground Size", groundSize);

        if (GUILayout.Button("Generate Corner Spawn Points"))
        {
            GenerateCorners();
        }
    }

    void GenerateCorners()
    {
        if (mazeTexture == null)
        {
            Debug.LogError("Assign maze texture first");
            return;
        }

        float cellSize = groundSize / gridSize;

        int cornerCount = 0;

        for (int x = 1; x < gridSize - 1; x++)
        {
            for (int y = 1; y < gridSize - 1; y++)
            {
                if (!IsWalkable(x, y))
                    continue;

                bool up = IsWalkable(x, y + 1);
                bool down = IsWalkable(x, y - 1);
                bool left = IsWalkable(x - 1, y);
                bool right = IsWalkable(x + 1, y);

                int neighbors = 0;

                if (up) neighbors++;
                if (down) neighbors++;
                if (left) neighbors++;
                if (right) neighbors++;

                bool isCorner =
                    neighbors == 2 &&
                    !((up && down) || (left && right));

                bool isJunction =
                    neighbors >= 3;

                if (isCorner || isJunction)
                {
                    CreateCornerPoint(x, y, cellSize);
                    cornerCount++;
                }
            }
        }

        Debug.Log("Created " + cornerCount + " spawn points");
    }


    bool IsWalkable(int x, int y)
    {
        int texX =
            Mathf.RoundToInt(
                (x + 0.5f) *
                mazeTexture.width /
                gridSize
            );

        int texY =
            Mathf.RoundToInt(
                (y + 0.5f) *
                mazeTexture.height /
                gridSize
            );

        Color pixel =
            mazeTexture.GetPixel(texX, texY);

        return pixel.grayscale > 0.8f;
    }


    void CreateCornerPoint(int x, int y, float cellSize)
    {
        GameObject go =
        new GameObject("CornerSpawnPoint");

        if (parentContainer != null)
            go.transform.parent = parentContainer;

        float offset = groundSize * 0.5f;

        Vector3 pos = new Vector3(
            (x * cellSize) - offset + cellSize * 0.5f,
            0f,
            (y * cellSize) - offset + cellSize * 0.5f
        );

        go.transform.position = pos;
    }
}