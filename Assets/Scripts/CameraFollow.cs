using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow
    private Vector3 offset; // Offset from the target

    void Start()
    {
        offset = transform.position - target.position; // Calculate initial offset
    }

    void Update()
    {
        transform.position = target.position + offset; // Update camera position to follow the target
    }
}
