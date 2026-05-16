using UnityEngine;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    public RectTransform leftImage;
    public RectTransform rightImage;

    public GameObject splashPanel;
    public GameObject cinematicPanel;

    public float moveSpeed = 2000f;

    void Start()
    {
        StartCoroutine(PlaySplash());
    }

    IEnumerator PlaySplash()
    {
        while (leftImage.anchoredPosition.x > -3000 || rightImage.anchoredPosition.x < 3000)
        {
            leftImage.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;
            rightImage.anchoredPosition += Vector2.right * moveSpeed * Time.deltaTime;

            yield return null;
        }

        // After animation finishes
        splashPanel.SetActive(false);
        cinematicPanel.SetActive(true);
    }
}