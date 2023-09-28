using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    private Transform playerTransform;
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;
    private bool isDestroyed = false;

    private void Start()
    {
        // Find the player
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Move enemy to player
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (!isDestroyed)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                isDestroyed = true;
                // Disable the collider to prevent further interactions
                Collider2D enemyCollider = GetComponent<Collider2D>();
                if (enemyCollider != null)
                {
                    enemyCollider.enabled = false;
                }

                // Destroy the enemy GameObject
                Destroy(gameObject);
            }
        }
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }
}
