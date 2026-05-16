using UnityEngine;
using System.Collections;

public class DamageZone : MonoBehaviour
{
    public LevelManager levelManager;
    private bool isProcessingHit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Stop if already hit or if the game is over
        if (isProcessingHit || (levelManager != null && levelManager.lives <= 0)) return;

        if (other.CompareTag("EnemyRight") || other.CompareTag("EnemyLeft"))
        {
            isProcessingHit = true;

            // 1. Stop Enemy movement
            MonoBehaviour move = (MonoBehaviour)other.GetComponent("EnemyRightToLeft") ?? (MonoBehaviour)other.GetComponent("Enemylef");
            if (move != null) move.enabled = false;

            // 2. Play Enemy Attack
            Animator enemyAnim = other.GetComponent<Animator>();
            if (enemyAnim != null)
            {
                enemyAnim.SetInteger("AT", Random.Range(1, 4));
                enemyAnim.SetTrigger("IA");
            }

            // 3. Ninja Reaction & Damage
            if (levelManager != null) levelManager.TakeDamage();

            Animator ninjaAnim = GetComponentInParent<Animator>();
            if (ninjaAnim != null)
            {
                int side = other.CompareTag("EnemyRight") ? 1 : 2;

                // Set the side first, then pull the trigger for a clean reaction
                ninjaAnim.SetInteger("TiltSide", side);
                ninjaAnim.SetTrigger("StartTilt");

                StartCoroutine(HitCooldown());
            }

            // 4. Cleanup enemy
            other.GetComponent<Collider2D>().enabled = false;
            Destroy(other.gameObject, 1.2f);
        }
    }

    IEnumerator HitCooldown()
    {
        // Prevents double-tilt and rapid life loss
        yield return new WaitForSeconds(0.6f);
        isProcessingHit = false;
    }
}