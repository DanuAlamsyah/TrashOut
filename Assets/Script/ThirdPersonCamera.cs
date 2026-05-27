using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    [Header("Camera Distance")]
    public float distance = 6f;
    public float height = 2f;

    [Header("Mouse Sensitivity")]
    public float mouseSensitivity = 3f;

    [Header("Vertical Rotation Limit")]
    public float minYAngle = -20f;
    public float maxYAngle = 60f;

    [Header("Smooth")]
    public float smoothSpeed = 10f;

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

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX += mouseX;
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0f);

        Vector3 targetPosition = target.position + Vector3.up * height;

        // Posisi kamera yang diinginkan
        Vector3 desiredPosition = targetPosition + rotation * new Vector3(0f, 0f, -distance);

        RaycastHit hit;

        // Cek apakah ada objek di antara player dan kamera
        if (Physics.Linecast(targetPosition, desiredPosition, out hit))
        {
            // Geser kamera ke depan sedikit dari dinding
            desiredPosition = hit.point + hit.normal * 0.2f;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(targetPosition);
    }
}