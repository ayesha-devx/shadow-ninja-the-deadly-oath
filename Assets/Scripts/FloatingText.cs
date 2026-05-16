using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 2f;      // How fast it floats up
    public float destroyTime = 0.8f;   // How long before it disappears
    public Vector3 offset = new Vector3(0, 0.5f, 0); // Extra height offset

    private TextMeshPro textMesh;
    private Color textColor;
    private float timer;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh != null)
        {
            textColor = textMesh.color;
        }

        // Add the initial offset to the starting position
        transform.position += offset;

        // Auto-destroy the object after the set time
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // 1. Move the text upward over time
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // 2. Fade out logic
        if (textMesh != null)
        {
            timer += Time.deltaTime;
            // Calculate transparency (alpha) based on remaining time
            float alpha = 1 - (timer / destroyTime);
            textColor.a = Mathf.Clamp01(alpha);
            textMesh.color = textColor;
        }
    }
}