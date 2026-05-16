using UnityEngine;

public class SelfDestruct : MonoBehaviour {
    void Start() {
        // Random rotation to make every hit look unique
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        // Destroy after 0.1 seconds (fast flash)
        Destroy(gameObject, 0.1f);
    }
}