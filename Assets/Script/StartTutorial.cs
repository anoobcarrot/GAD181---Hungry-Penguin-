using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartTutorial : MonoBehaviour
{
    [SerializeField] private float delayBeforeTransition = 1f; // Delay in seconds before transitioning
    [SerializeField] private CanvasGroup tutorialCanvasGroup;
    [SerializeField] private float fadeDuration = 2f; // Duration of the fade-in and fade-out effects

    private void Start()
    {
        // Start with a fully transparent canvas
        tutorialCanvasGroup.alpha = 0f;

        // Start the fade-in effect
        StartCoroutine(FadeInTutorial());
    }

    public void ContinueButtonClicked()
    {
        // Start the fade-out effect before transitioning
        StartCoroutine(FadeOutAndTransition());
    }

    private IEnumerator FadeInTutorial()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            tutorialCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure the canvas is fully visible
        tutorialCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOutAndTransition()
    {
        float elapsedTime = 0f;
        float startAlpha = tutorialCanvasGroup.alpha; // Get the current alpha value

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            tutorialCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure the canvas is fully transparent
        tutorialCanvasGroup.alpha = 0f;

        // Transition to the next scene after the fade-out effect
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes available. You've reached the end.");
        }
    }
}








