using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController Instance; // Singleton instance for easy access
    public Transform child;
    public Transform guy;
    public float maxSpeed = 35f; // Maximum speed of the ball
    public float moveForce = 10f; // Force applied to the ball when moving
    public float mph; // Speed in miles per hour
    public Rigidbody rb; // Reference to the ball's Rigidbody component

    void Awake()
    {
        Instance = this; // Set the singleton instance
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the bal
    }

    void FixedUpdate()
    {
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

        if(rb.linearVelocity.magnitude > maxSpeed)
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
}
