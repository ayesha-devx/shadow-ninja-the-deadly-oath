    using UnityEngine;

public class NinjaMovement : MonoBehaviour
{
    // You can change this number in the Unity Inspector to go faster or slower
    public float speed = 2.5f;

    void Update()
    {
        // Moves the ninja to the left every frame
        // 'Vector3.left' is a shortcut for (-1, 0, 0)
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Optional: If the ninja goes off-screen, you can reset him
        if (transform.position.x < -15f)
        {
            transform.position = new Vector3(15f, transform.position.y, 0);
        }
    }
}