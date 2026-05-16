using UnityEngine;

public class LockMessageUI : MonoBehaviour
{
    public GameObject messagePanel;

    public GameObject mapLockedText;
    public GameObject levelLockedText;

    public void ShowMapLocked()
    {
        messagePanel.SetActive(true);

        mapLockedText.SetActive(true);
        levelLockedText.SetActive(false);

        Invoke(nameof(HideMessage), 2f);
    }

    public void ShowLevelLocked()
    {
        messagePanel.SetActive(true);

        mapLockedText.SetActive(false);
        levelLockedText.SetActive(true);

        Invoke(nameof(HideMessage), 2f);
    }

    void HideMessage()
    {
        messagePanel.SetActive(false);
    }
}