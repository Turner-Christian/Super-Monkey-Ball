using UnityEngine;

public class BallController : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public float moveForce = 10f; // Force applied to the ball when moving
    public float maxZAngle = 35f; // Variable to store the rotation angle
    public float maxXAngle = 15f; // Variable to store the rotation angle
    public float maxYOffset = 0.4f; // Maximum Y offset for the camera when moving forward
    public float cameraSmoothness = 0.5f; // Smoothness factor for camera movement
    private Rigidbody _rb; // Reference to the ball's Rigidbody component
    private float _originalXAngle;

    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the bal
        _originalXAngle = mainCamera.transform.eulerAngles.x; // Store the initial X angle of the camera
    }

    void Update()
    {
        // GOING LEFT
        if (Input.GetAxis("Horizontal") < 0)
        {
            float input = Input.GetAxis("Horizontal");
            float targetAngle = Mathf.Clamp(input * maxZAngle, -maxZAngle, maxZAngle); // Clamp the angle to the max angle

            mainCamera.transform.eulerAngles = new Vector3(
                mainCamera.transform.eulerAngles.x,
                mainCamera.transform.eulerAngles.y,
                targetAngle
            );
        }
        // GOING RIGHT
        else if (Input.GetAxis("Horizontal") > 0)
        {
            float input = Input.GetAxis("Horizontal");
            float targetAngle = Mathf.Clamp(input * maxZAngle, -maxZAngle, maxZAngle); // Clamp the angle to the max angle

            mainCamera.transform.eulerAngles = new Vector3(
                mainCamera.transform.eulerAngles.x,
                mainCamera.transform.eulerAngles.y,
                targetAngle
            );
        }
        else
        {
            mainCamera.transform.eulerAngles = new Vector3(
                mainCamera.transform.eulerAngles.x,
                mainCamera.transform.eulerAngles.y,
                0 // Reset the angle when no input is detected
            );
        }
        // GOING FORWARD
        if (Input.GetAxis("Vertical") > 0)
        {
            float input = Input.GetAxis("Vertical");
            float targetAngle = Mathf.Clamp(_originalXAngle - input * maxXAngle, _originalXAngle - maxXAngle, _originalXAngle);
            mainCamera.transform.eulerAngles = new Vector3(
                targetAngle,
                mainCamera.transform.eulerAngles.y,
                mainCamera.transform.eulerAngles.z
            );
        }
        else
        {
            mainCamera.transform.eulerAngles = new Vector3(
                _originalXAngle,
                mainCamera.transform.eulerAngles.y,
                mainCamera.transform.eulerAngles.z
            );
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right arrows)
        float moveZ = Input.GetAxis("Vertical"); // Get vertical input (W/S or Up/Down arrows)

        Vector3 force = new Vector3(moveX, 0, moveZ) * moveForce; // Calculate the force vector based on input
        _rb.AddForce(force); // Apply the force to the ball's Rigidbody
    }
}
