using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBarrier : MonoBehaviour
{
    [SerializeField] private Timer timer; // ref to timer script


    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the collider is a player
        if (other.CompareTag("Player"))
        {

            // Call the StopTimer method from the Timer script
            if (timer != null)
            {
                timer.GameOver();
            }

        }
    }
}
