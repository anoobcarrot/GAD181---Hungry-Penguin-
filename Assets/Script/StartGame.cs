using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{

    [SerializeField] private Button startButton;

    [SerializeField] private string nextSceneName;
    [SerializeField] private float delayBeforeTransition = 2f; // Adjust the delay as needed

    public void LoadGameScene()
    {

        // Start the delayed scene transition
        StartCoroutine(DelayedSceneTransition());
    }

    private IEnumerator DelayedSceneTransition()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeTransition);

        // Transition to the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}