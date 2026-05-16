using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver = false;
    public bool isGameStarted = false;

    [Header("Level Progress")]
    public int currentLevel = 1; // Set 1, 2, or 3 in the Inspector per scene

    [Header("Audio Settings")]
    public AudioSource battleMusic;
    public AudioClip victoryVoiceClip;
    public AudioClip loseVoiceClip;
    public AudioClip ninjaHitSound;

    [Header("Mission Settings")]
    public int enemiesRemaining = 20;
    public int lives = 3;
    public int score = 0;
    private int totalEnemiesDefeated = 0;
    private int currentCombo = 0;
    private int bestCombo = 0;

    [Header("Mission Panel & Sprites")]
    public GameObject missionPanel;
    public SpriteRenderer missionSpriteRenderer;
    public Sprite missionLvlSprite;

    [Header("UI & Menus")]
    public TextMeshPro counterText;
    public TextMeshPro scoreText;
    public GameObject victoryMenuContainer;
    public GameObject gameOverPanel;

    [Header("Victory Stats Slots")]
    public TextMeshPro finalScoreText;
    public TextMeshPro bestComboText;
    public TextMeshPro victoryEnemiesText;

    [Header("Game Over Stats Slots")]
    public TextMeshPro loseScoreText;
    public TextMeshPro loseComboText;
    public TextMeshPro loseEnemiesText;

    [Header("Ninja Poses")]
    public GameObject gameplayNinja;
    public GameObject winPoseNinja;
    public GameObject defeatedNinjaPose;

    [Header("References")]
    public GameObject spawnerManager;
    public GameObject[] heartObjects;
    public ComboManager juicyComboScript;
    public ComboCelebration3D comboCeleb3D;

    void Start()
    {
        isGameOver = false;
        isGameStarted = false;
        Time.timeScale = 1f;

        if (missionSpriteRenderer != null && missionLvlSprite != null)
            missionSpriteRenderer.sprite = missionLvlSprite;

        // Reset Poses and Panels
        if (winPoseNinja != null) winPoseNinja.SetActive(false);
        if (defeatedNinjaPose != null) defeatedNinjaPose.SetActive(false);
        if (victoryMenuContainer != null) victoryMenuContainer.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (missionPanel != null) missionPanel.SetActive(true);

        ResetHearts();
        UpdateUI();
    }

    public void StartGameLogic()
    {
        if (isGameStarted) return;
        isGameStarted = true;
        if (missionPanel != null) missionPanel.SetActive(false);
        if (battleMusic != null) battleMusic.Play();

        if (spawnerManager != null)
        {
            spawnerManager.SetActive(true);
            EnemySpawner spawner = spawnerManager.GetComponent<EnemySpawner>();
            if (spawner != null) spawner.StartSpawning();
        }
    }

    public void EnemyDefeated()
    {
        if (isGameOver || !isGameStarted) return;

        enemiesRemaining--;
        totalEnemiesDefeated++;
        score += 100;
        currentCombo++;

        if (currentCombo > bestCombo) bestCombo = currentCombo;

        // --- COMBO CELEBRATION LOGIC ---
        if (currentCombo > 0 && currentCombo % 5 == 0)
        {
            if (comboCeleb3D != null)
            {
                // Call the animation (Shake happens inside ComboCelebration3D on 10, 20, 30)
                comboCeleb3D.ShowCelebration(currentCombo);

                // Add a small Hit-Stop freeze only on multiples of 10 for extra impact
                if (currentCombo % 10 == 0)
                {
                    StartCoroutine(HitFreeze(0.07f));
                }
            }
        }

        if (juicyComboScript != null)
        {
            juicyComboScript.IncreaseCombo(score);
            juicyComboScript.UpdateMission(enemiesRemaining);
        }

        UpdateUI();
        if (enemiesRemaining <= 0) StartCoroutine(DelayedMissionComplete());
    }

    // Small time-freeze to make big hits feel "Heavy"
    IEnumerator HitFreeze(float duration)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(duration);
        if (!isGameOver) Time.timeScale = 1f;
    }

    public void TakeDamage()
    {
        if (isGameOver || !isGameStarted) return;
        lives--;
        currentCombo = 0; // Reset combo on hit

        if (ninjaHitSound != null) AudioSource.PlayClipAtPoint(ninjaHitSound, Camera.main.transform.position);

        if (heartObjects != null && lives >= 0 && lives < heartObjects.Length)
            heartObjects[lives].SetActive(false);

        if (juicyComboScript != null) juicyComboScript.ResetCombo();

        UpdateUI();
        if (lives <= 0) StartCoroutine(DelayedGameOver());
    }

    IEnumerator DelayedMissionComplete()
    {
        isGameOver = true;
        if (battleMusic != null) battleMusic.Stop();
        yield return new WaitForSeconds(0.5f);

        if (gameplayNinja != null) gameplayNinja.SetActive(false);
        if (winPoseNinja != null) winPoseNinja.SetActive(true);

        UpdateVictoryStats();

        yield return new WaitForSecondsRealtime(1.5f);
        if (victoryVoiceClip != null) AudioSource.PlayClipAtPoint(victoryVoiceClip, Camera.main.transform.position);

        Time.timeScale = 0;
        if (victoryMenuContainer != null) victoryMenuContainer.SetActive(true);
    }

    IEnumerator DelayedGameOver()
    {
        isGameOver = true;
        if (battleMusic != null) battleMusic.Stop();
        yield return new WaitForSeconds(0.5f);

        if (gameplayNinja != null) gameplayNinja.SetActive(false);
        if (defeatedNinjaPose != null) defeatedNinjaPose.SetActive(true);

        UpdateGameOverStats();

        yield return new WaitForSecondsRealtime(1.5f);
        if (loseVoiceClip != null) AudioSource.PlayClipAtPoint(loseVoiceClip, Camera.main.transform.position);

        Time.timeScale = 0;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void UpdateUI()
    {
        if (counterText != null) counterText.text = "ENEMIES LEFT: " + Mathf.Max(0, enemiesRemaining);
        if (scoreText != null) scoreText.text = "SCORE: " + score;
    }

    private void UpdateVictoryStats()
    {
        if (finalScoreText != null) finalScoreText.text = "SCORE: " + score;
        if (bestComboText != null) bestComboText.text = "BEST COMBO: " + bestCombo;
        if (victoryEnemiesText != null) victoryEnemiesText.text = "DEFEATED: " + totalEnemiesDefeated;
    }

    private void UpdateGameOverStats()
    {
        if (loseScoreText != null) loseScoreText.text = "SCORE: " + score;
        if (loseComboText != null) loseComboText.text = "BEST COMBO: " + bestCombo;
        if (loseEnemiesText != null) loseEnemiesText.text = "DEFEATED: " + totalEnemiesDefeated;
    }

    private void ResetHearts()
    {
        foreach (GameObject heart in heartObjects)
            if (heart != null) heart.SetActive(true);
    }
}