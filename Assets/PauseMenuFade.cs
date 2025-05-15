using System.Collections;
using UnityEngine;

public class PauseMenuFade : MonoBehaviour
{
    public CanvasGroup pauseMenuCanvasGroup;
    public float fadeDuration = 1f; // Duration of the fade in seconds

    void Awake()
    {
        if (pauseMenuCanvasGroup == null)
        {
            pauseMenuCanvasGroup = GetComponent<CanvasGroup>();
        }
        pauseMenuCanvasGroup.alpha = 0f; // Ensure alpha is 0 at the start
        gameObject.SetActive(false); // Ensure the GameObject is inactive at the start
    }

    public IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;

        // Ensure the canvas group is initially invisible and not interactable
        pauseMenuCanvasGroup.alpha = 0f;
        pauseMenuCanvasGroup.interactable = false;
        pauseMenuCanvasGroup.blocksRaycasts = false;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            pauseMenuCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure the canvas group is fully visible and interactable after fading in
        pauseMenuCanvasGroup.alpha = 1f;
        pauseMenuCanvasGroup.interactable = true;
        pauseMenuCanvasGroup.blocksRaycasts = true;
    }



    public IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;

        // Ensure the canvas group is fully visible and interactable
        pauseMenuCanvasGroup.alpha = 1f;
        pauseMenuCanvasGroup.interactable = true;
        pauseMenuCanvasGroup.blocksRaycasts = true;

        // Fade out loop
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            pauseMenuCanvasGroup.alpha = Mathf.Clamp01(1f - elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure the canvas group is completely invisible and not interactable after fading out
        pauseMenuCanvasGroup.alpha = 0f;
        pauseMenuCanvasGroup.interactable = false;
        pauseMenuCanvasGroup.blocksRaycasts = false;
    }
}
