using UnityEngine;
using TMPro;
using System.Collections;

public class CinematicController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public string[] dialogueLines;
    public bool[] isVillainLine;

    public GameObject heroImage;
    public GameObject villainImage;

    public GameObject cinematicPanel;
    public GameObject startScreenPanel;

    public float typingSpeed = 0.03f;
    public float delayBetweenLines = 1.5f;

    public float holdDurationToSkip = 3.0f; // Updated to 3s to match your UI text
    private float holdTimer = 0f;

    int currentLine = 0;
    bool cinematicFinished = false;

    // We store the Coroutine so we can stop it specifically
    private Coroutine dialogueCoroutine;

    void OnEnable()
    {
        currentLine = 0;
        cinematicFinished = false;
        holdTimer = 0f;

        // Start the dialogue and store the reference
        dialogueCoroutine = StartCoroutine(PlayDialogue());
    }

    void Update()
    {
        HandleHoldToSkip();
    }

    IEnumerator PlayDialogue()
    {
        while (currentLine < dialogueLines.Length)
        {
            bool villainSpeaking = isVillainLine[currentLine];

            heroImage.SetActive(!villainSpeaking);
            villainImage.SetActive(villainSpeaking);

            yield return StartCoroutine(TypeLine(dialogueLines[currentLine]));

            yield return new WaitForSeconds(delayBetweenLines);

            currentLine++;
        }

        cinematicFinished = true;
        EndCinematic();
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void HandleHoldToSkip()
    {
        if (cinematicFinished)
            return;

        // Input.GetMouseButton(0) works anywhere on screen
        // as long as "Active Input Handling" is set to "Both" in Project Settings.
        if (Input.GetMouseButton(0))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdDurationToSkip)
            {
                EndCinematic();
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }

    public void EndCinematic()
    {
        if (cinematicFinished && !cinematicPanel.activeSelf) return;

        cinematicFinished = true;

        // Stop all typing and line-switching immediately
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }
        StopAllCoroutines();

        // Switch panels
        if (cinematicPanel != null) cinematicPanel.SetActive(false);
        if (startScreenPanel != null) startScreenPanel.SetActive(true);

        Debug.Log("Cinematic Ended/Skipped");
    }
}