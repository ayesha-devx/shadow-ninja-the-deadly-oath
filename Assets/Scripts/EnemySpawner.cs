using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyLeftPrefab;
    public GameObject enemyRightPrefab;

    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    public float spawnInterval = 2f;
    public int totalEnemies = 20;

    // --- NEW: This will be set by your LevelManager or manually in the Inspector ---
    public float currentMoveSpeed = 7f;

    private LevelManager levelManager;
    private int lastSide = -1;
    private int sameSideCount = 0;

    void Awake()
    {
        levelManager = Object.FindFirstObjectByType<LevelManager>();
    }

    public void StartSpawning()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        while (!LevelManager.isGameOver)
        {
            if (levelManager != null && levelManager.enemiesRemaining > 0)
            {
                SpawnEnemy();
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        int side = Random.Range(0, 2);

        if (side == lastSide)
        {
            sameSideCount++;
            if (sameSideCount >= 2)
            {
                side = 1 - side;
                sameSideCount = 0;
            }
        }
        else
        {
            sameSideCount = 0;
        }

        lastSide = side;

        GameObject newEnemy;
        if (side == 0)
        {
            newEnemy = Instantiate(enemyLeftPrefab, leftSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            newEnemy = Instantiate(enemyRightPrefab, rightSpawnPoint.position, Quaternion.identity);
        }

        // --- NEW: INJECT SPEED INTO THE NEW ENEMY ---
        ApplySpeedToEnemy(newEnemy);
    }

    private void ApplySpeedToEnemy(GameObject enemy)
    {
        // Check for the script that moves right-to-left
        EnemyRightToLeft scriptR = enemy.GetComponent<EnemyRightToLeft>();
        if (scriptR != null)
        {
            scriptR.moveSpeed = currentMoveSpeed;
        }

        // Check for the script that moves left-to-right
        Enemylef scriptL = enemy.GetComponent<Enemylef>();
        if (scriptL != null)
        {
            scriptL.moveSpeed = currentMoveSpeed;
        }
    }
}