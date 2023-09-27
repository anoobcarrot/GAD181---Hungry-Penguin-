using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EndSceneCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup endSceneCanvasGroup;
    [SerializeField] private float fadeDuration = 0f;

    private void Start()
    {
        // Set the initial alpha to fully transparent
        endSceneCanvasGroup.alpha = 0f;

        // Call a method to start fading in the CanvasGroup
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        // Start the fade-in process
        StartCoroutine(FadeCanvasGroup(endSceneCanvasGroup, 0f, 1f, fadeDuration));
    }

    public void StartFadeOut()
    {
        // Start the fade-out process
        StartCoroutine(FadeCanvasGroup(endSceneCanvasGroup, 1f, 0f, fadeDuration));
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

