using UnityEngine;
using System.Collections;

public class NinjaCombat : MonoBehaviour
{
    private Animator anim;
    private Camera mainCam;

    [Header("Audio")]
    public AudioSource attackSound; // Drag your Kick/Punch AudioSource here

    [Header("Movement Settings")]
    public float lungeDistance = 0.5f;
    public float lungeSpeed = 15f;
    public float returnSpeed = 10f;

    [Header("Hurt Settings")]
    public float hurtBackDistance = 0.3f;
    public float hurtDuration = 0.6f;

    [Header("Bouncing Idle Settings")]
    public float bounceSpeed = 3.5f;
    public float bounceHeight = 0.05f;
    public float scaleStretch = 0.03f;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private bool isBusy = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
        originalPosition = transform.position;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (LevelManager.isGameOver) return;

        if (!isBusy)
        {
            HandleBouncingIdle();

            // LEFT CLICK / TAP → Smart attack (based on position)
            if (Input.GetMouseButtonDown(0))
            {
                DetermineAttackSide(Input.mousePosition);
            }

            // RIGHT CLICK → Always Right Attack
            if (Input.GetMouseButtonDown(1))
            {
                PerformAttack("Right");
            }
        }
        else
        {
            // Reset scale while busy
            transform.localScale = originalScale;
        }
    }

    void DetermineAttackSide(Vector3 inputPos)
    {
        Vector3 ninjaScreenPos = mainCam.WorldToScreenPoint(transform.position);

        if (inputPos.x < ninjaScreenPos.x)
        {
            PerformAttack("Left");
        }
        else
        {
            PerformAttack("Right");
        }
    }

    void HandleBouncingIdle()
    {
        float wave = Mathf.Sin(Time.time * bounceSpeed);
        float newYPos = originalPosition.y + (wave * bounceHeight);
        transform.position = new Vector3(originalPosition.x, newYPos, originalPosition.z);

        float newYScale = originalScale.y + (wave * scaleStretch);
        float newXScale = originalScale.x - (wave * (scaleStretch * 0.5f));
        transform.localScale = new Vector3(newXScale, newYScale, originalScale.z);
    }

    public void ApplyHurtHold()
    {
        isBusy = true;
        transform.localScale = originalScale;
        transform.position = originalPosition;
        StopAllCoroutines();
        StartCoroutine(HurtSequence());
    }

    void PerformAttack(string side)
    {
        isBusy = true;
        transform.localScale = originalScale;
        transform.position = originalPosition;

        // --- PLAY ATTACK SOUND ---
        if (attackSound != null)
        {
            // Randomize pitch slightly so every hit sounds unique
            attackSound.pitch = Random.Range(0.9f, 1.1f);
            attackSound.PlayOneShot(attackSound.clip);
        }

        int attackType = Random.Range(0, 2);
        float direction = (side == "Left") ? -1f : 1f;

        StartCoroutine(LungeSequence(direction));

        if (side == "Left")
            anim.SetTrigger(attackType == 0 ? "HitLeft" : "KickLeft");
        else
            anim.SetTrigger(attackType == 0 ? "Hit" : "Kick");
    }

    IEnumerator LungeSequence(float direction)
    {
        Vector3 targetPos = originalPosition + new Vector3(direction * lungeDistance, 0, 0);
        float t = 0;

        while (t < 1.0f)
        {
            t += Time.deltaTime * lungeSpeed;
            transform.position = Vector3.Lerp(transform.position, targetPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime * returnSpeed;
            transform.position = Vector3.Lerp(transform.position, originalPosition, t);
            yield return null;
        }

        transform.position = originalPosition;
        isBusy = false;
    }

    IEnumerator HurtSequence()
    {
        Vector3 hurtPos = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z + hurtBackDistance);
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * 15f;
            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, hurtPos.x, t),
                originalPosition.y,
                transform.position.z
            );
            yield return null;
        }

        yield return new WaitForSecondsRealtime(hurtDuration);

        t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime * returnSpeed;
            transform.position = Vector3.Lerp(transform.position, originalPosition, t);
            yield return null;
        }

        transform.position = originalPosition;
        isBusy = false;
    }
}