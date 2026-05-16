using UnityEngine;
using TMPro;
using System.Collections;

public class ComboCelebration3D : MonoBehaviour
{
    private TextMeshPro textMesh;
    private Vector3 originalScale;
    private Vector3 startPos;
    private Transform mainCameraTransform;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip milestoneSound;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        originalScale = transform.localScale;
        startPos = transform.position;

        if (Camera.main != null)
            mainCameraTransform = Camera.main.transform;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (textMesh != null)
        {
            Color c = textMesh.color;
            c.a = 0;
            textMesh.color = c;
        }
    }

    public void ShowCelebration(int comboCount)
    {
        StopAllCoroutines();
        StartCoroutine(Animate3D(comboCount));

        // --- LOGIC: SHAKE AND SOUND ONLY ON MULTIPLES OF 10 (10, 20, 30...) ---
        if (comboCount > 0 && comboCount % 10 == 0)
        {
            // Trigger the Sound
            if (audioSource != null && milestoneSound != null)
            {
                audioSource.PlayOneShot(milestoneSound);
            }

            // Trigger the Shake
            TriggerBigShake();
        }
    }

    public void TriggerBigShake()
    {
        if (mainCameraTransform != null)
        {
            StartCoroutine(ShakeCamera(0.3f, 0.2f));
        }
    }

    IEnumerator Animate3D(int comboCount)
    {
        if (textMesh == null) yield break;

        SetMilestoneStyle(comboCount);

        transform.position = startPos;
        transform.localScale = originalScale * 0.1f;

        float t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime * 8f;
            float bounce = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.localScale = Vector3.Lerp(originalScale * 0.1f, originalScale * 1.2f, bounce);

            Color c = textMesh.color;
            c.a = t;
            textMesh.color = c;
            yield return null;
        }

        transform.localScale = originalScale * 1.3f;
        yield return new WaitForSeconds(0.05f);
        transform.localScale = originalScale * 1.2f;

        yield return new WaitForSeconds(0.8f);

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            Color c = textMesh.color;
            c.a = 1 - t;
            textMesh.color = c;
            transform.position = startPos + new Vector3(0, t * 0.5f, 0);
            yield return null;
        }

        transform.position = startPos;
        transform.localScale = originalScale;
    }

    private void SetMilestoneStyle(int comboCount)
    {
        textMesh.color = new Color(1, 1, 1, textMesh.color.a);

        if (comboCount >= 30) {
            textMesh.text = "<color=#FFD700>⚔️</color> <color=#FF3232>COMBO MASTER</color> <color=#FFD700>⚔️</color>";
        }
        else if (comboCount >= 25) {
            textMesh.text = "<color=#00FFFF>⚡</color> <color=#FFFF00>GODLIKE</color> <color=#00FFFF>⚡</color>";
        }
        else if (comboCount >= 20) {
            textMesh.text = "🔥 <color=#FF0000>UNSTOPPABLE</color> 🔥";
        }
        else if (comboCount >= 15) {
            textMesh.text = "<color=#9933FF>ELITE! x15</color>";
        }
        else if (comboCount >= 10) {
            textMesh.text = "<color=#00FF00>WARRIOR! x10</color>";
        }
        else if (comboCount >= 5) {
            textMesh.text = "<color=#80CCFF>BRUTAL! x5</color>";
        }
        else {
            textMesh.text = "COMBO x" + comboCount;
        }
    }

    IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPos = mainCameraTransform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            mainCameraTransform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCameraTransform.localPosition = originalPos;
    }
}