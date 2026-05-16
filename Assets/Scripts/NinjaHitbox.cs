using UnityEngine;
using System.Collections;
using TMPro;

public class NinjaHitbox : MonoBehaviour
{
    public LevelManager levelManager;

    [Header("Hit Effect Settings")]
    public GameObject hitSparkPrefab;
    public GameObject floatingTextPrefab;
    public float hitStopDuration = 0.12f;

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isEnemy = other.CompareTag("EnemyRight") || other.CompareTag("EnemyLeft");

        if (isEnemy && other.transform.root != transform.root)
        {
            // --- IMPACT EFFECTS ---
            StopCoroutine("TriggerHitStop");
            StartCoroutine(TriggerHitStop(hitStopDuration));

            if (hitSparkPrefab != null)
            {
                Instantiate(hitSparkPrefab, transform.position, Quaternion.identity);
            }

            // --- FLOATING TEXT (Spawns on Enemy Head) ---
            if (floatingTextPrefab != null)
            {
                // Get the position of the enemy we just hit
                Vector3 enemyPos = other.transform.position;

                // Set spawn position: Enemy's X, and slightly above Enemy's Y
                Vector3 spawnPos = new Vector3(enemyPos.x, enemyPos.y + 1.5f, 0);

                GameObject popUp = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity, null);

                TextMeshPro textComp = popUp.GetComponent<TextMeshPro>();
                if (textComp != null)
                {
                    textComp.text = "+100";
                }
            }

            // --- ENEMY DEFEAT LOGIC ---
            HandleEnemyDefeat(other);
        }
    }

    IEnumerator TriggerHitStop(float duration)
    {
        float originalScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = originalScale;
    }

    void HandleEnemyDefeat(Collider2D other)
    {
        var rightEnemy = other.GetComponent<EnemyRightToLeft>();
        if (rightEnemy != null) { rightEnemy.StopEnemy(); rightEnemy.enabled = false; }

        var leftEnemy = other.GetComponent<Enemylef>();
        if (leftEnemy != null) { leftEnemy.StopEnemy(); leftEnemy.enabled = false; }

        if (levelManager != null)
        {
            levelManager.EnemyDefeated();
        }

        Animator anim = other.GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("isDead");

        other.GetComponent<Collider2D>().enabled = false;

        Destroy(other.gameObject, 1.5f);
    }
}