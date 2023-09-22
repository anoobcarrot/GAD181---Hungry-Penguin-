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

    private bool isSlidingLeft = false;
    private bool isSlidingRight = false;
    private bool canSlide = false; // Flag to determine if sliding is allowed
    private float slideDuration = 3f; // Duration of the slide

    private bool isFlyingUp = false;
    private bool canFly = false; // Flag to determine if flying is allowed
    private float flyDuration = 2f; // Duration of the fly
    private float flyTimer = 0f; // Timer for flying
    private bool hasRedFish = false;
    private LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        // Check if the player is grounded using the Collider2D component.
        isGrounded = playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")); // Get Ground Layer

        // Move the player horizontally.
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        // Get the player's current position
        Vector2 currentPosition = rb.position;

        // Calculate screen boundaries
        Camera mainCamera = Camera.main;
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect; // Half of the camera's width
        float cameraHalfHeight = mainCamera.orthographicSize; // Half of the camera's height

        float minX = mainCamera.transform.position.x - cameraHalfWidth;
        float maxX = mainCamera.transform.position.x + cameraHalfWidth;
        float minY = mainCamera.transform.position.y - cameraHalfHeight;
        float maxY = mainCamera.transform.position.y + cameraHalfHeight;

        // Clamp the player's X position within the screen boundaries
        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);

        // Clamp the player's Y position within the screen boundaries
        currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

        // Update the player's position
        rb.position = currentPosition;

        // Detect key presses for walking animations.
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Set the "IsWalkingRight" parameter to true.
            animator.SetBool("IsWalkingRight", true);
            animator.SetBool("IsWalkingLeft", false); // Ensure the opposite direction is set to false
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // Set the "IsWalkingLeft" parameter to true.
            animator.SetBool("IsWalkingLeft", true);
            animator.SetBool("IsWalkingRight", false); // Ensure the opposite direction is set to false
        }

        // Detect key releases to stop walking animations and return to idle.
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            // Set both "IsWalkingRight" and "IsWalkingLeft" parameters to false.
            animator.SetBool("IsWalkingRight", false);
            animator.SetBool("IsWalkingLeft", false);
        }

        // Detect key press for jumping.
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Determine the direction of the jump animation based on the horizontal input.
            bool isJumpingLeft = horizontalInput < 0;
            bool isJumpingRight = horizontalInput > 0;

            // Perform the jump.
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // Set the jump direction parameters in the Animator.
            animator.SetBool("IsJumpingLeft", isJumpingLeft);
            animator.SetBool("IsJumpingRight", isJumpingRight);

            // Set the IsJumpingStationary parameter to true for a stationary jump.
            animator.SetBool("IsJumpingStationary", true);
        }

        // Detect key press for sliding(blue fish).
        if (Input.GetKeyDown(KeyCode.S) && isGrounded && canSlide && collectedBlueFish)
        {
            // Reset the collectedBlueFish flag.
            collectedBlueFish = false;

            // Determine the direction of the slide animation based on the horizontal input.
            isSlidingLeft = horizontalInput < 0;
            isSlidingRight = horizontalInput > 0;

            // Set the slide direction parameters in the Animator.
            animator.SetBool("IsSlidingLeft", isSlidingLeft);
            animator.SetBool("IsSlidingRight", isSlidingRight);

            if (isSlidingLeft || isSlidingRight)
            {
                moveSpeed *= 2f; // Double the move speed while sliding left or right.
                StartCoroutine(DisableSlideAfterDuration(slideDuration));
            }
        }

        // Detect key release to stop sliding animations and reset actions.
        if (Input.GetKeyUp(KeyCode.S))
        {
            // Reset slide direction parameters in the Animator.
            animator.SetBool("IsSlidingLeft", false);
            animator.SetBool("IsSlidingRight", false);

            moveSpeed = 5f; // Reset move speed to the default value.
        }

        // Detect key press for flying (red fish).
        if (Input.GetKeyDown(KeyCode.W) && canFly && collectedRedFish)
        {
            Debug.Log("Is Flying");
            // Reset the collectedRedFish flag.
            collectedRedFish = false;

            isFlyingUp = true;
            flyTimer = flyDuration;
            rb.gravityScale = 0f; // Disable gravity while flying

            // Set the flying direction parameters in the Animator.
            animator.SetBool("IsFlyingUp", isFlyingUp);

            // Set the "IsFlying" parameter in the Animator to true.
            animator.SetBool("IsFlying", true); // Add this line
        }

        // Detect key release to stop flying.
        if (Input.GetKeyUp(KeyCode.W))
        {
            isFlyingUp = false;
            rb.gravityScale = 1f; // Restore gravity after flying

            // Reset flying direction parameters in the Animator.
            animator.SetBool("IsFlyingUp", isFlyingUp);

            // Set the "IsFlying" parameter in the Animator to false.
            animator.SetBool("IsFlying", false); // Add this line
        }


        // Update the flying timer.
        if (isFlyingUp)
        {
            flyTimer -= Time.deltaTime;

            if (flyTimer <= 0f)
            {
                isFlyingUp = false;
                rb.gravityScale = 1f; // Restore gravity after flying

                // Reset flying direction parameters in the Animator.
                animator.SetBool("IsFlyingUp", isFlyingUp);

                // Set the "IsFlying" parameter in the Animator to false.
                animator.SetBool("IsFlying", false); // Add this line
            }
        }
    }

        private void LateUpdate()
    {
        // Reset the jump direction parameters in the Animator when grounded.
        if (isGrounded)
        {
            animator.SetBool("IsJumpingLeft", false);
            animator.SetBool("IsJumpingRight", false);

            // Reset the IsJumpingStationary parameter.
            animator.SetBool("IsJumpingStationary", false);

        }
    }

    private IEnumerator DisableSlideAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Disable sliding and reset sliding animation.
        canSlide = false;
        isSlidingLeft = false;
        isSlidingRight = false;

        // Reset move speed to the default value.
        moveSpeed = 5f;

        // Reset sliding animation.
        animator.SetBool("IsSlidingLeft", false);
        animator.SetBool("IsSlidingRight", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BlueFish")) // Assuming "BlueFish" is the tag for the blue fish.
        {
            // Handle the collection of the blue fish.
            collectedBlueFish = true;

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("RedFish")) // Assuming "RedFish" is the tag for the red fish.
        {
            // Handle the collection of the red fish.
            collectedRedFish = true;
            Debug.Log("Red fish collected");

            Destroy(other.gameObject);
        }
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


