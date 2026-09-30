using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI Instance;

    public GameObject pausePanel;

    public GameObject obtainedItemsPanel;


    void Awake()
    {
        Instance = this;

        pausePanel.SetActive(false);
        obtainedItemsPanel.SetActive(false);
    }


    public void TogglePause()
    {
        bool isActive =
            pausePanel.activeSelf;

        pausePanel.SetActive(!isActive);

        Time.timeScale =
            pausePanel.activeSelf
            ? 0f
            : 1f;
    }

    public void ShowObtainedItems()
    {
        obtainedItemsPanel.SetActive(true);

        var grid =
            obtainedItemsPanel.GetComponent<ObtainedItemsGrid>();

        if (grid != null)
            grid.RefreshGrid();
    }

    public void HideObtainedItems()
    {
        obtainedItemsPanel.SetActive(false);
    }


    public void ResumeGame()
    {
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }


    public void QuitGame()
    {
        if (GameSaveManager.Instance != null)
        {
            GameSaveManager.Instance.SaveGame();
            Debug.Log("Game saved before quitting.");
        }

        Time.timeScale = 1f;

        Application.Quit();
    }
}