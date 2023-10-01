using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Fish collected by player");
            // Collect the red fish
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                // Disable the red fish object
                gameObject.SetActive(false);

                // Call the AlllowFlap method
                playerMovement.AllowFlap();
                
                // Notify the Fish Collection Manager that this fish is collected
                FishCollectionManager.instance.CollectFish(gameObject);
            }
        }
    }
}