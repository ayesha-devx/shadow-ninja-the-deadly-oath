using UnityEngine;

public class HitZone : MonoBehaviour
{
    public LevelManager levelManager;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("EnemyRight") || other.CompareTag("EnemyLeft") || other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy detected in HitZone");

            // Stop left enemy movement
            Enemylef enemyLeft = other.GetComponent<Enemylef>();
            if (enemyLeft != null)
            {
                Debug.Log("Stopping left enemy movement");
                enemyLeft.StopEnemy();
                enemyLeft.enabled = false;
            }

            // Stop right enemy movement
            EnemyRightToLeft enemyRight = other.GetComponent<EnemyRightToLeft>();
            if (enemyRight != null)
            {
                Debug.Log("Stopping right enemy movement");
                enemyRight.StopEnemy();
                enemyRight.enabled = false;
            }

            // Stop physics movement
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 2f;

                // Freeze horizontal sliding
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }

            // Update score
            if (levelManager != null)
            {
                levelManager.EnemyDefeated();
            }

            // Play death animation
            Animator anim = other.GetComponent<Animator>();
            if (anim != null)
            {
                Debug.Log("Triggering death animation");
                anim.SetTrigger("isDead");
            }

            // Disable collider so it doesn't trigger again
            Collider2D col = other.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }

            // Destroy enemy after animation
            Destroy(other.gameObject, 1.5f);
        }
    }
}