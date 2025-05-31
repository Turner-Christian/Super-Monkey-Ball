using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float maxZAngle = 15f; // Maximum angle for Z-axis rotation
    public float smoothSpeed = 15f; // Smoothing speed
    private float _originalXAngle;
    private float _originalZAngle;
    private float _currentXAngle;
    private float _currentZAngle;

    void Start()
    {
        _originalZAngle = transform.eulerAngles.z;
        _originalXAngle = transform.eulerAngles.x;
        _currentXAngle = _originalXAngle;
        _currentZAngle = _originalZAngle;
    }

    void Update()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        Vector3 rot = transform.eulerAngles;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float targetZAngle =
            (horizontalInput != 0)
                ? Mathf.Clamp(maxZAngle * horizontalInput, -maxZAngle, maxZAngle)
                : _originalZAngle;

        float targetXAngle = _originalXAngle + (-verticalInput * 5f);

        // Smoothly interpolate angles
        _currentXAngle = Mathf.LerpAngle(
            _currentXAngle,
            targetXAngle,
            Time.deltaTime * smoothSpeed
        );
        _currentZAngle = Mathf.LerpAngle(
            _currentZAngle,
            targetZAngle,
            Time.deltaTime * smoothSpeed
        );

        transform.eulerAngles = new Vector3(_currentXAngle, rot.y, _currentZAngle);
    }
}
