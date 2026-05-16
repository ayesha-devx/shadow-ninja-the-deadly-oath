using UnityEngine;

public class EnemyRunLeft : MonoBehaviour
{
    public float moveSpeed = 3f;
   

    void Update()
    {
        // Moves the enemy forward in the current direction
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        // If you want them to pace back and forth eventually,
        // we can add a 'Flip' function here later!
    }
}