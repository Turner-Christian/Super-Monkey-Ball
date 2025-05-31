using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Rigidbody targetRb;
    public float followSmooth = 0.15f;
    public float lookSmooth = 0.1f;

    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 velocityXZ = new Vector3(targetRb.linearVelocity.x, 0, targetRb.linearVelocity.z);
        Vector3 direction =
            velocityXZ.sqrMagnitude > 0.1f ? velocityXZ.normalized : transform.forward;

        // Only rotate around the Y axis (no vertical tilt)
        Quaternion flatLookRotation = Quaternion.LookRotation(direction, Vector3.up);
        Vector3 horizontalOffset = new Vector3(offset.x, 0, offset.z);
        Vector3 rotatedOffset = flatLookRotation * horizontalOffset;

        // Re-apply the original vertical offset (camera height)
        rotatedOffset.y = offset.y;

        Vector3 desiredPosition = target.position + rotatedOffset;
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            followSmooth
        );

        // Smoothly rotate to look at the player (with level Y-axis)
        Vector3 flatTarget = new Vector3(
            target.position.x,
            transform.position.y,
            target.position.z
        );
        Quaternion desiredRotation = Quaternion.LookRotation(flatTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, lookSmooth);
    }
}
