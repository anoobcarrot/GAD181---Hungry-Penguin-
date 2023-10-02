using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip powerUpSound;
    public GameObject currentTime;
    public Timer timer;
    public float destroyDelay = 1.0f;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentTime = GameObject.FindWithTag("Timer");
        timer = currentTime.GetComponent<Timer>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // set up to reset the timer back to 10 seconds on collision
            Debug.Log("collided with powerup");
            audioSource.PlayOneShot(powerUpSound);
            timer.currentTime = 10f;
            StartCoroutine(Destruct());
        }
    }

    //destroy powerup after delay
    IEnumerator Destruct()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(this.gameObject);
    }
}
