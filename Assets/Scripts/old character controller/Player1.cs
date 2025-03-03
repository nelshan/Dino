using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player1 : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;
    private PlayerAnimatedSprite animatedSprite;

    public float jumpForce = 8f;
    public float gravity = 20f;
    private bool isCrouching = false;
    private bool gameStarted = false;

    private float originalRadius; // To store the original radius

    private void Awake()
    {
        character = GetComponent<CharacterController>();
        animatedSprite = GetComponent<PlayerAnimatedSprite>();
        originalRadius = character.radius; // Store the original radius value
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
        gameStarted = false;
        animatedSprite.SetAnimation(animatedSprite.idleSprites);
    }

    private void Update()
    {
        // Prevent movement & jumping before game starts
        if (!gameStarted && !Input.GetKeyDown(KeyCode.Space)) 
        {
            return;
        }

        // Apply gravity only when the player is not grounded
        if (!character.isGrounded)
        {
            direction.y -= gravity * Time.deltaTime;
        }
        else
        {
            // Tiny downward force to ensure ground detection
            direction.y = -0.1f;

            if (Input.GetKeyDown(KeyCode.Space))  // Use Space key for Jump
            {
                if (!gameStarted) // Start the game only on the first jump
                {
                    gameStarted = true;
                    animatedSprite.SetAnimation(animatedSprite.runningSprites);
                }
                // Apply the jump force upward
                direction.y = jumpForce;
            }
        }

        // Handle crouching only if the game has started
        if (gameStarted)
        {
            if (Input.GetKey(KeyCode.LeftShift))  // Use Left Shift for Crouch
            {
                isCrouching = true;
                animatedSprite.SetAnimation(animatedSprite.crouchSprites);
                
                // Set the CharacterController radius to 0.26 when crouching
                character.radius = 0.26f;
            }
            else if (isCrouching)
            {
                isCrouching = false;
                animatedSprite.SetAnimation(animatedSprite.runningSprites);
                
                // Restore the original radius when not crouching
                character.radius = originalRadius;
            }
        }

        // Move the player with the updated direction
        character.Move(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
            animatedSprite.SetAnimation(animatedSprite.dieSprites);
        }
    }
}



