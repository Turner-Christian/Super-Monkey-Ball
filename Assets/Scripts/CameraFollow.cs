using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow
    public float yOffsetAdjustment = 0f; // Allow runtime adjustment to Y offset

    private Vector3 initialOffset;

    void Start()
    {
        initialOffset = transform.position - target.position;
    }

    void Update()
    {
        Vector3 adjustedOffset = new Vector3(
            initialOffset.x,
            initialOffset.y + yOffsetAdjustment,
            initialOffset.z
        );

        transform.position = target.position + adjustedOffset;
    }
}
