using UnityEngine;
using TMPro;
using System.Collections;

public class ComboManager : MonoBehaviour
{
    [Header("Assign Objects")]
    public TextMeshPro comboText;
    public TextMeshPro scoreText;
    public TextMeshPro missionText;
    public Transform camTransform;
    public SpriteRenderer damageFlashSprite;

    [Header("Settings")]
    public float pulseSpeed = 4f;
    private int currentCombo = 0;
    private Vector3 comboOrigScale;
    private Coroutine comboBreakRoutine;

    void Start()
    {
        if (comboText != null)
        {
            comboOrigScale = comboText.transform.localScale;
            comboText.text = "COMBO x0";
            comboText.color = new Color(1f, 0.6f, 0f); // Orange
        }

        if (damageFlashSprite != null)
        {
            Color c = damageFlashSprite.color;
            c.a = 0;
            damageFlashSprite.color = c;
        }
    }

    void Update()
    {
        // Breathing effect for the text when a combo is active
        if (currentCombo > 0 && comboText != null)
        {
            float breathing = 1.0f + Mathf.Sin(Time.unscaledTime * pulseSpeed) * 0.02f;
            comboText.transform.localScale = comboOrigScale * breathing;
        }
    }

    public void IncreaseCombo(int newScore)
    {
        // Stop the break animation if the player starts hitting enemies again
        if (comboBreakRoutine != null) StopCoroutine(comboBreakRoutine);

        currentCombo++;

        // FORMAT: COMBO x1, COMBO x2, etc.
        if(comboText != null) comboText.text = "COMBO x" + currentCombo;
        if(scoreText != null) scoreText.text = "SCORE: " + newScore;

        StopAllCoroutines();

        bool isMilestone = (currentCombo % 10 == 0);
        float multiplier = isMilestone ? 1.5f : 1.15f;

        StartCoroutine(PunchAndFlash(multiplier));

        if (isMilestone)
        {
            StartCoroutine(HitStop(0.12f));
            StartCoroutine(SmoothShake(0.3f, 0.4f));
        }

        UpdateColors();
    }

    public void UpdateMission(int remaining)
    {
        if(missionText != null) missionText.text = "ENEMIES LEFT: " + remaining;
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        if (comboBreakRoutine != null) StopCoroutine(comboBreakRoutine);
        comboBreakRoutine = StartCoroutine(ComboBreakSequence());
    }

    // Sequence for when the player gets hit
    private IEnumerator ComboBreakSequence()
    {
        if (comboText == null) yield break;

        // Visual feedback for breaking the streak
        comboText.color = Color.gray;
        comboText.text = "COMBO x0";

        float elapsed = 0f;
        float duration = 0.4f;
        float shakeMagnitude = 0.12f;
        Vector3 originalTextPos = comboText.transform.localPosition;

        // Violent text-only shake
        while (elapsed < duration)
        {
            comboText.transform.localPosition = originalTextPos + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        comboText.transform.localPosition = originalTextPos;
        comboText.color = new Color(1f, 0.6f, 0f); // Back to orange
    }

    public IEnumerator FlashRed()
    {
        if (damageFlashSprite == null) yield break;

        float t = 0;
        Color c = damageFlashSprite.color;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 20f;
            c.a = Mathf.Lerp(0, 0.4f, t);
            damageFlashSprite.color = c;
            yield return null;
        }
        t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 5f;
            c.a = Mathf.Lerp(0.4f, 0, t);
            damageFlashSprite.color = c;
            yield return null;
        }
    }

    IEnumerator PunchAndFlash(float multiplier)
    {
        Color targetColor = GetComboColor();
        comboText.color = Color.white;
        comboText.transform.localScale = comboOrigScale * multiplier;

        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 12f;
            comboText.color = Color.Lerp(Color.white, targetColor, t);
            comboText.transform.localScale = Vector3.Lerp(comboText.transform.localScale, comboOrigScale, t);
            yield return null;
        }
        comboText.color = targetColor;
    }

    IEnumerator HitStop(float duration)
    {
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    public IEnumerator SmoothShake(float duration, float magnitude)
    {
        if (camTransform == null) yield break;
        Vector3 startPos = camTransform.localPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            camTransform.localPosition = startPos + (Vector3)Random.insideUnitCircle * magnitude;
            magnitude = Mathf.Lerp(magnitude, 0, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        camTransform.localPosition = startPos;
    }

    void UpdateColors()
    {
        if(comboText != null) comboText.color = GetComboColor();
    }

    Color GetComboColor()
    {
        if (currentCombo >= 30) return new Color(0.6f, 0f, 1f); // Purple
        if (currentCombo >= 20) return Color.red;
        if (currentCombo >= 10) return new Color(1f, 0.84f, 0f); // Gold
        if (currentCombo >= 5) return Color.yellow;
        return new Color(1f, 0.6f, 0f); // Orange
    }
}