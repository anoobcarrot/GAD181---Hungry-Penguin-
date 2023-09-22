using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Fish collected by player");
            // Collect the item and allow sliding in the player.
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.AllowSlide();
                gameObject.SetActive(false);
                // Notify the Fish Collection Manager that this fish is collected.
                FishCollectionManager.instance.CollectFish(gameObject);
            }
        }
    }
}





