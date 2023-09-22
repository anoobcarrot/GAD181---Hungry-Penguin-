using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndSceneController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Text endText;

    [SerializeField] private CanvasGroup textCanvasGroup;
    [SerializeField] private CanvasGroup blackCanvasGroup;

    private float textDuration = 1f; // duration of when text appears after the video ends
    private float fadeDuration = 5f; // duration before fading to the main menu
    private float fadeToBlackDuration = 2f; // duration for fading to black

    private void Start()
    {
        // Start playing the video
        videoPlayer.Play();

        // Delay for before displaying text
        Invoke("DisplayEndText", (float)videoPlayer.length + 0f);
    }

    private void DisplayEndText()
    {
        // Display "The End" text
        endText.gameObject.SetActive(true);
        FadeInText(endText);
        FadeInCanvasGroup(textCanvasGroup);
        StartCoroutine(FadeToMainMenu(textDuration));
    }

    private IEnumerator FadeToMainMenu(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Fade out "The End" text
        FadeOutText(endText);

        // Set the alpha of the text canvas back to 0 to hide it
        textCanvasGroup.alpha = 0f;

        // Delay for 2 seconds before fading to black
        yield return new WaitForSeconds(fadeToBlackDuration);

        // Fade to black using the CanvasGroup
        FadeInCanvasGroup(blackCanvasGroup);

        // Load the main menu scene after the fade
        Invoke("LoadMainMenuScene", fadeDuration);
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene("Main Menu"); // Go to the main menu
    }

    // Helper method to fade in a UI element
    private void FadeInText(Graphic graphic)
    {
        graphic.CrossFadeAlpha(1f, fadeDuration, false); // Fade in the text
    }

    // Helper method to fade out a UI element
    private void FadeOutText(Graphic graphic)
    {
        graphic.CrossFadeAlpha(0f, fadeDuration, false); // Fade out the text
    }

    private void FadeInCanvasGroup(CanvasGroup group)
    {
        StartCoroutine(FadeCanvasGroup(group, group.alpha, 1f, fadeToBlackDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        group.alpha = endAlpha;
    }
}


