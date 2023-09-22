using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private float fadeDuration = 3f; // Duration of the fade-in effect

    private void Start()
    {
        // Start with a fully transparent canvas
        mainMenuCanvasGroup.alpha = 0f;

        // Start the fade-in effect
        StartCoroutine(FadeInMainMenu());
    }

    private IEnumerator FadeInMainMenu()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            mainMenuCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration * 2);
            yield return null;
        }

        // Ensure the canvas is fully visible
        mainMenuCanvasGroup.alpha = 1f;
    }

    public void FadeOutTheMainMenu()
    {
        StartCoroutine(FadeOutMainMenu());
    }

    private IEnumerator FadeOutMainMenu()
    {
        float elapsedTime = 0f;
        float startAlpha = mainMenuCanvasGroup.alpha; // Get the current alpha value

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            mainMenuCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration * 2);
            yield return null;
        }

        // Ensure the canvas is fully transparent
        mainMenuCanvasGroup.alpha = 0f;
    }
}
