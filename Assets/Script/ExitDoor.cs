using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private GameManager gameManager;
    private bool playerInRange = false;

    private void Start()
    {
        // Find the GameManager in the scene.
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        // Check if the player is in range (inside the door's collider) and is right-clicking
        if (playerInRange && Input.GetMouseButtonDown(1) && gameManager != null)
        {
            // Check if all fish are collected before allowing access
            if (FishCollectionManager.instance.AreAllFishCollected())
            {
                // Load the next level
                gameManager.LoadNextLevel();
            }
            else
            {
                // Testing purposes
                Debug.Log("Cannot access the door. Collect all fish first.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            // Player is in range.
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            // Player is out of range
            playerInRange = false;
        }
    }
}

