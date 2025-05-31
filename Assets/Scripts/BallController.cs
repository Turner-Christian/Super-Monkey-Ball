using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController Instance; // Singleton instance for easy access
    public float moveForce = 10f; // Force applied to the ball when moving
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
    }
}
