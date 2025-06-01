using UnityEngine;
using Unity.Cinemachine;

[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide from Add Component menu
public class TiltCameraByInput : CinemachineExtension
{
    public float maxZAngle = 15f;
    public float maxXAngle = 5f;
    public float smoothSpeed = 15f;

    private float currentZ;
    private float currentX;

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

            float targetZ = horizontal * maxZAngle;
            float targetX = -vertical * maxXAngle;

            currentZ = Mathf.Lerp(currentZ, targetZ, deltaTime * smoothSpeed);
            currentX = Mathf.Lerp(currentX, targetX, deltaTime * smoothSpeed);

            Quaternion tilt = Quaternion.Euler(currentX, 0f, currentZ);
            state.RawOrientation = state.RawOrientation * tilt;
        }
    }
}
