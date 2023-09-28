using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    [SerializeField] private Image[] heartImages;

    // Reference to the PlayerMovement script
    private PlayerMovement playerMovement;

    // Flag to prevent multiple damage calls in a short time
    private bool canTakeDamage = true;

    private bool isEnemyDestroyed = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Get a reference to the PlayerMovement script
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(int damage)
    {
        // Check if the player can take damage and is not sliding
        if (!canTakeDamage || (playerMovement != null && playerMovement.IsSliding))
        {
            return; // Exit the function if the player can't take more damage yet or is sliding
        }

        // Check if an enemy was destroyed in the same frame
        if (isEnemyDestroyed)
        {
            return; // Exit the function if an enemy was destroyed recently
        }

        Debug.Log("Player took " + damage + " damage");

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("Game Over");
        }

        // Prevent further damage for a short time
        StartCoroutine(EnableDamageAfterDelay(1.0f));
    }

    private IEnumerator EnableDamageAfterDelay(float delay)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(delay);
        canTakeDamage = true;
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHealth)
            {
                // Set heart image to true
                heartImages[i].enabled = true;
            }
            else
            {
                // Set heart image to false
                heartImages[i].enabled = false;
            }
        }
    }

    // Method for the isEnemyDestroyed flag
    public void SetEnemyDestroyed(bool destroyed)
    {
        isEnemyDestroyed = destroyed;
    }
}
