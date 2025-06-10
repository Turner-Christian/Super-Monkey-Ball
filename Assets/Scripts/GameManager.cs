using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance for easy access
    public Camera MainCamera; // Reference to the main camera
    public Camera GoalCamera; // Reference to the goal camera
    public TextMeshProUGUI TimerText; // UI Text to display the timer
    public TextMeshProUGUI SpeedText; // UI Text to display the speed
    public TiltCameraByInput tiltCameraByInput; // Reference to the TiltCameraByInput script
    public static Vector3 PlayerStartPosition; // Position to respawn the player
    public static Vector3 PlayerStartRotation; // Rotation to respawn the player
    public static bool PlayerFallen;
    public static bool _playerAlive = true; // Flag to indicate if the player is alive
    public static bool goalScored = false; // Flag to indicate if a goal has been scored
    private bool _respawnScheduled = false; // Flag to prevent multiple respawn calls
    private const float _RespawnDelay = 1.5f; // Delay before respawning the player
    private float _timer = 60f; // Timer for the game, set to 60 seconds
    private bool _timerRunning = true; // Flag to indicate if the timer is running

    [SerializeField]
    private float _customGravity = -60f; // Custom gravity value

    void Awake()
    {
        Instance = this; // Set the singleton instance
        Physics.gravity = new Vector3(0, _customGravity, 0); // Set custom gravity
        MainCamera.gameObject.SetActive(true); // Activate the main camera
        GoalCamera.gameObject.SetActive(false); // Deactivate the goal camera
    }

    void Update()
    {
        SpeedText.text = "MPH " + BallController.Instance.mph.ToString(); // Update the speed text
        if (PlayerFallen && !_respawnScheduled)
        {
            PlayerDead(); // Call PlayerDead if the player has fallen and respawn is not scheduled
        }

        if (_timerRunning)
        {
            TimerText.text = Mathf.FloorToInt(_timer).ToString(); // Update the timer text
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _timer = 0f;
                _timerRunning = false; // Stop the timer when it reaches zero
                PlayerDead(); // Call PlayerDead when the timer runs out
            }
        }

        if (!_playerAlive && !_respawnScheduled)
        {
            PlayerDead(); // Respawn the player if they are not alive and respawn is not scheduled
        }
    }

    public void GoalScored()
    {
        // Trigger Confetti
        // Invoke a respawn and change the position of spawn
        // Goal animation for the player
        goalScored = true; // Set the goal scored flag
        MainCamera.gameObject.SetActive(false); // Deactivate the main camera
        GoalCamera.gameObject.SetActive(true); // Activate the goal camera
        Physics.gravity = new Vector3(0, 0, 0); // Set gravity to zero when the goal is scored
        Invoke("ReverseGravity", 1f); // Reverse gravity after a short delay
        _timerRunning = false; // Stop the timer
    }

    private void PlayerDead()
    {
        _respawnScheduled = true; // Set flag to prevent multiple respawns
        _playerAlive = false; // Set player alive state to false
        PlayerFallen = true; // Set player fallen state to true
        Invoke("RespawnPlayer", _RespawnDelay); // Schedule player respawn
    }

    private void RespawnPlayer()
    {
        BallController.Instance.rb.rotation = Quaternion.Euler(0, 180f, 0); // Reset player rotation to face the correct direction
        BallController.Instance.transform.position = PlayerStartPosition; // Respawn the player at the start position
        BallController.Instance.rb.linearVelocity = Vector3.zero; // Reset velocity
        BallController.Instance.rb.angularVelocity = Vector3.zero; // Reset angular velocity
        PlayerFallen = false; // Reset fallen state
        _respawnScheduled = false; // Reset respawn flag
        _timer = 60f; // Reset the timer to 60 seconds
        _timerRunning = true; // Restart the timer
        _playerAlive = true; // Set player alive state to true
        tiltCameraByInput.ResetTilt(); // Reset camera tilt
    }

    private void ReverseGravity()
    {
        Physics.gravity = new Vector3(0, -_customGravity, 0); // Reverse the gravity direction
    }
}
