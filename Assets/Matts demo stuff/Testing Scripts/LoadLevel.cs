using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public string levelName; // the name of the scene to load
    public float loadLevelDelay = 6.0f; //The delay before the scene is loaded after the trigger is entered
    public GameObject playerObject; // Reference to the game object to disable

    private void OnTriggerEnter2D (Collider2D other)
    {
        //Check if the collider is a player
        if(other.CompareTag("Player"))
        {
            //Disable the player game object
            playerObject.SetActive(false);

            //Call The LoadLevelDelayed method after a delay
            Invoke("LoadLevelDelayed", loadLevelDelay);
        }
    }

   private void LoadLevelDelayed()
    {
        // Load the specified level
        SceneManager.LoadScene(levelName);
    }
}
