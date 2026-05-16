using UnityEngine;

public class Enemylef : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fixedYPosition = -4.02f; // <--- This locks the height
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
            // Move right
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

            // FORCE the Y position to stay exactly at -4.02
            transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z);
        }
    }

    public void StopEnemy()
    {
        if (isDead) return;
        isDead = true;

        // --- GLOBAL SOUND MANAGER LOGIC ---
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
            // Set velocity to zero so it stops moving physically
            rb.linearVelocity = Vector2.zero;

            // Optional: Change to Kinematic so gravity/physics don't move it after death
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}