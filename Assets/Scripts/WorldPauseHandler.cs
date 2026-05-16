using UnityEngine;

public class WorldPauseHandler : MonoBehaviour
{
    [Header("Menu Reference")]
    public GameObject pauseMenuContainer; // Drag 'PauseMenuContainer' here in Inspector

    void OnMouseDown()
    {
        PauseGame();
    }

    public void PauseGame()
    {
        if (pauseMenuContainer != null)
            pauseMenuContainer.SetActive(true);

        Time.timeScale = 0f; // Freeze game
        Debug.Log("Game Paused via World Sprite");
    }

    // THIS IS THE MISSING FUNCTION CAUSING YOUR ERROR
    public void Resume()
    {
        if (pauseMenuContainer != null)
            pauseMenuContainer.SetActive(false);

        Time.timeScale = 1f; // Unfreeze game
        Debug.Log("Game Resumed");
    }
}