using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons; // An array to hold references to the level selection buttons.

    private void Start()
    {
        // Loop through the levelButtons array and set up click events for each button.
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1; // Level numbers start from 1.

            // Add a click event listener to each button.
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    // Function to load a selected level.
    void LoadLevel(int levelIndex)
    {
        // Construct the scene name based on your naming convention.
        string sceneName = "Level " + levelIndex;

        // Load the selected level.
        SceneManager.LoadScene(sceneName);
    }
}
