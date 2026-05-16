using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject splash;
    public GameObject cinematic;
    public GameObject startScreen;
    public GameObject howToPlay;
    public GameObject mapSelector;

    public GameObject levelSelectorM1;
    public GameObject levelSelectorM2;
    public GameObject levelSelectorM3;

    void Start()
{
    if (PlayerPrefs.GetInt("OpenMapSelector", 0) == 1)
    {
        PlayerPrefs.SetInt("OpenMapSelector", 0); // reset

        ShowMapSelector(); // directly go to map panel
    }
    else
    {
        ShowSplash(); // normal flow
    }
}

    void DisableAllPanels()
    {
        if (splash) splash.SetActive(false);
        if (cinematic) cinematic.SetActive(false);
        if (startScreen) startScreen.SetActive(false);
        if (howToPlay) howToPlay.SetActive(false);
        if (mapSelector) mapSelector.SetActive(false);

        if (levelSelectorM1) levelSelectorM1.SetActive(false);
        if (levelSelectorM2) levelSelectorM2.SetActive(false);
        if (levelSelectorM3) levelSelectorM3.SetActive(false);
    }

    public void ShowSplash()
    {
        DisableAllPanels();
        splash.SetActive(true);
    }

    public void ShowCinematic()
    {
        DisableAllPanels();
        cinematic.SetActive(true);
    }

    public void ShowStartScreen()
{
    DisableAllPanels();
    startScreen.SetActive(true);

    FindFirstObjectByType<AudioManager>().PlayMenuMusic();
}

    public void ShowHowToPlay()
    {
        DisableAllPanels();
        howToPlay.SetActive(true);
    }

    public void ShowMapSelector()
    {
        DisableAllPanels();
        mapSelector.SetActive(true);
    }

    public void ShowMap1Levels()
    {
        DisableAllPanels();
        levelSelectorM1.SetActive(true);
    }

    public void ShowMap2Levels()
    {
        DisableAllPanels();
        levelSelectorM2.SetActive(true);
    }

    public void ShowMap3Levels()
    {
        DisableAllPanels();
        levelSelectorM3.SetActive(true);
    }

    public void BackToStart()
    {
        DisableAllPanels();
        startScreen.SetActive(true);
    }

    public void BackToMaps()
    {
        DisableAllPanels();
        mapSelector.SetActive(true);
    }
}