using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    [Header("Normal Camera")]
    public float distance = 6f;
    public float height = 2f;
    public float sideOffset = 0f;

    [Header("Aim Camera")]
    public float aimDistance = 2.2f;
    public float aimHeight = 1.7f;
    public float aimSideOffset = 0.9f;
    public float aimLookForward = 8f;

    [Header("Mouse Sensitivity")]
    public float mouseSensitivity = 3f;

    [Header("Vertical Rotation Limit")]
    public float minYAngle = -20f;
    public float maxYAngle = 60f;

    [Header("Smooth")]
    public float smoothSpeed = 12f;

    [Header("Camera Collision")]
    public LayerMask collisionLayerMask = ~0;
    public float collisionOffset = 0.35f;

    private float rotationX = 0f;
    private float rotationY = 20f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        bool isAiming = Input.GetMouseButton(1);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX += mouseX;
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0f);

        float currentDistance = isAiming ? aimDistance : distance;
        float currentHeight = isAiming ? aimHeight : height;
        float currentSideOffset = isAiming ? aimSideOffset : sideOffset;

        Vector3 pivotPosition = target.position + Vector3.up * currentHeight;

        Vector3 desiredPosition =
            pivotPosition +
            rotation * new Vector3(currentSideOffset, 0f, -currentDistance);

        if (Physics.Linecast(
            pivotPosition,
            desiredPosition,
            out RaycastHit hit,
            collisionLayerMask,
            QueryTriggerInteraction.Ignore))
        {
            desiredPosition = hit.point + hit.normal * collisionOffset;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        if (isAiming)
        {
            // Mode aim: kamera melihat ke depan, bukan ke badan player
            Vector3 aimLookTarget = pivotPosition + rotation * Vector3.forward * aimLookForward;

            Quaternion aimRotation = Quaternion.LookRotation(aimLookTarget - transform.position);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                aimRotation,
                smoothSpeed * Time.deltaTime
            );
        }
        else
        {
            // Mode normal: kamera selalu lock LookAt ke player
            Vector3 normalLookTarget = target.position + Vector3.up * height;

            Quaternion normalRotation = Quaternion.LookRotation(normalLookTarget - transform.position);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                normalRotation,
                smoothSpeed * Time.deltaTime
            );
        }
    }
}