using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMenuButton : MonoBehaviour
{
    public enum ButtonType { Resume, Restart, Retry, MainMenu, Quit, NextLevel }
    public ButtonType type;

    void OnMouseDown()
    {
        OnButtonClick();
    }

    public void OnButtonClick()
    {
        LevelManager lvl = Object.FindFirstObjectByType<LevelManager>();
        WorldPauseHandler pause = Object.FindFirstObjectByType<WorldPauseHandler>();

        switch (type)
        {
            case ButtonType.Resume:
                if (pause != null)
                    pause.Resume();
                break;

            case ButtonType.NextLevel:
                Time.timeScale = 1f;

                if (lvl != null)
                {
                    if (lvl.currentLevel == 1)
                        SceneManager.LoadScene("Level2");
                    else if (lvl.currentLevel == 2)
                        SceneManager.LoadScene("Level3");
                    else
                        SceneManager.LoadScene("EntryScene");
                }
                break;

            case ButtonType.Restart:
            case ButtonType.Retry:
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

            case ButtonType.MainMenu:
                Time.timeScale = 1f;

                if (pause != null)
                    pause.Resume();
                    
                PlayerPrefs.SetInt("OpenMapSelector", 1);
                SceneManager.LoadScene("EntryScene");
                break;

            case ButtonType.Quit:
                Time.timeScale = 1f;
                Application.Quit();
                break;
        }
    }
}