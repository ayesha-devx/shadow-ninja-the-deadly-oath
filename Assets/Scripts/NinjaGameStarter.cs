using UnityEngine;

public class NinjaGameStarter : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject loadingContainer; // Drag 'Bar_Container' here
    public GameObject otherLoadingItems; // Drag a parent object containing Shuriken/Text here
    public Transform maskTransform;    // Drag 'whitesqaure' here
    public Transform shuriken;         // Drag 'shirukenloadingscreen_0' here

    [Header("Settings")]
    public float loadSpeed = 0.3f;
    public float fullWidth = 10f; // The Scale X when bar is full

    private float progress = 0f;
    private bool gameStarted = false;

    void Update()
    {
        if (progress < 1f)
        {
            progress += Time.deltaTime * loadSpeed;

            // 1. Fill the bar
            if (maskTransform != null)
                maskTransform.localScale = new Vector3(fullWidth * progress, maskTransform.localScale.y, 1);

            // 2. Spin shuriken
            if (shuriken != null)
                shuriken.Rotate(0, 0, -250 * Time.deltaTime);
        }
        else if (!gameStarted)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;

        // Option A: Just turn off the loading screen
        loadingContainer.SetActive(false);
        if(otherLoadingItems != null) otherLoadingItems.SetActive(false);

        // Option B: Enable your Ninja Player script or Spawner here
        Debug.Log("Battlefield Prepared! Game Starting...");
    }
}