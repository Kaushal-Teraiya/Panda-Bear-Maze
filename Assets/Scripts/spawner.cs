using UnityEngine;
using UnityEngine.UI;

public class SpawnCollectibleInFront : MonoBehaviour
{
    public GameObject collectiblePrefab;

    public Transform player;

    public float spawnDistance = 2f;

    public float spawnHeightOffset = 0.3f;
    public Button spawnButton;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnCollectible();
        }
    }

    public void SpawnCollectible()
    {
        Vector3 spawnPosition =
            player.position +
            player.forward * spawnDistance;

        spawnPosition.y += spawnHeightOffset;

        Instantiate(
            collectiblePrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}