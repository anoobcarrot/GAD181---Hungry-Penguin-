using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private int collectedRedFishCount = 0;

    private bool isGrounded = false;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Animator animator;

    private bool collectedBlueFish = false;

    private bool isFlyingUp = false;
    private bool canFly = false; // If flying is allowed
    private float flyDuration = 2f; // Fly duration
    private bool isFlyingOnCooldown = false;
    private float flyCooldown = 0.8f; // Cooldown between flaps

    private bool isWalking = false;
    private bool isSliding = false;
    private bool canSlide = false; // Flag to determine if sliding is allowed
    private float slideDuration = 3f; // Duration of the slide

    private LayerMask groundLayer;

    private PlayerHealth playerHealth;

    private bool isCollidingWithEnemy = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("Ground");

        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        // Define a ray that goes straight down from the player's position
        Vector2 rayOrigin = rb.position;
        Vector2 rayDirection = Vector2.down;
        float rayDistance = 1f; // Adjust this distance as needed

        // Perform a raycast to check if the player is grounded
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayer);

        // Check if the raycast hits something on the "Ground" layer
        isGrounded = hit.collider != null;

        // Move the player horizontally
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        // Get the player's current position
        Vector2 currentPosition = rb.position;

        // Update the player's position
        rb.position = currentPosition;

        //for animations
        if (movement.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
        //for animations
        animator.SetBool("IsWalking", isWalking);
        //for animations
        if (movement.y < 0.01f && movement.y > -0.01f)
        {
            animator.SetBool("IsJumping", false);
        }

        // Horizontal Parameter
        animator.SetFloat("Horizontal", horizontalInput);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Perform the jump
            animator.SetBool("IsJumping", true);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            
        }

        // Detect key press for flying (red fish)
        if (Input.GetKeyDown(KeyCode.Space) && canFly && collectedRedFishCount > 0 && !isGrounded && !isFlyingOnCooldown)
        {
            Debug.Log("Is Flap");
            isFlyingUp = true;
            rb.velocity = Vector2.zero; // Reset the current velocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // Set the flying direction parameters in the Animator
            // animator.SetBool("IsFlyingUp", isFlyingUp);

            // Set the "IsFlying" parameter in the Animator to true
            // animator.SetBool("IsFlying", true);

            // Disable flying after a duration
            StartCoroutine(DisableFlyingAfterDuration());

            // Set cooldown between flaps
            isFlyingOnCooldown = true;
            StartCoroutine(ResetFlyingCooldown());
        }

        // Detect key press for sliding(blue fish)
        if (Input.GetKeyDown(KeyCode.S) && isGrounded && canSlide && collectedBlueFish)
        {
            // Reset the collectedBlueFish flag
            collectedBlueFish = false;

            // Set the slide direction parameters in the Animator
            animator.SetBool("IsSliding", true);

           
            moveSpeed *= 2f; // Double the move speed while sliding left or right
            isSliding = true; // Set the sliding flag to true
            StartCoroutine(DisableSlideAfterDuration(slideDuration));
            Debug.Log("is sliding");
           
        }

        if (isCollidingWithEnemy && isSliding == false)
        {
            // If so, continuously damage the player's health
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Damage the player health
            }
        }

        // Detect key release to stop sliding animations and reset actions
        if (Input.GetKeyUp(KeyCode.S))
        {
            // Reset slide direction parameters in the Animator
            animator.SetBool("IsSliding", false);

            moveSpeed = 5f; // Reset move speed to the default value
            isSliding = false; // Set the sliding flag to false
        }

       
    }

    private void LateUpdate()
    {
        // Reset the jump direction parameters in the Animator when grounded
        if (isGrounded)
        {
            //animator.SetBool("IsJumping", false);
        }
    }

    // Disable flying after a duration
    private IEnumerator DisableFlyingAfterDuration()
    {
        yield return new WaitForSeconds(flyDuration);

        if (collectedRedFishCount > 0)
        {
            collectedRedFishCount--; // Decrease the count
        }
        else
        {
            // If no redfish left, disable flying
            canFly = false;
            Debug.Log("Fly disabled");
        }

        isFlyingUp = false;
        rb.gravityScale = 1f; // Restore gravity after flying

        // Reset flying direction parameters in the Animator
        animator.SetBool("IsFlyingUp", isFlyingUp);

            // Set the "IsFlying" parameter in the Animator to false
            animator.SetBool("IsFlying", false);
    }

    // Reset the flying cooldown to flap again
    private IEnumerator ResetFlyingCooldown()
    {
        yield return new WaitForSeconds(flyCooldown);
        isFlyingOnCooldown = false; // Reset the flying cooldown
    }

    private IEnumerator DisableSlideAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Disable sliding and reset sliding animation
        canSlide = false;
        isSliding = false;

        // Reset move speed to the default value
        moveSpeed = 5f;

        animator.SetBool("IsSliding", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BlueFish")) // For interacting with Blue Fish
        {
            // Collection of the blue fish
            collectedBlueFish = true;

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RedFish")) // For interacting with Red Fish
        {
            // Collect the red fish
            collectedRedFishCount++;

            Debug.Log("Red fish collected");

            // Disable the collected red fish object
            other.gameObject.SetActive(false);

            // Notify the Fish Collection Manager that this fish is collected
            FishCollectionManager.instance.CollectFish(other.gameObject);
        }

        else if (other.CompareTag("Enemy")) // Enemy Tag
        {
            Debug.Log("collided with enemy");
            // Check if player is currently sliding
            if (isSliding)
            {
                // Get enemy script
                EnemyAI enemy = other.GetComponent<EnemyAI>();

                // Check if the enemy exists and is not destroyed
                if (enemy != null && !enemy.IsDestroyed())
                {
                    enemy.TakeDamage(1); // Damage enemy 
                    playerHealth.SetEnemyDestroyed(true); // Set the enemy destroyed flag
                    Debug.Log("Enemy damaged by player sliding.");
                }
            }

            // Set a flag for the player colliding with enemy
            isCollidingWithEnemy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Enemy Tag
        {
            // Reset the flag 
            isCollidingWithEnemy = false;
        }
    }

    public bool IsSliding
    {
        get { return isSliding; }
    }

    public void AllowSlide()
    {
        canSlide = true;
    }

    public void AllowFly()
    {
        canFly = true;
        Debug.Log("Fly enabled");
    }
}


