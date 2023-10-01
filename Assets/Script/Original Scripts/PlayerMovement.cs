using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;
    private bool isGrounded = false;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Animator animator;

    private bool collectedBlueFish = false;
    private bool collectedRedFish = false;

    private bool isJumping = false;

    private bool isSlidingLeft = false;
    private bool isSlidingRight = false;
    private bool isSliding = false;
    private bool canSlide = false; // Flag to determine if sliding is allowed
    private float slideDuration = 3f; // Duration of the slide

    private bool isFlyingUp = false;
    private bool canFly = false; // Flag to determine if flying is allowed
    private float flyDuration = 2f; // Duration of the fly
    private float flyTimer = 0f; // Timer for flying
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
        // Check if the player is grounded 
        isGrounded = playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")); // Get Ground Layer

        // Move the player horizontally
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        // Get the player's current position
        Vector2 currentPosition = rb.position;

        // Update the player's position
        rb.position = currentPosition;

        bool isWalking = movement != Vector2.zero;
        animator.SetBool("IsWalking", isWalking);

        // Horizontal Parameter
        animator.SetFloat("Horizontal", horizontalInput);

        // Detect key press for jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Set the "IsJumping" parameter to true.
            animator.SetBool("IsJumping", true);

            // JUMPPP
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }

        // Detect key press for sliding(blue fish)
        if (Input.GetKeyDown(KeyCode.S) && isGrounded && canSlide && collectedBlueFish)
        {
            // Reset the collectedBlueFish flag
            collectedBlueFish = false;

            // Determine the direction of the slide animation based on the horizontal input
            isSlidingLeft = horizontalInput < 0;
            isSlidingRight = horizontalInput > 0;

            // Set the slide direction parameters in the Animator
            animator.SetBool("IsSlidingLeft", isSlidingLeft);
            animator.SetBool("IsSlidingRight", isSlidingRight);

            if (isSlidingLeft || isSlidingRight)
            {
                moveSpeed *= 2f; // Double the move speed while sliding left or right
                isSliding = true; // Set the sliding flag to true
                StartCoroutine(DisableSlideAfterDuration(slideDuration));
            }
        }

        // Detect key release to stop sliding animations and reset actions
        if (Input.GetKeyUp(KeyCode.S))
        {
            // Reset slide direction parameters in the Animator
            animator.SetBool("IsSlidingLeft", false);
            animator.SetBool("IsSlidingRight", false);

            moveSpeed = 5f; // Reset move speed to the default value
            isSliding = false; // Set the sliding flag to false
        }

        // Detect key press for flying (red fish)
        if (Input.GetKeyDown(KeyCode.W) && canFly && collectedRedFish)
        {
            Debug.Log("Is Flying");
            // Reset the collectedRedFish flag
            collectedRedFish = false;

            isFlyingUp = true;
            flyTimer = flyDuration;
            rb.gravityScale = 0f; // Disable gravity while flying

            // Set the flying direction parameters in the Animator
            animator.SetBool("IsFlyingUp", isFlyingUp);

            // Set the "IsFlying" parameter in the Animator to true
            animator.SetBool("IsFlying", true);
        }

        // Detect key release to stop flying
        if (Input.GetKeyUp(KeyCode.W))
        {
            isFlyingUp = false;
            rb.gravityScale = 1f; // Restore gravity after flying

            // Reset flying direction parameters in the Animator
            animator.SetBool("IsFlyingUp", isFlyingUp);

            // Set the "IsFlying" parameter in the Animator to false
            animator.SetBool("IsFlying", false);
        }

        // Update the flying timer
        if (isFlyingUp)
        {
            flyTimer -= Time.deltaTime;

            if (flyTimer <= 0f)
            {
                isFlyingUp = false;
                rb.gravityScale = 1f; // Restore gravity after flying

                // Reset flying direction parameters in the Animator
                animator.SetBool("IsFlyingUp", isFlyingUp);

                // Set the "IsFlying" parameter in the Animator to false
                animator.SetBool("IsFlying", false);
            }
        }

        if (isCollidingWithEnemy)
        {
            // If so, continuously damage the player's health
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Damage the player health
            }
        }
    }

        private void LateUpdate()
    {
        // Reset the jump direction parameters in the Animator when grounded
        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    private IEnumerator DisableSlideAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Disable sliding and reset sliding animation
        canSlide = false;
        isSlidingLeft = false;
        isSlidingRight = false;

        // Reset move speed to the default value
        moveSpeed = 5f;

        // Reset sliding animation
        animator.SetBool("IsSlidingLeft", false);
        animator.SetBool("IsSlidingRight", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BlueFish")) // Assuming "BlueFish" is the tag for the blue fish
        {
            // Handle the collection of the blue fish
            collectedBlueFish = true;

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RedFish")) // Assuming "RedFish" is the tag for the red fish
        {
            // Handle the collection of the red fish
            collectedRedFish = true;
            Debug.Log("Red fish collected");

            Destroy(other.gameObject);
        }

        else if (other.CompareTag("Enemy")) // Enemy Tag
        {
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


