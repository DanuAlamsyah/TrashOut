using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpSound;
    [Range(0f, 3f)] public float jumpVolume = 2f;

    private Animator animator;
    private CharacterController characterController;
    private float verticalVelocity;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f; // 0 = suara 2D, tidak mengecil karena jarak
            audioSource.volume = 1f;
        }

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W)) vertical = 1f;
        if (Input.GetKey(KeyCode.S)) vertical = -1f;
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        if (cameraTransform == null)
        {
            Debug.LogWarning("Camera Transform belum diisi di PlayerMovement.");
            return;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = cameraForward * vertical + cameraRight * horizontal;
        direction.Normalize();

        bool isMoving = direction.magnitude > 0f;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && isMoving;

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 move = direction * currentSpeed;

        // Gravity
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Suara jump
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound, jumpVolume);
            }
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);

        if (isMoving)
        {
            // Saat mundur, jangan putar badan
            if (vertical >= 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        if (animator != null)
        {
            animator.SetBool("isRunning", isMoving);
        }
    }
}
