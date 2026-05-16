using UnityEngine;

public class HowToPlayController : MonoBehaviour
{
    public GameObject[] tutorialImages;

    public GameObject nextButton;
    public GameObject cancelButton;

    public GameObject howToPlayPanel;
    public GameObject startScreenPanel;

    int currentIndex = 0;

    void OnEnable()
    {
        currentIndex = 0;
        UpdateTutorial();
    }

    public void Next()
    {
        if (currentIndex < tutorialImages.Length - 1)
        {
            currentIndex++;
            UpdateTutorial();
        }
    }

    void UpdateTutorial()
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            tutorialImages[i].SetActive(i == currentIndex);
        }

        if (currentIndex == tutorialImages.Length - 1)
        {
            nextButton.SetActive(false);
            cancelButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(true);
            cancelButton.SetActive(false);
        }
    }

    public void Cancel()
    {
        howToPlayPanel.SetActive(false);
        startScreenPanel.SetActive(true);
    }
}