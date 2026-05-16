using UnityEngine;

public class EnemyRightToLeft : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fixedYPosition = -4.02f; // <--- Set your desired Y here
    private bool isDead = false;

    [Header("Audio Settings")]
    public AudioClip dieSound;

    void Update()
    {
        if (LevelManager.isGameOver)
        {
            Destroy(gameObject);
            return;
        }

        if (!isDead)
        {
            // Move left
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

            // FORCE the Y position to stay at -4.02
            transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);
        }
    }

    public void StopEnemy()
    {
        if (isDead) return;
        isDead = true;

        if (dieSound != null)
        {
            GameObject manager = GameObject.Find("GlobalSoundManager");
            if (manager != null)
            {
                AudioSource source = manager.GetComponent<AudioSource>();
                if (source != null)
                {
                    source.PlayOneShot(dieSound);
                }
            }
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Note: In older Unity versions this is rb.velocity
            // In Unity 6 / newer 2022+ it is rb.linearVelocity
            rb.linearVelocity = Vector2.zero;

            // Also freeze physics so it doesn't fall or bounce after death
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}