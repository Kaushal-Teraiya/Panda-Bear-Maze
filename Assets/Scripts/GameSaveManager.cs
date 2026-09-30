using UnityEngine;
using System.Collections;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance;

    public float autoSaveInterval = 60f;


    void Awake()
    {
        Instance = this;

        StartCoroutine(LoadAfterDelay());

        StartCoroutine(AutoSaveRoutine());
    }


    IEnumerator LoadAfterDelay()
    {
        // wait 1 frame so Inventory & GiftInventory exist
        yield return null;

        LoadGame();
    }


    IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveInterval);

            SaveGame();
        }
    }


    void OnApplicationQuit()
    {
        SaveGame();
    }


    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            SaveGame();
    }


    public void SaveGame()
    {
        Debug.Log(
    "Saving gifts: " +
    string.Join(",",
    GiftInventory.Instance.GetSavedGiftNames())
);
        if (Inventory.Instance == null)
        {
            Debug.LogError("Inventory missing!");
            return;
        }

        Debug.Log("Saving game...");

        PlayerPrefs.SetInt("KeyCount",
            Inventory.Instance.keyCount);

        PlayerPrefs.SetInt("BubbleCount",
            Inventory.Instance.bubbleCount);

        PlayerPrefs.SetInt("BananaCount",
            Inventory.Instance.bananaCount);


        // save gifts
        if (GiftInventory.Instance != null)
        {
            PlayerPrefs.SetString(
                "ObtainedGifts",
                string.Join(",",
                GiftInventory.Instance.GetSavedGiftNames())
            );
        }

        PlayerPrefs.Save();

        Debug.Log("Game saved SUCCESSFULLY");
    }


    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("KeyCount"))
        {
            Debug.Log("No save data yet.");
            return;
        }

        Debug.Log("Loading save data...");


        Inventory.Instance.keyCount =
            PlayerPrefs.GetInt("KeyCount");

        Inventory.Instance.bubbleCount =
            PlayerPrefs.GetInt("BubbleCount");

        Inventory.Instance.bananaCount =
            PlayerPrefs.GetInt("BananaCount");


        string savedGifts =
            PlayerPrefs.GetString("ObtainedGifts");

        if (!string.IsNullOrEmpty(savedGifts)
            && GiftInventory.Instance != null)
        {
            GiftInventory.Instance.LoadFromNames(
                new System.Collections.Generic.List<string>(
                    savedGifts.Split(',')
                )
            );
        }
        if (PauseMenuUI.Instance != null)
        {
            var grid =
                PauseMenuUI.Instance
                .obtainedItemsPanel
                .GetComponent<ObtainedItemsGrid>();

            if (grid != null)
                grid.RefreshGrid();
        }
        Debug.Log("Save loaded SUCCESSFULLY");
    }
}