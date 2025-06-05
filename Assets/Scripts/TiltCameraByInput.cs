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

    private float _currentZ;
    private float _currentX;
    private float _currentYOffset;
    private float _currentSurfaceTilt;
    private bool _wasFalling = false;
    private Vector3 _frozenPosition;

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

            _currentZ = Mathf.Lerp(_currentZ, targetZ, deltaTime * smoothSpeed);
            _currentX = Mathf.Lerp(_currentX, targetX, deltaTime * smoothSpeed);

            // Smooth tilt from surface angle
            float surfaceAngle = angleDetector != null ? angleDetector.surfaceAngle : 0f;

            float targetSurfaceTilt = surfaceAngle * 0.3f;
            _currentSurfaceTilt = Mathf.Lerp(_currentSurfaceTilt, targetSurfaceTilt, deltaTime * smoothSpeed);

            // Total X tilt: base + input + terrain influence
            float totalX = baseXRotation + _currentX + _currentSurfaceTilt;

            // Apply tilt to camera rotation
            Quaternion tilt = Quaternion.Euler(totalX, 0f, _currentZ);
            state.RawOrientation = state.RawOrientation * tilt;

            // Smooth Y offset change based on surface angle
            float targetYOffset = baseYOffset + (surfaceAngle * heightAdjustmentMultiplier);
            _currentYOffset = Mathf.Lerp(_currentYOffset, targetYOffset, deltaTime * smoothSpeed);

            // Apply modified height
            Vector3 pos = state.RawPosition;
            pos.y = pos.y + (_currentYOffset - baseYOffset);
            state.RawPosition = pos;

            //Handle falling state
            //if player is falling, freeze position but still look at player
            if (GameManager.PlayerFallen)
            {
                // Get freeze position once
                if (!_wasFalling)
                {
                    _frozenPosition = state.RawPosition;
                    _wasFalling = true;
                }

                // Keep frozen position
                state.RawPosition = _frozenPosition;

                // Adjust camera to look at player
                if (vcam.LookAt != null)
                {
                    Vector3 toTarget = (vcam.LookAt.position - _frozenPosition).normalized;
                    Quaternion lookRot = Quaternion.LookRotation(toTarget, Vector3.up);

                    // Apply the look rotation
                    Quaternion altTilt = Quaternion.Euler(totalX, 0f, _currentZ);
                    state.RawOrientation = lookRot * altTilt;
                }
            }
            else
            {
                _wasFalling = false; // Reset falling state

                //Apply modified height
                Vector3 altPos = state.RawPosition;
                altPos.y = pos.y + (_currentYOffset - baseYOffset);
                state.RawPosition = altPos;
            }
        }
    }
}
