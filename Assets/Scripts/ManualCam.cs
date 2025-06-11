using UnityEngine;

public class ManualCam : MonoBehaviour
{
    public GameObject target; // The target object to follow
    public float OffsetX = 0f; // X offset from the target
    public float OffsetY = 2f; // Y offset from the target
    public float OffsetZ = 0f; // Z offset from the target
    public float xRotation = 0f; // X rotation of the camera

    void Update()
    {
        // Camera follow
        transform.position = new Vector3(
            target.transform.position.x + OffsetX,
            target.transform.position.y + OffsetY,
            target.transform.position.z + OffsetZ
        );
        // Set camera rotation
        transform.rotation = Quaternion.Euler(
            xRotation,
            target.transform.rotation.eulerAngles.y,
            0f
        ); 
    }
}
