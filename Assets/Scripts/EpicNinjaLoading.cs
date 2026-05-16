using UnityEngine;
using TMPro;
using System.Collections;

public class EpicNinjaLoading : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource bgmSource;           // Drag your main music AudioSource here
    public AudioSource completeSoundSource; // NEW: Drag your "Loading Complete" AudioSource here
    public float songStartTime = 0f;

    [Header("Groups")]
    public GameObject loadingGroup;
    public GameObject missionPanel;

    [Header("References")]
    public Transform shuriken;
    public Transform blueBar;
    public Transform redBar;
    public GameObject spark;

    [Header("Animation References")]
    public TMP_Text loadingText;
    public TMP_Text preparingText;

    [Header("Ultra Slow Settings")]
    public SpriteRenderer missionSprite;
    [Range(1f, 10f)] public float slideInDuration = 4.0f;
    [Range(1f, 10f)] public float slideOutDuration = 4.0f;
    public float startYPosition = 12f;
    public float exitYPosition = -15f;

    [Header("Loading Settings")]
    public float totalLoadTime = 5.0f;
    public float spinSpeed = -400f;

    private float elapsed = 0f;
    private bool isLoadingDone = false;
    private bool isWaitingForTap = false;
    private Vector3 centerPosition = Vector3.zero;

    void Start()
    {
        Time.timeScale = 0f;
        loadingGroup.SetActive(true);
        missionPanel.SetActive(false);

        // START MAIN BACKGROUND MUSIC
        if (bgmSource != null && bgmSource.clip != null)
        {
            bgmSource.time = songStartTime;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        if (blueBar) blueBar.localScale = new Vector3(0, 3, 1);
        if (redBar) redBar.localScale = new Vector3(0, 3, 1);

        if (missionPanel != null)
            missionPanel.transform.position = new Vector3(0, startYPosition, 0);

        if (loadingText != null) StartCoroutine(AnimateDots(loadingText));
        if (preparingText != null) StartCoroutine(AnimateDots(preparingText));
    }

    void Update()
    {
        if (!isLoadingDone)
        {
            if (shuriken) shuriken.Rotate(0, 0, spinSpeed * Time.unscaledDeltaTime);
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / totalLoadTime);

            if (blueBar) blueBar.localScale = new Vector3(progress, 3, 1);
            if (redBar) redBar.localScale = new Vector3(progress, 3, 1);

            // --- LOADING COMPLETE TRIGGER ---
            if (progress >= 1f)
            {
                isLoadingDone = true;

                // PLAY THE "COMPLETE" SOUND EFFECT
                if (completeSoundSource != null)
                {
                    completeSoundSource.Play();
                }

                if (spark) spark.SetActive(true);
                StartCoroutine(WaitAndSlideMission());
            }
        }
        else if (isWaitingForTap && Input.GetMouseButtonDown(0))
        {
            StartGameplay();
        }
    }

    IEnumerator AnimateDots(TMP_Text targetText)
    {
        string originalText = targetText.text.Replace(".", "").Trim();
        while (true)
        {
            targetText.text = originalText + ".";
            yield return new WaitForSecondsRealtime(0.5f);
            targetText.text = originalText + "..";
            yield return new WaitForSecondsRealtime(0.5f);
            targetText.text = originalText + "...";
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    IEnumerator WaitAndSlideMission()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        loadingGroup.SetActive(false);
        if (missionPanel != null) missionPanel.SetActive(true);

        float timer = 0;
        Vector3 startPos = new Vector3(0, startYPosition, 0);

        while (timer < slideInDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / slideInDuration;
            t = Mathf.SmoothStep(0f, 1f, t);
            missionPanel.transform.position = Vector3.Lerp(startPos, centerPosition, t);
            yield return null;
        }

        missionPanel.transform.position = centerPosition;
        isWaitingForTap = true;
    }

    void StartGameplay()
    {
        isWaitingForTap = false;
        StopAllCoroutines();
        StartCoroutine(SlideOutAndStart());
    }

    IEnumerator SlideOutAndStart()
    {
        float timer = 0;
        Vector3 endPos = new Vector3(0, exitYPosition, 0);

        while (timer < slideOutDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / slideOutDuration;
            t = Mathf.SmoothStep(0f, 1f, t);
            missionPanel.transform.position = Vector3.Lerp(centerPosition, endPos, t);
            yield return null;
        }

        // STOP THE MAIN BGM ONCE PANEL IS GONE
        if (bgmSource != null) bgmSource.Stop();

        missionPanel.SetActive(false);
        Time.timeScale = 1f;

        LevelManager lm = Object.FindFirstObjectByType<LevelManager>();
        if (lm != null) lm.StartGameLogic();

        EnemySpawner spawner = Object.FindFirstObjectByType<EnemySpawner>();
        if (spawner != null) spawner.StartSpawning();
    }
}