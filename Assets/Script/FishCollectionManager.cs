using System.Collections.Generic;
using UnityEngine;

public class FishCollectionManager : MonoBehaviour
{
    public static FishCollectionManager instance; // Singleton instance

    private List<GameObject> fishList = new List<GameObject>();
    private bool allFishCollected = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance of the manager exists.
            return;
        }

        // Find game objects with "RedFish" tag
        GameObject[] redFishObjects = GameObject.FindGameObjectsWithTag("RedFish");

        // Find game objects with "BlueFish" tag
        GameObject[] blueFishObjects = GameObject.FindGameObjectsWithTag("BlueFish");

        // Combine the arrays into one
        GameObject[] fishObjects = new GameObject[redFishObjects.Length + blueFishObjects.Length];
        redFishObjects.CopyTo(fishObjects, 0);
        blueFishObjects.CopyTo(fishObjects, redFishObjects.Length);

        foreach (GameObject fish in fishObjects)
        {
            fishList.Add(fish);
        }
    }

    public void CollectFish(GameObject fish)
    {
        // Remove the collected fish from the list
        fishList.Remove(fish);

        // Check if all fish are collected
        if (fishList.Count == 0)
        {
            allFishCollected = true;
            // GameManager.instance.EnableExitDoor();
        }
    }

    public bool AreAllFishCollected()
    {
        return allFishCollected;
    }
}
