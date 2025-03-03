using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Transform playerStartPosition; // Reference to the starting position of the player

    private Player player;
    private Spawner spawner;

    private float score;
    public float Score => score;

    public bool gameStarted = false;  // To track if the game has started
    public bool gameOver = false; // To track if the game is in the game-over state

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // Find Player and Spawner
        player = FindAnyObjectByType<Player>();
        spawner = FindAnyObjectByType<Spawner>();

        // Prepare for a new game (but don't start yet)
        NewGame();
    }

    private void Update()
    {
        // Start the game when the player presses the jump button (Space key)
        if (!gameStarted && !gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            gameStarted = true;
            player.gameObject.SetActive(true);
            spawner.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(true);
            player.GetComponent<PlayerAnimatedSprite>().SwitchToRunning();  // Switch to running
        }

        if (gameStarted && !gameOver)
        {
            gameSpeed += gameSpeedIncrease * Time.deltaTime;
            score += gameSpeed * Time.deltaTime;
            scoreText.text = Mathf.FloorToInt(score).ToString("D5");

            // Handle crouching when Left Shift is pressed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                player.GetComponent<PlayerAnimatedSprite>().SwitchToCrouching();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift)) // Stop crouching when Left Shift is released
            {
                player.GetComponent<PlayerAnimatedSprite>().SwitchToRunning();
            }
        }
    }

    public void NewGame()
    {
        // Ensure obstacles are removed at the start of a new game
        Obstacle[] obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        score = 0f;
        gameSpeed = initialGameSpeed;

        // Set initial states for the game
        gameStarted = false;
        gameOver = false; // Reset game over state
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        // Reset player's position to start position (e.g., on the ground)
        player.transform.position = playerStartPosition.position; // Make sure playerStartPosition is set to the correct position

        player.GetComponent<PlayerAnimatedSprite>().SetAnimation(player.GetComponent<PlayerAnimatedSprite>().idleSprites); // Set idle sprite
        UpdateHiscore();
        
        // Enable the player components (Player and PlayerAnimatedSprite)
        player.GetComponent<Player>().enabled = true;
        player.GetComponent<PlayerAnimatedSprite>().enabled = true;
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        gameStarted = false;
        gameOver = true; // Set game-over state to true

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        player.GetComponent<PlayerAnimatedSprite>().SwitchToDie(); // Switch to die sprite

        // Disable the player components (Player and PlayerAnimatedSprite) when game is over
        player.GetComponent<Player>().enabled = false;
        player.GetComponent<PlayerAnimatedSprite>().enabled = false;

        UpdateHiscore();
    }

    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }
}
