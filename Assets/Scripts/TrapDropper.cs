using UnityEngine;

public class TrapDropper : MonoBehaviour
{
    [Header("Trap Prefabs")]
    public GameObject bubbleTrapPrefab;
    public GameObject bananaTrapPrefab;

    public static TrapDropper Instance;

    void Awake()
    {
        Instance = this;
    }
    [Header("Drop Settings")]
    public float dropDistance = 2f;
    public float rayHeight = 2f;
    public float rayDistance = 5f;


    [Header("Cooldowns")]
    public float bubbleCooldown = 5f;
    public float bananaCooldown = 2f;


    float lastBubbleSpawnTime = -999f;
    float lastBananaSpawnTime = -999f;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DropBubbleTrap();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            DropBananaTrap();
        }

       
            Debug.DrawRay(Vector3.zero, Vector3.up * 10f, Color.red);
        
    }


    void DropBubbleTrap()
    {
        if (Time.time < lastBubbleSpawnTime + bubbleCooldown)
        {
            float remaining =
                bubbleCooldown -
                (Time.time - lastBubbleSpawnTime);

            FeedbackTextUI.Instance.ShowMessage(
                "Bubble ready in " +
                Mathf.Ceil(remaining) + "s"
            );

            return;
        }


        if (!Inventory.Instance.UseBubble())
            return;


        if (!SpawnTrap(bubbleTrapPrefab))
        {
            Inventory.Instance.AddBubble();
            return;
        }


        lastBubbleSpawnTime = Time.time;
    }

    bool SpawnTrap(GameObject trapPrefab)
    {
        return SpawnTrap(
            trapPrefab,
            Quaternion.LookRotation(transform.forward),
            7f
        );
    }
    void DropBananaTrap()
    {
        if (Time.time < lastBananaSpawnTime + bananaCooldown)
        {
            float remaining =
                bananaCooldown -
                (Time.time - lastBananaSpawnTime);

            FeedbackTextUI.Instance.ShowMessage(
                "Banana ready in " +
                Mathf.Ceil(remaining) + "s"
            );

            return;
        }

        if (!Inventory.Instance.UseBanana())
            return;


        // 👇 banana-specific rotation override
        Quaternion bananaRotation =
            Quaternion.Euler(-90f, 45f, 0f);

        float bananaSpawnY = 2.36f;

        if (!SpawnTrap(
                bananaTrapPrefab,
                bananaRotation,
                bananaSpawnY
            ))
        {
            Inventory.Instance.AddBanana();
            return;
        }

        lastBananaSpawnTime = Time.time;
    }
    bool SpawnTrap(
      GameObject trapPrefab,
      Quaternion rotation,
      float spawnY
  )
    {
        if (trapPrefab == null)
        {
            Debug.LogError("Trap prefab NOT assigned!");
            return false;
        }


        Vector3 forwardPos =
            transform.position +
            transform.forward * dropDistance;


        Ray ray =
            new Ray(
                forwardPos + Vector3.up * rayHeight,
                Vector3.down
            );

        Debug.Log("Drawing ray");
        Debug.DrawRay(
            forwardPos + Vector3.up * rayHeight,
            Vector3.down * rayDistance,
            Color.purple,
            50f
        );


        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Instantiate(
                trapPrefab,
                new Vector3(
                    hit.point.x,
                    spawnY,
                    hit.point.z
                ),
                rotation
            );

            return true;
        }


        Debug.LogWarning("Raycast failed → ground not detected");
        return false;
    }
    public void DropBubbleFromUIButton()
    {
        TrapDropper.Instance.DropBubbleTrap();
    }


    public void DropBnanaFromUIButton()
    {
        TrapDropper.Instance.DropBananaTrap();
    }

}