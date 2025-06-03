using UnityEngine;

public class SurfaceAngle : MonoBehaviour
{
    public float surfaceAngle;
    public float rayLength = 3f;
    public LayerMask groundMask;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, groundMask))
        {
            // Calculate the angle between the surface normal and the up vector
            surfaceAngle = Vector3.Angle(hit.normal, Vector3.up);
            // Debug.Log("Surface Angle: " + surfaceAngle);
        }
        else
        {
            // Reset angle if no surface is detected
            surfaceAngle = 0f;
            Debug.Log("No surface detected within ray length.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    }
}
