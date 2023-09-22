using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float totalTime = 10f; // Total time for the level.
    private float currentTime; // Current time remaining.
    private bool playerReachedDoor = false;

    [SerializeField] private GameObject exitDoor;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject fadeObject; // Reference to the GameObject with the sprite to fade
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    [SerializeField] private float fadeDuration = 0.2f; // Duration of the fade-in effect

    private void Start()
    {
        currentTime = totalTime;
        UpdateTimerDisplay();
        spriteRenderer = fadeObject.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeInLevel());
    }

    private void Update()
    {
        if (!playerReachedDoor)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (currentTime <= 0f)
            {
                // Player ran out of time, perform game over actions.
                GameOver();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();
    }

    public bool PlayerReachedDoor()
    {
        // Called when the player reaches the exit door.
        if (!playerReachedDoor)
        {
            playerReachedDoor = true;
            StartCoroutine(FadeOutAndLoadNextLevel(2f));
        }

        return playerReachedDoor;
    }

    private IEnumerator FadeInLevel()
    {
        // Start with a fully transparent sprite and its children
        SetAlphaRecursively(fadeObject.transform, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); // Change 0f to 1f here
            SetAlphaRecursively(fadeObject.transform, alpha);
            yield return null;
        }

        // Ensure the sprite and its children are fully opaque
        SetAlphaRecursively(fadeObject.transform, 1f);
    }

    // Recursive function to set alpha for a GameObject and its children
    private void SetAlphaRecursively(Transform parent, float alpha)
    {
        foreach (Transform child in parent)
        {
            if (child.TryGetComponent<SpriteRenderer>(out SpriteRenderer childRenderer))
            {
                Color childColor = childRenderer.color;
                childColor.a = alpha;
                childRenderer.color = childColor;
            }
        }

        // Set the alpha for the parent object
        if (parent.TryGetComponent<SpriteRenderer>(out SpriteRenderer parentRenderer))
        {
            Color parentColor = parentRenderer.color;
            parentColor.a = alpha;
            parentRenderer.color = parentColor;
        }
    }

    public void LoadNextLevel()
    {
        // Get the current scene's build index.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the next scene by incrementing the build index.
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is valid (within the build settings).
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If there is no next scene, you can handle it accordingly.
            Debug.LogWarning("There is no next scene in the build settings.");
        }
    }

    private IEnumerator FadeOutAndLoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        float startAlpha = spriteRenderer.color.a; // Get the current alpha value

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f); // Ensure the sprite is fully opaque

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("There is no next scene in the build settings.");
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}




