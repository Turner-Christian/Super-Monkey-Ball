using UnityEngine;
using Unity.Cinemachine;

[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide from Add Component menu
public class TiltCameraByInput : CinemachineExtension
{
    public SurfaceAngle angleDetector; // Reference to SurfaceAngle script

    public float maxZAngle = 15f;
    public float maxXAngle = 5f;
    public float smoothSpeed = 15f;

    public float baseXRotation = 20f;
    public float baseYOffset = 1f;
    public float heightAdjustmentMultiplier = 0.05f;

    private float currentZ;
    private float currentX;
    private float currentYOffset;
    private float currentSurfaceTilt;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {
        if (stage == CinemachineCore.Stage.Finalize)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Smooth tilt by input
            float targetZ = horizontal * maxZAngle;
            float targetX = -vertical * maxXAngle;

            currentZ = Mathf.Lerp(currentZ, targetZ, deltaTime * smoothSpeed);
            currentX = Mathf.Lerp(currentX, targetX, deltaTime * smoothSpeed);

            // Smooth tilt from surface angle
            float surfaceAngle = angleDetector != null ? angleDetector.surfaceAngle : 0f;
            float targetSurfaceTilt = surfaceAngle * 0.3f;
            currentSurfaceTilt = Mathf.Lerp(currentSurfaceTilt, targetSurfaceTilt, deltaTime * smoothSpeed);

            // Total X tilt: base + input + terrain influence
            float totalX = baseXRotation + currentX + currentSurfaceTilt;

            // Apply tilt to camera rotation
            Quaternion tilt = Quaternion.Euler(totalX, 0f, currentZ);
            state.RawOrientation = state.RawOrientation * tilt;

            // Smooth Y offset change based on surface angle
            float targetYOffset = baseYOffset + (surfaceAngle * heightAdjustmentMultiplier);
            currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, deltaTime * smoothSpeed);

            // Apply modified height
            Vector3 pos = state.RawPosition;
            pos.y = pos.y + (currentYOffset - baseYOffset);
            state.RawPosition = pos;
        }
    }
}
