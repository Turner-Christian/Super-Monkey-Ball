using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController Instance; // Singleton instance for easy access
    public Animator GoalsAnimator; // Animator for goal animations
    public Transform child;
    public Transform guy;
    public float decelerationSpeed = 2f; // Deceleration factor for the ball
    public float maxSpeed = 35f; // Maximum speed of the ball
    public float moveForce = 10f; // Force applied to the ball when moving
    public float mph; // Speed in miles per hour
    public Rigidbody rb; // Reference to the ball's Rigidbody component
    private bool _shouldDecelerate = false; // Flag to indicate if the ball should decelerate

    void Awake()
    {
        Instance = this; // Set the singleton instance
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the bal
        GameManager.PlayerStartPosition = transform.position; // Set the player's start position
    }

    void FixedUpdate()
    {
        if (_shouldDecelerate)
        {
            // Decelerate the ball
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            Vector3 slowedHorizontal = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.fixedDeltaTime * decelerationSpeed);
            rb.linearVelocity = new Vector3(slowedHorizontal.x, rb.linearVelocity.y, slowedHorizontal.z);
            if (rb.linearVelocity.magnitude < 0.1f) // Stop decelerating when close to zero
            {
                _shouldDecelerate = false;
            }
        }

        if (!GameManager._playerAlive || Camera.main == null)
        {
            return; // Exit early if the player is not alive
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Get camera forward direction, projected onto XZ plane
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        // Get right vector from forward
        Vector3 camRight = new Vector3(camForward.z, 0, -camForward.x);

        // Combine input with camera direction
        Vector3 moveDir = (camForward * moveZ + camRight * moveX).normalized;

        rb.AddForce(moveDir * moveForce);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed; // Limit speed to 10 units
        }

    }

    void LateUpdate()
    {
        Vector3 velocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        mph = Mathf.FloorToInt(velocity.magnitude * 2.23694f); // Convert to MPH (1 m/s = 2.23694 mph)

        // Deadzone to avoid jitter when the ball is barely moving
        if (velocity.sqrMagnitude > 0.01f)
        {
            // Make the child look in the direction of velocity, using its local Z axis
            child.rotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
            guy.rotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathBox"))
        {
            GameManager._playerAlive = false; // Set player fallen state
            Debug.Log("Ball has fallen off the map!");
        }
        if (other.CompareTag("GoalBox") && GameManager._playerAlive)
        {
            GoalsAnimator.SetTrigger("onGoal"); // Trigger goal animation
            GameManager.goalScored = true; // Set goal scored state
            _shouldDecelerate = true; // Set deceleration flag
        }
    }
}
