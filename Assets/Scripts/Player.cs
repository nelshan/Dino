using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private SpriteRenderer spriteRenderer;
    private PlayerAnimatedSprite animatedSprite;

    public float jumpForce = 8f;
    public float gravity = 20f;
    private bool isCrouching = false;
    private bool gameStarted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animatedSprite = GetComponent<PlayerAnimatedSprite>();

        UpdateColliderSize(); // Ensure the collider matches the initial sprite
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector3.zero;
        gameStarted = false;
        animatedSprite.SetAnimation(animatedSprite.idleSprites);
        UpdateColliderSize(); // Reset collider to match the idle sprite
    }

    private void Update()
    {
        // Prevent movement & jumping before game starts
        if (!gameStarted && !Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        // Apply gravity manually
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))  // Use Space key for Jump
            {
                if (!gameStarted) // Start the game only on the first jump
                {
                    gameStarted = true;
                    animatedSprite.SetAnimation(animatedSprite.runningSprites);
                    UpdateColliderSize(); // Update collider for running sprite
                }
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            }
        }

        // Handle crouching only if the game has started
        if (gameStarted)
        {
            if (Input.GetKey(KeyCode.LeftShift))  // Use Left Shift for Crouch
            {
                isCrouching = true;
                animatedSprite.SetAnimation(animatedSprite.crouchSprites);
                UpdateColliderSize(); // Update collider to match crouching sprite
            }
            else if (isCrouching)
            {
                isCrouching = false;
                animatedSprite.SetAnimation(animatedSprite.runningSprites);
                UpdateColliderSize(); // Restore collider for running sprite
            }
        }
    }

    private bool IsGrounded()
    {
        // Simple grounded check using Raycast
        return Physics.Raycast(transform.position, Vector3.down, 0.6f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
            animatedSprite.SetAnimation(animatedSprite.dieSprites);
            UpdateColliderSize(); // Update collider for death sprite
        }
    }

    private void UpdateColliderSize()
    {
        if (spriteRenderer.sprite == null) return;

        // Get the sprite bounds
        Bounds spriteBounds = spriteRenderer.sprite.bounds;

        // Adjust BoxCollider size and position to match sprite size
        boxCollider.size = new Vector3(spriteBounds.size.x, spriteBounds.size.y, 0.2f); // Keep some thickness on Z-axis
        boxCollider.center = new Vector3(spriteBounds.center.x, spriteBounds.center.y, 0); // Keep collider centered
    }
}
